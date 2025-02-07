using InnoVault.PRT;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using WeaponReset.Command;
using WeaponReset.Content.Dusts.Particles;
using WeaponReset.Content.General;

namespace WeaponReset.Content.Weapons.OreSwords
{
    public class OreSwordProj : BasicMeleeWeaponSword
    {
        public Action<SwingHelper_GeneralSwing> SetFullRot;
        public Action<SwingHelper_GeneralSwing> ResetFullRot;
        public Action<SwingHelper_GeneralSwing> ChangeToRot;
        public Action<SwingHelper_GeneralSwing> MoveSlash;
        public Action<NPC, NPC.HitInfo, int> OnHitEffect;
        public SwingHelper_GeneralSwing.Setting.PreDraw DrawProj;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
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
                case ItemID.WoodenSword:
                case ItemID.PalmWoodSword:
                case ItemID.AshWoodSword:
                case ItemID.BorealWoodSword:
                case ItemID.ShadewoodSword:    // 暗影木剑
                case ItemID.EbonwoodSword:      // 黑木剑
                    return Color.Brown with { A = 0 } * factor;
                case ItemID_Chinese.骨剑:
                    return Color.WhiteSmoke with { A = 0 } * factor;
                case ItemID_Chinese.颌骨剑:
                    return Color.Yellow with { A = 0 } * factor;
                case ItemID_Chinese.糖棒剑:
                    return Color.WhiteSmoke with { A = 0 } * factor;
                case ItemID_Chinese.橙陨石光剑:
                    return Color.Orange with { A = 0 } * factor;
                case ItemID_Chinese.白陨石光剑:
                    return Color.White with { A = 0 } * factor;
                case ItemID_Chinese.红陨石光剑:
                    return Color.Red with { A = 0 } * factor;
                case ItemID_Chinese.黄陨石光剑:
                    return Color.Yellow with { A = 0 } * factor;
                case ItemID_Chinese.蓝陨石光剑:
                    return Color.Blue with { A = 0 } * factor;
                case ItemID_Chinese.紫陨石光剑:
                    return Color.Purple with { A = 0 } * factor;
                case ItemID_Chinese.绿陨石光剑:
                    return Color.Green with { A = 0 } * factor;
                case ItemID_Chinese.火山:
                    return Color.OrangeRed with { A = 0 } * factor;
                case ItemID_Chinese.紫挥棒鱼:
                    return Color.Purple with { A = 0 } * factor;
            }
            return default;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.SourceDamage += 1.5f;
        }
        //public override void AI()
        //{
        //    base.AI();
        //}
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
        public virtual void Defence(Player player, Entity target, ref Player.HurtModifiers hurtModifiers)
        {
            if(Projectile == null || !Projectile.active || Projectile.ModProjectile is not OreSwordProj)
            {
                player.GetModPlayer<WeaponResetPlayer>().OnModifyByHit -= Defence;
                return;
            }
            if(CurrentSkill is SwingHelper_Defence skill && player.GetModPlayer<WeaponResetPlayer>().PlayerImmune <= 0) // 处于防御姿态
            {
                if (Projectile.ai[0] == 1 && Projectile.ai[1] < 20 * (Projectile.extraUpdates + 1)) // 掏出剑的时候
                {
                    hurtModifiers.SourceDamage *= 0;
                    player.GetModPlayer<WeaponResetPlayer>().OreSwordDef = 600;
                    player.GetModPlayer<WeaponResetPlayer>().PlayerImmune = 120;
                    SoundEngine.PlaySound(SoundID.NPCHit4 with { Pitch = 0.4f }, Player.position);
                }
                else
                {
                    player.GetModPlayer<WeaponResetPlayer>().OreSwordDef = 300;
                    hurtModifiers.SourceDamage *= 0.2f;
                    SoundEngine.PlaySound(SoundID.NPCHit4 with { Pitch = -0.4f }, Player.position);
                }

                if (player.whoAmI == Main.myPlayer)
                    player.velocity = (player.Center - Main.MouseWorld).SafeNormalize(default) * 15;
            }
            player.GetModPlayer<WeaponResetPlayer>().OnModifyByHit -= Defence;
        }
        public override void Init()
        {
            OldSkills = new();

            NoUse noUse = new(Player, SwingHelper, this)
            {
                Length = Projectile.Size.Length()
            }; // 玩家拿在手上不使用的时候
            #region 各种事件 
            DrawProj = (sb, drawColor) =>
            {
                SwingHelper.Swing_Draw_ItemAndTrailling(drawColor, TextureAssets.Extra[201].Value, GetColor);
                return false;
            }; // 绘制弹幕

            Func<float, float> swingChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time,2.5f)); // 缓动函数

            OnHitEffect = (target, hit, damage) =>
            {
                if(Player.whoAmI == Main.myPlayer)
                    Projectile.netUpdate = true;
                if (Main.netMode != NetmodeID.Server) // 仅有本地端允许查看
                {
                    for (int i = -70; i <= 70; i++)
                    {
                        var dust = Dust.NewDustPerfect(target.Center, DustID.FireworksRGB, Projectile.velocity.RotatedByRandom(6.28) * i * 0.0005f, 0, GetColor(0.3f), 0.8f);
                        dust.noGravity = false;
                    }

                    var extra98 = PRTLoader.NewParticle<Extra98_OnHit>(target.Center, Projectile.velocity * 0.05f, GetColor(1), 1f);
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
            Action<NPC, NPC.HitInfo, int> StrongPunchCamera = (_, _, _) =>
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Projectile.velocity.SafeNormalize(default).RotatedByRandom(0.7), 7f, 15f, 15));
            };

            float attackSpeed = Player.GetWeaponAttackSpeed(SpawnItem) * SpawnItem.useTime;

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

            MoveSlash = (skill) =>
            {
                if (Projectile.ai[1] < skill.onAtk.SwingTime * (Projectile.extraUpdates + 1) - 2)
                {
                    Player.velocity.X = Player.direction * 60;
                    if (Projectile.ai[1] < skill.onAtk.SwingTime * (Projectile.extraUpdates + 1) * 0.5f) // 给予无敌帧
                    {
                        TheUtility.SetPlayerImmune(Player, 2);
                    }
                }
                else
                    Player.velocity.X *= 0.001f;
            };
            #endregion

            #region 创建技能
            SwingHelper_GeneralSwing SwingUp = new(this, // 上斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
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
                    OnHitEffect.Invoke(target, hit, damage);
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
                preDraw = DrawProj,
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
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcross = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
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
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingDown = new(this, // 横砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
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
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot
            }, SwingHelper, Player);

            SwingHelper_CountSwing StorngSlash = new(this, // 下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(2f, 0.8f), // 速度缩放
                VisualRotation = 0.8f, // 视觉朝向
                OnHitStopTime = 4, // 击中停止时间
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
                OnHit = OnHitEffect + StrongPunchCamera,
                OnChange = ResetFullRot,
                OnUse = SetFullRot
            }, SwingHelper, Player, 1);

            SwingHelper_Defence Def = new(this, // 防御
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(), // 挥舞长度
                ChangeCondition = () => Player.controlUseTile && OreSwordItems.DefCD <= 0, // 右键防御
                SwingRot = 0, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f), // 起始速度朝向
                VelScale = new Vector2(1f, 1f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 5, // 前摇时间
                OnChange = ChangeToRot,
                OnUse = (_) =>
                {
                    OreSwordItems.DefCD = OreSwordItems.DefCDMax;
                }
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 2, // 后摇最大时间
                PostAtkTime = 1, // 后摇切换时间
            },
            onAtk: new() // 攻击时
            {
                SwingTime = 60, // 挥舞时间
                OnUse = (_) =>
                {
                    Player.GetModPlayer<WeaponResetPlayer>().OnModifyByHit += Defence;
                },
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcross2 = new(this, // 横砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseTile,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1.8f, 0.3f), // 速度缩放
                VisualRotation = 0.7f, // 视觉朝向
                ActionDmg = 1.5f, // 动作值
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
                SwingTime = attackSpeed / 2, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcross_Dash = new(this, // 逆横砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(0.4f),// 起始速度朝向
                VelScale = new Vector2(1.8f, 0.3f), // 速度缩放
                VisualRotation = 0.7f, // 视觉朝向
                ActionDmg = 2.5f, // 动作值
            },
            preAtk: new() // 攻击前
            {
                PreTime = 10, // 前摇时间
                OnUse = (_) =>
                {
                    if (Player.controlUseItem && Projectile.ai[1] >= 8)
                    {
                        Projectile.ai[1] = 8;
                    }
                },
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 3, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = 10, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect + StrongPunchCamera,
                OnChange = ResetFullRot,
                OnUse = SetFullRot + MoveSlash

            }, SwingHelper, Player);

            SwingHelper_GeneralSwing SwingAcross3 = new(this, // 横砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseTile,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(1.8f, 0.3f), // 速度缩放
                VisualRotation = 0.7f, // 视觉朝向
                ActionDmg = 3.5f, // 动作值
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
                SwingTime = attackSpeed / 2, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player);

            SwingHelper_Defence Def2 = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(), // 挥舞长度
                ChangeCondition = () => Player.controlUseItem && OreSwordItems.DefCD <= 0, // 右键防御
                SwingRot = 0, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f), // 起始速度朝向
                VelScale = new Vector2(1f, 1f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
            },
            preAtk: new() // 攻击前
            {
                PreTime = 5, // 前摇时间
                OnChange = ChangeToRot,
                OnUse = (_) =>
                {
                    OreSwordItems.DefCD = OreSwordItems.DefCDMax;
                }
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 1, // 后摇切换时间
            },
            onAtk: new() // 攻击时
            {
                SwingTime = 60, // 挥舞时间
                OnUse = (_) =>
                {
                    Player.GetModPlayer<WeaponResetPlayer>().OnModifyByHit += Defence;
                },
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
            }, SwingHelper, Player);

            SwingHelper_GeneralSwing StorngSlash2 = new(this, // 斜下砍
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseTile,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-0.4f),// 起始速度朝向
                VelScale = new Vector2(2f, 2f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
                ActionDmg = 11.5f, // 动作值
            },
            preAtk: new() // 攻击前
            {
                PreTime = 60, // 前摇时间
                OnUse = (_) =>
                {
                    if (Player.controlUseTile&& Projectile.ai[1] >= 58)
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
                SwingTime = attackSpeed / 4, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect + StrongPunchCamera,
                OnChange = ResetFullRot,
                OnUse = SetFullRot + MoveSlash
            }, SwingHelper, Player);

            SwingUp.preAtk.OnChange += (_) => DamageAdd = 0;
            SwingUp.onAtk.OnChange += NoHitChange;
            SwingAcrossDown.onAtk.OnChange += NoHitChange;
            SwingAcross.onAtk.OnChange += NoHitChange;
            SwingDown.onAtk.OnChange += NoHitChange;
            StorngSlash.onAtk.ModifyHit += LastModifyHit;

            SwingUp.CanMoveScreen = SwingAcrossDown.CanMoveScreen = SwingAcross.CanMoveScreen = SwingDown.CanMoveScreen = StorngSlash.CanMoveScreen = SwingAcross2.CanMoveScreen = SwingAcross_Dash.CanMoveScreen = SwingAcross3.CanMoveScreen = StorngSlash2.CanMoveScreen = true;
            #endregion
            #region 连接技能
            SwingAcrossDown.AddSkill(SwingAcross3).AddSkill(Def2).AddSkill(StorngSlash2);
            SwingUp.AddSkill(SwingAcross2).AddSkill(SwingAcross_Dash);
            Def.AddBySkill(noUse);
            noUse.AddSkill(SwingUp).AddSkill(SwingAcrossDown).AddSkill(SwingAcross).AddSkill(SwingDown).AddSkill(StorngSlash);
            #endregion
            #region 注册技能
            (this as IBasicSkillProj).Register(SwingUp, SwingAcrossDown, SwingAcross, SwingDown, StorngSlash, Def, noUse, SwingAcross2, SwingAcross_Dash, SwingAcross3, Def2, StorngSlash2);
            #endregion
            CurrentSkill = noUse;
        }
    }
}