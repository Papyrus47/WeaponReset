using System.IO;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using WeaponReset.Content.General;

namespace WeaponReset.Content.Weapons.OreSwords
{
    public class OreSwordProj : BasicMeleeWeaponSword
    {
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
                    return Color.Lerp(Color.MediumPurple,Color.Gray,0.6f) with { A = 0 } * factor;
                case ItemID.TungstenBroadsword:
                    return Color.MediumSeaGreen with { A = 20 } * factor;
                case ItemID.TinBroadsword:
                    return Color.Silver with { A = 20 } * factor;
            }    
            return default;
        }
        public override void AI()
        {
            if (Player.itemTimeMax <= 0)
            {
                Projectile.Kill();
                return;
            }
            base.AI();
        }
        /// <summary>
        /// 这里是用于发射弹幕
        /// </summary>
        public virtual void OnChange(SwingHelper_GeneralSwing swingHelper_GeneralSwing)
        {

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
                Player.velocity += (Player.Center - target.Center) * (Projectile.Size.LengthSquared() / (Player.Center - target.Center).LengthSquared() * 0.02f);

            }; // 击中效果

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
                SwingTime = Player.itemTimeMax, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = (target, hit, damage) =>
                {
                    onHitEffect.Invoke(target, hit, damage);
                    if (target.knockBackResist != 0)
                        target.velocity.Y = -5f; // 击飞
                },
                OnChange = (_) =>
                {
                    Player.fullRotation = 0;
                    Player.legRotation = 0;

                },
                OnUse = (_) =>
                {
                    Player.fullRotation = MathHelper.Lerp(Player.fullRotation, Player.direction * 0.4f, 0.1f);
                    Player.legRotation = -Player.fullRotation;
                    Player.fullRotationOrigin = Player.Size * 0.5f;
                }

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcross = new(this, // 横斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 0.3f), // 速度缩放
                VisualRotation = 0.7f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
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
                PostMaxTime = 15, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = Player.itemTimeMax, // 挥舞时间
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
                    Player.legRotation = -Player.fullRotation;
                    Player.fullRotationOrigin = Player.Size * 0.5f;
                }
            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingDown = new(this, // 下挥
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1, 1f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
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
                PostMaxTime = 20, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = Player.itemTimeMax, // 挥舞时间
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
                    Player.legRotation = -Player.fullRotation;
                    Player.fullRotationOrigin = Player.Size * 0.5f;
                }
            }, SwingHelper, Player);

            SwingHelper_GeneralSwing Spurt = new(this, // 突刺
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi, // 挥舞角度
                preDraw = drawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = -Vector2.UnitX,// 起始速度朝向
                VelScale = new Vector2(1, 0.0001f), // 速度缩放
                VisualRotation = 0, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
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
                PostMaxTime = 60, // 后摇最大时间
                PostAtkTime = 10, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = Player.itemTimeMax / 2, // 挥舞时间
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
                    Player.legRotation = -Player.fullRotation;
                    Player.fullRotationOrigin = Player.Size * 0.5f;
                }
            }, SwingHelper, Player);

            #region 废弃代码
            //SwingHelper_CountSwing MoreSlash = new(this, // 突刺
            //setting: new() // 设置
            //{
            //    SwingLenght = Projectile.Size.Length(),// 挥舞长度
            //    ChangeCondition = () => Player.controlUseTile && Player.velocity.Y != 0,
            //    SwingRot = MathHelper.Pi, // 挥舞角度
            //    preDraw = drawProj,
            //    SwingDirectionChange = true, // 挥舞方向变化
            //    StartVel = -Vector2.UnitY,// 起始速度朝向
            //    VelScale = new Vector2(1, 1f), // 速度缩放
            //    VisualRotation = 0, // 视觉朝向
            //},
            //preAtk: new() // 攻击前
            //{
            //    PreTime = 3, // 前摇时间
            //    OnChange = (_) =>
            //    {
            //        Projectile.damage /= 3;
            //        if (Player.whoAmI != Main.myPlayer) // 其他玩家不处理这个AI
            //            return;
            //        Player.ChangeDir((Main.MouseWorld.X - Player.Center.X > 0).ToDirectionInt());
            //        SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation()); // 朝向
            //    }
            //},
            //postAtk: new() // 攻击后
            //{
            //    PostMaxTime = 60, // 后摇最大时间
            //    PostAtkTime = 10, // 后摇切换时间
            //}, onAtk: new() // 攻击时
            //{
            //    SwingTime = Player.itemTimeMax / 4, // 挥舞时间
            //    TimeChange = swingChange, // 时间变化函数
            //    OnHit = onHitEffect,
            //    OnChange = (weapon) =>
            //    {
            //        if (Projectile.ai[2] > 0) // 剩余挥击次数大于0
            //        {
            //            weapon.setting.StartVel = -weapon.setting.StartVel;
            //            SwingHelper.Change(weapon.setting.StartVel, weapon.setting.VelScale);
            //            weapon.setting.SwingDirectionChange = !weapon.setting.SwingDirectionChange;
            //        }
            //        Player.fullRotation = 0;
            //        Player.legRotation = 0;
            //    },
            //    OnUse = (_) =>
            //    {
            //        Player.fullRotation = MathHelper.Lerp(Player.fullRotation, Player.direction * 0.4f, 0.1f);
            //        Player.legRotation = -Player.fullRotation;
            //        Player.fullRotationOrigin = Player.Size * 0.5f;
            //    }
            //}, SwingHelper, Player,5);

            //MoreSlash.AddSkill(SwingUp);
            //MoreSlash.AddBySkill(noUse, SwingUp, SwingAcross, SwingDown, Spurt);
            #endregion
            SwingUp.onAtk.OnChange += OnChange;
            SwingAcross.onAtk.OnChange += OnChange;
            SwingDown.onAtk.OnChange += OnChange;
            Spurt.onAtk.OnChange += OnChange;

            noUse.AddSkill(SwingUp).AddSkill(SwingAcross).AddSkill(SwingDown).AddSkill(Spurt).AddSkill(SwingUp);
            CurrentSkill = noUse;
        }
    }
}
