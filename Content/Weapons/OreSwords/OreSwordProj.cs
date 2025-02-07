using System.IO;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using WeaponReset.Command;
using WeaponReset.Content.General;

namespace WeaponReset.Content.Weapons.OreSwords
{
    public class OreSwordProj : BasicMeleeWeaponSword
    {
        public float DamageAdd;
        public override string Texture => $"Terraria/Images/Item_{ItemID.None}";
        public Color GetColor(float factor)
        {
            switch (SpawnItem.type)
            {
                case ItemID.CopperBroadsword:
                    return Color.OrangeRed with { A = 0 } * factor;
                case ItemID.IronBroadsword:
                    return Color.Gray with { A = 0 } * factor;
                case ItemID.SilverBroadsword:
                    return Color.Silver with { A = 0 } * factor;
                case ItemID.GoldBroadsword:
                    return Color.Gold with { A = 0 } * factor;
                case ItemID.PlatinumBroadsword:
                    return Color.LightBlue with { A = 0 } * factor;
                case ItemID.LeadBroadsword:
                    return Color.Lerp(Color.MediumPurple, Color.Gray, 0.6f) with { A = 0 } * factor;
                case ItemID.TungstenBroadsword:
                    return Color.MediumSeaGreen with { A = 20 } * factor;
                case ItemID.TinBroadsword:
                    return Color.Silver with { A = 20 } * factor;
            }
            return default;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.SourceDamage += 1.5f;
        }
        public override void AI()
        {
            if (Player != null && Player.itemTimeMax <= 2)
            {
                Projectile.Kill();
                return;
            }
            base.AI();
        }
        /// <summary>
        /// 这里是用于发射弹幕
        /// </summary>
        public virtual void OnAtk(SwingHelper_GeneralSwing swingHelper_GeneralSwing)
        {
            //    if ((int)Projectile.ai[1] == (int)swingHelper_GeneralSwing.onAtk.SwingTime / 4 * 3 * (Projectile.extraUpdates + 1) && Main.myPlayer == Player.whoAmI)
            //        Projectile.NewProjectile(new SwordQi.SwordQiSource(SwingHelper.Clone() as SwingHelper, GetColor), Projectile.Center, (Main.MouseWorld - Projectile.Center).SafeNormalize(default) * 10, ModContent.ProjectileType<SwordQi>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
        public virtual void LastModifyHit(NPC target, ref NPC.HitModifiers hitModifiers)
        {
            hitModifiers.SourceDamage += DamageAdd;
            hitModifiers.FinalDamage *= 2;
        }
        public virtual void NoHitChange(SwingHelper_GeneralSwing swingHelper_GeneralSwing)
        {
            if (Projectile.numHits > 0)
                return;
            DamageAdd += 1.5f;
        }
        public override void Init()
        {
            OldSkills = new();

            NoUse noUse = new(Player, SwingHelper, this)
            {
                Length = Projectile.Size.Length()
            }; // 玩家拿在手上不使用的时候

            SwingHelper_GeneralSwing.Setting.PreDraw drawProj = (sb, drawColor) =>
            {
                SwingHelper.Swing_Draw_ItemAndTrailling(drawColor, TextureAssets.Extra[201].Value, GetColor);
                return false;
            }; // 绘制弹幕

            Func<float, float> swingChange = (time) => MathHelper.SmoothStep(0, 1f, time); // 缓动函数

            Action<NPC, NPC.HitInfo, int> onHitEffect = (target, hit, damage) =>
            {
                for (int i = 0; i <= 30; i++)
                {
                    var dust = Dust.NewDustPerfect(target.Center, DustID.Smoke, Projectile.velocity.RotatedBy(MathHelper.PiOver2 * Player.direction).RotatedByRandom(0.3).SafeNormalize(default) * i * 0.5f, 0, GetColor(1), 0.8f);
                    dust.noGravity = true;

                    //dust = Dust.NewDustPerfect(target.Center, DustID.CrystalPulse, Projectile.velocity.SafeNormalize(default) * i * 0.5f, 100, Color.Purple, 0.8f);
                    //dust.noGravity = true;
                }
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Projectile.velocity.SafeNormalize(default).RotatedByRandom(0.7), 3f, 2f, 2));
                if (Player.velocity.Y != 0)
                    Player.velocity.Y = -10;
                //Player.velocity += (Player.Center - target.Center) * (Projectile.Size.Length() / Math.Max(Projectile.Size.Length() / 4, (Player.Center - target.Center).Length()) * 0.06f);

            }; // 击中效果

