using InnoVault.PRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using WeaponReset.Command;
using WeaponReset.Command.GlobalNPCs;
using WeaponReset.Content.Dusts.Particles;
using WeaponReset.Content.General;
using WeaponReset.Content.Weapons.SPAtkSwords;

namespace WeaponReset.Content.Weapons.ShortSwords
{
    public class ShortSwordsProj : BasicMeleeWeaponSword
    {
        public SpurtHelper SpurtHelper;
        public override string Texture => $"Terraria/Images/Item_{ItemID.None}";
        public override void OnSpawn(IEntitySource source)
        {
            SpurtHelper = new SpurtHelper(Projectile);
            base.OnSpawn(source);
        }
        public virtual Color GetColor(float factor)
        {
            switch(SpawnItem.type)
            {
                case ItemID.CopperShortsword:
                    return Color.OrangeRed with { A = 0 } * factor;
                case ItemID.IronShortsword:
                    return Color.Gray with { A = 0 } * factor;
                case ItemID.SilverShortsword:
                    return Color.Silver with { A = 0 } * factor;
                case ItemID.GoldShortsword:
                    return Color.Gold with { A = 0 } * factor;
                case ItemID.PlatinumShortsword:
                    return Color.LightBlue with { A = 0 } * factor;
                case ItemID.LeadShortsword:
                    return Color.Lerp(Color.MediumPurple, Color.Gray, 0.6f) with { A = 0 } * factor;
                case ItemID.TungstenShortsword:
                    return Color.MediumSeaGreen with { A = 0 } * factor;
                case ItemID.TinShortsword:
                    return Color.Silver with { A = 0 } * factor;
            }
            return default;
        }
        public override void AI()
        {
            if (!ShortSwordsItem.CanResetWeapon && SpawnItem?.ModItem?.Mod is not WeaponReset)
            {
                Projectile.Kill();
                return;
            }
            base.AI();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            SpurtHelper.SendData(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            SpurtHelper.ReadData(reader);
        }
        public override void Init()
        {
            OldSkills = new();
            #region 技能创建
            SwingHelper_NoUse noUse = new(Player, SwingHelper, this)
            {
                Length = Projectile.Size.Length()
            };

            SpurtHelper_GeneralOneSpurt generalOneSpurt1 = new(this,
             new()
             {
                 UseTimeMax = GetAttackSeepd,
                 SpurtLength = GetSpurtLength,
                 ActivationCondition = () => Player.controlUseItem,
                 ModifyHit = ModifyHitNPC_ScalingArmorPenetration

             }, SpurtHelper, Player);
            SpurtHelper_GeneralOneSpurt generalOneSpurt2 = new(this,
            new()
            {
                UseTimeMax = GetAttackSeepd,
                SpurtLength = GetSpurtLength,
                ActivationCondition = () => Player.controlUseItem,
                ModifyHit = ModifyHitNPC_ScalingArmorPenetration
            }, SpurtHelper, Player);
            SpurtHelper_GeneralOneSpurt generalOneSpurt3 = new(this,
            new()
            {
                UseTimeMax = GetAttackSeepd,
                SpurtLength = GetSpurtLength,
                ActivationCondition = () => Player.controlUseItem,
                ModifyHit = ModifyHitNPC_ScalingArmorPenetration
            }, SpurtHelper, Player);
            #region 挥舞要求
            SwingHelper_GeneralSwing.Setting.PreDraw DrawProj = null;
            Action<SwingHelper_GeneralSwing> ChangeToRot = null;
            float attackSpeed = 0;
            Func<float, float> swingChange = null;
            Action<NPC, NPC.HitInfo, int> OnHitEffect = null;
            Action<SwingHelper_GeneralSwing> ResetFullRot = null;
            Action<SwingHelper_GeneralSwing> SetFullRot = null;

            DrawProj = (sb, drawColor) =>
            {
                SwingHelper.Swing_Draw_ItemAndTrailling(drawColor, TextureAssets.Extra[201].Value, GetColor);
                return false;
            }; // 绘制弹幕

            swingChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f)); // 缓动函数

            OnHitEffect = (target, hit, damage) =>
            {
                if (Player.whoAmI == Main.myPlayer)
                    Projectile.netUpdate = true;
                if (Main.netMode != NetmodeID.Server) // 仅有本地端允许查看
                {
                    for (int i = -70; i <= 70; i++)
                    {
                        var dust = Dust.NewDustPerfect(target.Center, DustID.FireworksRGB, Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(6.28) * i * 0.1f, 0, GetColor(0.3f), 0.8f);
                        dust.noGravity = false;
                    }

                    var extra98 = PRTLoader.NewParticle<Extra98_OnHit>(target.Center + Main.rand.NextVector2CircularEdge(10,10), Projectile.velocity.SafeNormalize((Projectile.rotation).ToRotationVector2()).RotatedBy(MathHelper.PiOver2) * Projectile.Size.Length() * 0.1f, GetColor(1f) with { A = 0 }, 1f);
                    extra98.Origin = extra98.TexValue.Size() * 0.5f;
                    extra98.Update = (t) =>
                    {
                        t.Rotation = t.Velocity.ToRotation();
                        t.Scale_Vector2 = new Vector2(1f - t.LifetimeCompletion, t.Velocity.Length());
                        return true;
                    };
                    extra98.Draw = (t) =>
                    {
                        Main.spriteBatch.Draw(t.TexValue, t.Position - Main.screenPosition, null, t.Color, t.Rotation, t.Origin, t.Scale_Vector2, SpriteEffects.None, 0);
                        return false;
                    };
                }
                //Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Projectile.velocity.SafeNormalize(default).RotatedByRandom(0.7), 3f, 2f, 2));
                if (Player.velocity.Y != 0)
                    Player.velocity.Y = -10;
                //Player.velocity += (Player.Center - target.Center) * (Projectile.Size.Length() / Math.Max(Projectile.Size.Length() / 4, (Player.Center - target.Center).Length()) * 0.06f);

            }; // 击中效果

