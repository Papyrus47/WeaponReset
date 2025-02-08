﻿using InnoVault.PRT;
using Terraria.Audio;
using Terraria.GameContent;
using WeaponReset.Command;
using WeaponReset.Content.Dusts.Particles;
using WeaponReset.Content.General;
using WeaponReset.Content.Weapons.OreSwords;

namespace WeaponReset.Content.Weapons.SPAtkSwords
{
    public class SPAtkSwordProj : BasicMeleeWeaponSword
    {
        public ProjSkill_Instantiation[] skillPairs;
        public override string Texture => $"Terraria/Images/Item_{ItemID.None}";

        public float DamageAdd;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public virtual Color GetColor(float factor)
        {
            switch (SpawnItem.type)
            {
                case ItemID_Chinese.断钢剑:
                    return Color.Gold with { A = 0 } * factor;
            }
            return default;
        }

        public override void Unload()
        {
            base.Unload();
        }
        public override void Init()
        {
            OldSkills = new();
            #region 创建技能
            Func<float, float> swingChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 4.2f)); // 缓动函数
            float attackSpeed = Player.GetWeaponAttackSpeed(SpawnItem) * (SpawnItem.useTime + SpawnItem.useAnimation) / 2;

            SwingHelper_NoUse noUse = new(Player, SwingHelper, this)
            {
                Length = Projectile.Size.Length()
            }; // 玩家拿在手上不使用的时候

            SwingHelper_GeneralSwing SwingUp = new(this, // 上斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = false, // 挥舞方向变化
                StartVel = Vector2.UnitY.RotatedBy(MathHelper.PiOver4),// 起始速度朝向
                VelScale = new Vector2(1, 1), // 速度缩放
                VisualRotation = 0, // 视觉朝向
                OnHitStopTime = 2, // 击中后停留时间
                ActionDmg = 1.5f, // 伤害倍率
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 10, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = (target, hit, damage) =>
                {
                    OnHitEffect(target, hit, damage);
                    if (target.knockBackResist != 0)
                        target.velocity.Y = -10f; // 击飞
                },
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player)
            {
                CanMoveScreen = true
            };