            float attackSpeed = Player.GetWeaponAttackSpeed(SpawnItem) * 20;

            Action<SwingHelper_GeneralSwing> ChangeToRot = (_) => // 改变玩家朝向与剑朝向的旋转
                            {
                                if (Player.whoAmI != Main.myPlayer) // 其他玩家不处理这个AI
                                    return;
                                Player.ChangeDir((Main.MouseWorld.X - Player.Center.X > 0).ToDirectionInt());
                                SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation()); // 朝向
                            };
            Action<SwingHelper_GeneralSwing> ResetFullRot = (_) =>
            {
                Player.fullRotation = 0;
                Player.legRotation = 0;
            };
            Action<SwingHelper_GeneralSwing> SetFullRot = (skill) =>
            {
                Player.fullRotation = MathHelper.Lerp(Player.fullRotation, Projectile.velocity.X / Projectile.Size.Length() * Player.direction * skill.setting.SwingDirectionChange.ToDirectionInt() * 0.15f, 0.1f);
                Player.fullRotationOrigin = new Vector2(Player.width * 0.5f,Player.height);
            };

            #region 创建技能
            SwingHelper_GeneralSwing SwingUp = new(this, // 上斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 1), // 速度缩放
                VisualRotation = 0, // 视觉朝向
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
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = (target, hit, damage) =>
                {
                    onHitEffect.Invoke(target, hit, damage);
                    if (target.knockBackResist != 0)
                        target.velocity.Y = -5f; // 击飞
                },
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcrossDown = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 0.6f), // 速度缩放
                VisualRotation = 0.4f, // 视觉朝向
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
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = onHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcross = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(0.4f),// 起始速度朝向
                VelScale = new Vector2(1.3f, 0.3f), // 速度缩放
                VisualRotation = 0.7f, // 视觉朝向
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
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = onHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingDown = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1.5f, 1.5f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
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
            },
            onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = onHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot
            }, SwingHelper, Player);

            SwingHelper_CountSwing StorngSlash = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(2f, 0.8f), // 速度缩放
                VisualRotation = 0.8f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 60, // 前摇时间
                OnUse = (_) =>
                {
                    if (Player.controlUseItem && Projectile.ai[1] >= 58)
                    {
                        Projectile.ai[1] = 58;
                    }
                },
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, 
            onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed / 3, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = onHitEffect,
                OnChange = (skill) =>
                {
                    ResetFullRot.Invoke(skill);
                    //if (Projectile.ai[2] > 0)
                    //{
                    //    skill.setting.StartVel.Y = -skill.setting.StartVel.Y;
                    //    skill.setting.SwingDirectionChange = !skill.setting.SwingDirectionChange;
                    //}
                },
                OnUse = SetFullRot
            }, SwingHelper, Player,1);


            SwingHelper_GeneralSwing ChannelSwing = new(this, // 上斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseTile,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 1), // 速度缩放
                VisualRotation = 0, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnUse = (skill) =>
                {
                    if (skill.setting.ChangeCondition.Invoke())
                    {
                        Projectile.ai[1] = 0;
                    }
                },
                OnChange = (_) =>
                {
                    if (Player.whoAmI != Main.myPlayer) // 其他玩家不处理这个AI
                        return;
                    Player.ChangeDir((Main.MouseWorld.X - Player.Center.X > 0).ToDirectionInt());
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation()); // 朝向
                }
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed / 6, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = onHitEffect,
                OnChange = (_) =>
                {
                    Player.fullRotation = 0;
                    Player.legRotation = 0;

                },
                OnUse = (_) =>
                {
                    Player.fullRotation = MathHelper.Lerp(Player.fullRotation, Player.direction * 0.4f, 0.1f);
                    //Player.legRotation = -Player.fullRotation;
                    Player.fullRotationOrigin = Player.Size * 0.5f;
                }

            }, SwingHelper, Player);

            SwingUp.preAtk.OnChange += (_) => DamageAdd = 0;
            SwingUp.onAtk.OnChange += NoHitChange;
            SwingAcrossDown.onAtk.OnChange += NoHitChange;
            SwingAcross.onAtk.OnChange += NoHitChange;
            SwingDown.onAtk.OnChange += NoHitChange;
            StorngSlash.onAtk.ModifyHit += LastModifyHit;
            #endregion
            #region 连接技能
            noUse.AddSkill(SwingUp).AddSkill(SwingAcrossDown).AddSkill(SwingAcross).AddSkill(SwingDown).AddSkill(StorngSlash);
            #endregion
            CurrentSkill = noUse;
        }
    }
}