            attackSpeed = Player.GetWeaponAttackSpeed(SpawnItem) * (SpawnItem.useTime + SpawnItem.useAnimation) / 2;

            ChangeToRot = (_) => // 改变玩家朝向与剑朝向的旋转
            {
                if (Player.whoAmI != Main.myPlayer) // 其他玩家不处理这个AI
                    return;
                Player.ChangeDir((Main.MouseWorld.X - Player.Center.X > 0).ToDirectionInt());
                SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation()); // 朝向
            };
            ResetFullRot = (_) =>
            {
                Player.fullRotation = 0;
                Player.legRotation = 0;
            };
            SetFullRot = (skill) =>
            {
                Player.fullRotation = MathHelper.Lerp(Player.fullRotation, Projectile.velocity.X / Projectile.Size.Length() * Player.direction * skill.setting.SwingDirectionChange.ToDirectionInt() * 0.15f, 0.1f);
                Player.fullRotationOrigin = new Vector2(Player.width * 0.5f, Player.height);
            };
            #endregion
            SwingHelper_GeneralSwing SwingAcrossDown = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseTile,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 0.6f) * 1.5f, // 速度缩放
                VisualRotation = -0.4f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed * 2, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcrossUp = new(this, // 斜上砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseTile,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 0.6f) * 1.5f, // 速度缩放
                VisualRotation = -0.4f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed * 2, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SpurtHelper_GeneralMoreSpurt generalMoreSpurt = new(this,
             new()
             {
                 UseTimeMax = GetAttackSeepd,
                 SpurtLength = GetSpurtLength,
                 ActivationCondition = () => Player.controlUseTile,
                 DmgConut = 10,
                 angleFix = 1.1f,
                 centerOffset = 40,
                 OnHit = (t, hit, damage) => OnHitEffect?.Invoke(t, hit, damage),

             }, SpurtHelper, Player);

            SpurtHelper_GeneralOneSpurt DashSpurt = new(this,
             new()
             {
                 UseTimeMax = GetAttackSeepd,
                 SpurtLength = GetSpurtLength,
                 ActivationCondition = () => Player.controlUseItem,
                 OnHit = OnHitNPC_AddVulnerable,
                 ModifyHit = ModifyHitNPC_ScalingArmorPenetration

             }, SpurtHelper, Player)
            {
                OnUse = (skill) =>
                {
                    if (Projectile.ai[0] < GetAttackSeepd() * 0.5f && Main.myPlayer == Player.whoAmI)
                    {
                        Player.velocity = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(default) * 60;
                        TheUtility.SetPlayerImmune(Player, 2);
                    }
                    else if (Main.myPlayer == Player.whoAmI)
                        Player.velocity *= 0.5f;
                    if ((int)Projectile.ai[0] == 0)
                        Projectile.damage *= 3;
                }
            };
            #endregion
            #region 技能连接
            SwingAcrossDown.AddSkill(DashSpurt);
            SwingAcrossUp.AddSkill(generalMoreSpurt);
            noUse.AddSkill(SwingAcrossDown).AddSkill(SwingAcrossUp).AddSkill(generalOneSpurt1);
            noUse.AddSkill(generalOneSpurt1).AddSkill(generalOneSpurt2).AddSkill(generalOneSpurt3);
            #endregion
            #region 技能注册
            (this as IBasicSkillProj).Register(noUse, generalOneSpurt1, generalOneSpurt2, generalOneSpurt3, SwingAcrossDown, SwingAcrossUp, generalMoreSpurt, DashSpurt);
            #endregion
            CurrentSkill = noUse;
        }
        public virtual void OnHitNPC_AddVulnerable(NPC target, NPC.HitInfo hit,int dmgDone)
        {
            target.GetGlobalNPC<GeneralGlobalNPC>().Vulnerable.Strength += 2;
            target.GetGlobalNPC<GeneralGlobalNPC>().Vulnerable.Conuts += 5;
        }
        public virtual void ModifyHitNPC_ScalingArmorPenetration(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player.GetArmorPenetration(SpawnItem.DamageType) += SpawnItem.damage * 1.5f;
        }
        public virtual float GetSpurtLength() => Projectile.Size.Length() * 6.5f;
        public virtual int GetAttackSeepd() => (int)((SpawnItem.useTime + SpawnItem.useAnimation) / 2 / Player.GetWeaponAttackSpeed(SpawnItem));
    }
}
