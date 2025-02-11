using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using WeaponReset.Command.Configs;

namespace WeaponReset.Content.Weapons.Guns
{
    public class GunsItem : BasicWeaponsItems<GunsItem>
    {
        /// <summary>
        /// 枪口过热
        /// </summary>
        public int GunsHot;
        /// <summary>
        /// 枪口过热持续时间
        /// </summary>
        public int GunsHotTime;
        /// <summary>
        /// 枪口过热重置时间
        /// </summary>
        public int GunsHotReset;
        /// <summary>
        /// 枪口过热最大值
        /// </summary>
        public int GunsHotMax = 15;
        /// <summary>
        /// 枪口过热冷却时间
        /// </summary>
        public int GunsHotCD;
        /// <summary>
        /// 寻找弹药索引
        /// </summary>
        public static int FindGunsBulletIndex;
        /// <summary>
        /// 子弹强化
        /// </summary>
        public int ManaStrong;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return lateInstantiation && ResetWeaponID?.Contains(entity.type) == true;
        }
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            // On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;\
            ResetWeaponID ??= new HashSet<int>();
            ResetWeaponID.TryAddArray([
                ItemID_Chinese.红莱德枪,
                ItemID_Chinese.燧发枪,
                ItemID_Chinese.火枪,
                ItemID_Chinese.夺命枪,
                ItemID_Chinese.霰弹枪,
                ItemID_Chinese.左轮手枪,
                ItemID_Chinese.迷你鲨,
                ItemID_Chinese.三发猎枪,
                ItemID_Chinese.四管霰弹枪,
                ItemID_Chinese.手枪,
                ItemID_Chinese.凤凰爆破枪,
                ItemID_Chinese.气喇叭,
                ItemID_Chinese.发条式突击步枪,
                ItemID_Chinese.鳄鱼机关枪,
                ItemID_Chinese.玛瑙爆破枪,
                ItemID_Chinese.乌兹冲锋枪,
                ItemID_Chinese.巨兽鲨,
                ItemID_Chinese.维纳斯万能枪,
                ItemID_Chinese.战术霰弹枪,
                ItemID_Chinese.狙击步枪,
                ItemID_Chinese.玉米糖步枪,
                ItemID_Chinese.链式机枪,
                ItemID_Chinese.外星霰弹枪,
                ItemID_Chinese.太空海豚机枪
                ]);