            SwingHelper_GeneralSwing SwingAcross = new(this, // 横斩,卡肉
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-MathHelper.PiOver4),// 起始速度朝向
                VelScale = new Vector2(1, 0.3f), // 速度缩放
                VisualRotation = -0.7f, // 视觉朝向
                OnHitStopTime = 2, // 击中后停留时间
                ActionDmg = 1.5f, // 伤害倍率
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 10, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player)
            {
                CanMoveScreen = true,
            };

            SwingHelper_GeneralSwing SwingDown = new(this, // 下斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-MathHelper.PiOver4),// 起始速度朝向
                VelScale = new Vector2(1, 1f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
                OnHitStopTime = 2, // 击中后停留时间
                ActionDmg = 2.3f, // 伤害倍率
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 10, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player)
            {
                CanMoveScreen = true
            };
            SwingHelper_CountSwing TwoSwing = new(this, // 下斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-MathHelper.PiOver4),// 起始速度朝向
                VelScale = new Vector2(1.5f, 0.3f), // 速度缩放
                VisualRotation = -0.7f, // 视觉朝向
                OnHitStopTime = 2, // 击中后停留时间
                ActionDmg = 1.5f, // 伤害倍率
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 10, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed / 1.5f, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player,2)
            {
                CanMoveScreen = true
            };

            SwingHelper_GeneralSwing StrongSwingDown = new(this, // 下斩
            setting: new() // 设置
            {
                SwingLenght = Projectile.Size.Length(),// 挥舞长度
                ChangeCondition = () => Player.controlUseItem,
                SwingRot = MathHelper.Pi + MathHelper.PiOver2, // 挥舞角度
                preDraw = DrawProj,
                SwingDirectionChange = true, // 挥舞方向变化
                StartVel = -Vector2.UnitY.RotatedBy(-MathHelper.PiOver4),// 起始速度朝向
                VelScale = new Vector2(1.8f, 1f), // 速度缩放
                VisualRotation = 0f, // 视觉朝向
                OnHitStopTime = 2, // 击中后停留时间
                ActionDmg = 1f, // 伤害倍率
            },
            preAtk: new() // 攻击前
            {
                PreTime = 3, // 前摇时间
                OnUse = (_) =>
                {
                    if (Player.controlUseItem)
                    {
                        Projectile.ai[1] = 1;
                    }
                },
                OnChange = ChangeToRot
            },
            postAtk: new() // 攻击后
            {
                PostMaxTime = 30, // 后摇最大时间
                PostAtkTime = 10, // 后摇切换时间
            }, onAtk: new() // 攻击时
            {
                SwingTime = attackSpeed, // 挥舞时间
                TimeChange = swingChange, // 时间变化函数
                OnHit = OnHitEffect,
                OnChange = ResetFullRot,
                OnUse = SetFullRot

            }, SwingHelper, Player)
            {
                CanMoveScreen = true
            };

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

            SwingUp.preAtk.OnChange += (_) => DamageAdd = 0;
            SwingUp.onAtk.OnChange += NoHitChange;
            SwingAcross.onAtk.OnChange += NoHitChange;
            SwingDown.onAtk.OnChange += NoHitChange;
            TwoSwing.onAtk.OnChange += NoHitChange;
            StrongSwingDown.onAtk.ModifyHit += LastModifyHit;

            TwoSwing.preAtk.OnChange += (skill) => // 手动添加一个Change
            {
                skill.setting.SwingDirectionChange = true;// 挥舞方向变化
                skill.setting.StartVel = -Vector2.UnitY.RotatedBy(-MathHelper.PiOver4); // 起始朝向
            };
            TwoSwing.onAtk.OnChange += (skill) => // 手动添加一个Change
            {
                skill.setting.SwingDirectionChange = !skill.setting.SwingDirectionChange;// 挥舞方向变化
                skill.setting.StartVel = -skill.setting.StartVel; // 起始朝向
            };

            SwingDown.onAtk.OnUse += Shoot;
            StrongSwingDown.onAtk.OnUse += Shoot;
            #endregion
            #region 技能连接
            SwingUp.AddSkill(Def);
            noUse.AddSkill(SwingUp).AddSkill(SwingAcross).AddSkill(SwingDown).AddSkill(TwoSwing).AddSkill(StrongSwingDown);
            #endregion
            #region 技能注册
            (this as IBasicSkillProj).Register(noUse, SwingUp, SwingAcross,SwingDown, TwoSwing, StrongSwingDown);
            #endregion
            CurrentSkill = noUse;
        }
        public virtual void LastModifyHit(NPC target, ref NPC.HitModifiers hitModifiers)
        {
            hitModifiers.SourceDamage += DamageAdd;
            hitModifiers.FinalDamage *= 1.5f;
        }
        public virtual void NoHitChange(SwingHelper_GeneralSwing swingHelper_GeneralSwing)
        {
            //if (Projectile.numHits > 0)
            //    return;
            DamageAdd += swingHelper_GeneralSwing.setting.ActionDmg * 0.5f + Projectile.numHits * 0.7f;
        }
        public virtual void Defence(Player player, Entity target, ref Player.HurtModifiers hurtModifiers)
        {
            if (Projectile == null || !Projectile.active || Projectile.ModProjectile is not OreSwordProj)
            {
                player.GetModPlayer<WeaponResetPlayer>().OnModifyByHit -= Defence;
                return;
            }
            if (CurrentSkill is SwingHelper_Defence skill && player.GetModPlayer<WeaponResetPlayer>().PlayerImmune <= 0) // 处于防御姿态
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
        public virtual void Shoot(SwingHelper_GeneralSwing swingHelper_GeneralSwing)
        {
            if ((int)Projectile.ai[1] == (int)(swingHelper_GeneralSwing.onAtk.SwingTime / 4 * 3.9f) * (Projectile.extraUpdates + 1) && Main.myPlayer == Player.whoAmI)
            {
                TheUtility.Player_ItemCheck_Shoot(Player, SpawnItem, Projectile.damage);
            }
        }
        public virtual void ChangeToRot(SwingHelper_GeneralSwing skill)
        {
            if (Player.whoAmI != Main.myPlayer) // 其他玩家不处理这个AI
                return;
            Player.ChangeDir((Main.MouseWorld.X - Player.Center.X > 0).ToDirectionInt());
            SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation()); // 朝向
        }

        public virtual void OnHitEffect(NPC target, NPC.HitInfo hit, int damage)
        {
            if (Player.whoAmI == Main.myPlayer)
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
            if (Player.velocity.Y != 0)
                Player.velocity.Y = -10;
        }

        public virtual void ResetFullRot(SwingHelper_GeneralSwing skill)
        {
            Player.fullRotation = 0;
            Player.legRotation = 0;
        }

        public virtual void SetFullRot(SwingHelper_GeneralSwing skill)
        {
            Player.fullRotation = MathHelper.Lerp(Player.fullRotation, Projectile.velocity.X / Projectile.Size.Length() * Player.direction * skill.setting.SwingDirectionChange.ToDirectionInt() * 0.15f, 0.1f);
            Player.fullRotationOrigin = new Vector2(Player.width * 0.5f, Player.height);
        }

        public virtual bool DrawProj(SpriteBatch spriteBatch, Color drawColor)
        {
            SwingHelper.Swing_Draw_ItemAndTrailling(drawColor, TextureAssets.Extra[201].Value, GetColor);
            return false;
        }

    }
}