            On_Player.ChooseAmmo += On_Player_ChooseAmmo;
        }
        public static Item On_Player_ChooseAmmo(On_Player.orig_ChooseAmmo orig, Player self, Item weapon)
        {
            if (weapon.TryGetGlobalItem<GunsItem>(out _)) 
            {
                for (int i = FindGunsBulletIndex; i < 58; i++)
                {
                    if (self.inventory[i].stack > 0 && ItemLoader.CanChooseAmmo(weapon, self.inventory[i], self))
                    {
                        return self.inventory[i];
                    }
                }
            }
            return orig.Invoke(self, weapon);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                string text = ShiftText.Value;
                tooltips.Add(new(Mod, "OnShift_ShowText", text));
            }
            else
            {
                tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.UseResetBind.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            }
            base.ModifyTooltips(item, tooltips);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if(GunsHotCD > 0)
            {
                return false;
            }
            return base.CanUseItem(item, player);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (GunsHot < GunsHotMax)
                GunsHot++;
            else if (GunsHotTime == 0)
                GunsHotTime = 360;
            velocity = velocity.RotatedByRandom(0.2 * GunsHot / GunsHotMax);
            if (GunsHot > GunsHotMax * 0.8f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDustPerfect(player.MountedCenter + velocity.SafeNormalize(default) * 0.8f * item.width, DustID.FireworksRGB, Main.rand.NextVector2Unit() * 3, 0, Color.OrangeRed, 1.4f);
                    Dust.NewDustPerfect(player.MountedCenter + velocity.SafeNormalize(default) * 0.8f * item.width, DustID.Smoke, null, 100);
                }
            }
            if(ManaStrong > 0)
            {
                ManaStrong--;
                if(type == ProjectileID.Bullet || type == ProjectileID.SilverBullet)
                    type = ProjectileID.BulletHighVelocity;
                else
                    velocity *= 3f;
                damage += 1;
                damage = (int)(damage * 1.1f);
                knockback *= 1.5f;
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            if (WeaponReset.SpBind1.JustPressed)
            {
                SoundEngine.PlaySound(SoundID.Item149, player.position);
                for (int i = 0; player.CheckMana(item.useTime, true) && i < 100; i++)
                {
                    ManaStrong++;
                }
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (GunsHotTime <= 0 && GunsHotReset++ > player.GetWeaponAttackSpeed(item) * item.useTime * 2f)
            {
                GunsHotReset = 0;
                if(GunsHot > 0)
                    GunsHot--;
            }
            if (GunsHotCD > 0)
            {
                Dust.NewDustPerfect((player.compositeFrontArm.rotation + MathHelper.PiOver2).ToRotationVector2() * 0.8f * item.width + player.MountedCenter, DustID.Smoke, null, 100);
                GunsHotCD--;
            }
            if (GunsHotTime > 0)
            {
                if (player.ItemTimeIsZero)
                {
                    GunsHotTime--;
                    if(GunsHotTime <= 0)
                    {
                        GunsHot = 0;
                    }
                }
                else
                {
                    GunsHotTime++;
                    if (GunsHotTime > 360 * 1.5f)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            Dust.NewDustPerfect((player.compositeFrontArm.rotation + MathHelper.PiOver2).ToRotationVector2() * 0.8f * item.width + player.MountedCenter, DustID.FireworksRGB, Main.rand.NextVector2Unit() * 8, 0, Color.OrangeRed, 1.4f);
                        }
                        SoundEngine.PlaySound(SoundID.Item14, player.position);
                        GunsHotTime = 300;
                        GunsHotCD = 300;
                    }
                }
                if (GunsHotTime <= 0)
                {
                    GunsHotReset = 0;
                    GunsHot = 0;
                }
                player.GetKnockback(item.DamageType) *= 1.5f;
                player.GetCritChance(item.DamageType) *= 1.1f;
            }
            player.GetAttackSpeed(item.DamageType) += Math.Min(1f,GunsHot / (float)GunsHotMax);
        }
        public override void SetDefaults(Item entity)
        {
            entity.holdStyle = ItemHoldStyleID.HoldFront;
            GunsHotMax = (int)(100f / (entity.useTime + 1f) * 10);
        }
        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            float timeMax = (float)player.itemAnimation / player.itemAnimationMax;
            float factor = timeMax;
            factor = MathF.Pow(Math.Abs(factor - 0.5f), 1.2f);
            if(timeMax >= 0.55f)
                player.itemRotation -= factor * player.direction * MathHelper.PiOver4 * 0.25f; // 修改玩家旋转角度
            else
                player.itemRotation += factor * player.direction * MathHelper.PiOver4 * 0.25f; // 修改玩家旋转角度
        }
        public override void HoldStyle(Item item,Player player, Rectangle heldItemFrame)
        {
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.velocity.X / 10f);
            player.itemRotation = player.compositeFrontArm.rotation + MathHelper.PiOver2 * player.direction;
            player.itemLocation += new Vector2(player.width * -player.direction,player.height * -0.5f);
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player localPlayer = Main.LocalPlayer;
            if (localPlayer.HeldItem == item) // 玩家手持物品
            {
                #region 加载UI贴图
                if (!ModContent.Request<Texture2D>("WeaponReset/Content/Weapons/Guns/GunsUI").IsLoaded)
                    _ = ModContent.Request<Texture2D>("WeaponReset/Content/Weapons/Guns/GunsUI");
                #endregion
                #region 绘制
                Texture2D tex = ModContent.Request<Texture2D>("WeaponReset/Content/Weapons/Guns/GunsUI").Value;
                Rectangle rect = tex.Frame(horizontalFrames: 1, verticalFrames: 2, 0, 0, 0, 0);
                Rectangle rect_line = tex.Frame(horizontalFrames: 1, verticalFrames: 2, 0, 1, 0, 0);
                rect_line.X = 6;
                rect_line.Width = (int)((36f * GunsHot) / GunsHotMax);
                spriteBatch.Draw(tex, Main.LocalPlayer.Top + new Vector2(6, -20) - Main.screenPosition, rect_line, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(tex, Main.LocalPlayer.Top + new Vector2(0, -20) - Main.screenPosition, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);
                #endregion
                #region 绘制子弹类型
                Item drawItem = localPlayer.ChooseAmmo(item); // 选择弹药
                if (drawItem != null && !drawItem.IsAir) // 存在才开始下面的绘制内容
                {
                    tex = TextureAssets.Item[drawItem.type].Value;
                    if (Main.itemAnimations[drawItem.type] == null)
                        rect = tex.Frame(horizontalFrames: 1, verticalFrames: 1, 0, 0, 0, 0);
                    else
                        rect = tex.Frame(1, Main.itemAnimations[drawItem.type].FrameCount, 0, Main.itemAnimations[drawItem.type].Frame, 0, 0);
                    spriteBatch.Draw(tex, Main.LocalPlayer.Top + new Vector2(0, -50) - Main.screenPosition, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);
                }
                #endregion
            }
        }
    }
}