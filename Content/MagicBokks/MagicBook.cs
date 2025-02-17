using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using WeaponReset.Content.Weapons.MagicStaff;
using WeaponReset.Content.Weapons;

namespace WeaponReset.Content.MagicBokks
{
    public class MagicBook : BasicWeaponsItems<MagicBook>
    {
        public bool noUseGraphic;
        public override bool InstancePerEntity => true;
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            ResetWeaponID ??= new();
            ResetWeaponID.TryAddArray(
            [
                ItemID_Chinese.水矢,
                ItemID_Chinese.骷髅头法书,
                ItemID_Chinese.恶魔锄刀,
                ItemID_Chinese.诅咒焰_魔法书,
                ItemID_Chinese.黄金雨,
                ItemID_Chinese.水晶风暴,
                ItemID_Chinese.磁球,
                ItemID_Chinese.利刃台风,
                ItemID_Chinese.月耀,
            ]);
        }
        public static Color GetColor(Item item)
        {
            // 我 爱 deep seek 小姐
            switch (item.type)
            {
                //============== 水系/冰系法术 ==============
                case ItemID_Chinese.水矢: return new Color(0, 150, 255);  // 流动水蓝 (带10%透明度)
                case ItemID_Chinese.水晶风暴: return new Color(180, 240, 255); // 冰晶风暴 (高亮冷白)

                //============== 暗黑魔法系列 ==============
                case ItemID_Chinese.骷髅头法书: return Color.White; // 半透明暗白 (灵体效果)
                case ItemID_Chinese.恶魔锄刀: return Color.Purple;   // 恶魔紫
                case ItemID_Chinese.诅咒焰_魔法书: return Color.LightGreen; // 绿诅咒火焰

                //============== 神圣/光系法术 ==============
                case ItemID_Chinese.黄金雨: return new Color(255, 215, 50); // 黄金雨滴 (金属光泽)
                case ItemID_Chinese.月耀: return new Color(220, 240, 255); // 月光银辉

                //============== 自然能量系列 ==============
                case ItemID_Chinese.磁球: return new Color(0, 180, 255);  // 电磁蓝 (带荧光效果)
                case ItemID_Chinese.利刃台风: return new Color(0, 255, 180); // 风刃青绿色
            }
            return Color.White;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                string text = ShiftText.Value;
                //text = text.Replace("<left>", Language.GetTextValue("Mods.WeaponReset.Left"));
                //text = text.Replace("<right>", Language.GetTextValue("Mods.WeaponReset.Right"));
                tooltips.Add(new(Mod, "OnShift_ShowText", text));
            }
            else
            {
                tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.UseResetBind.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            }
            base.ModifyTooltips(item, tooltips);
        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            noUseGraphic = entity.noUseGraphic;
        }
        public override bool CanShoot(Item item, Player player)
        {
            //if (player.altFunctionUse == 2)
            //{
            //    item.noUseGraphic = true;
            //}
            //else
            //{
            //    item.noUseGraphic = noUseGraphic;
            //}
            return base.CanShoot(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<MagicCricleProj_F>()] < 3 && player.CheckMana(100, true))
            {
                player.itemTime = player.itemAnimation = 20;
                var proj = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<MagicCricleProj_F>(), damage, knockback, player.whoAmI, Main.rgbToHsl(GetColor(item)).X, 0f, item.Size.Length() * 0.5f);
                proj.localAI[1] = 2f + player.ownedProjectileCounts[ModContent.ProjectileType<MagicCricleProj_F>()]; // 缩放
                (proj.ModProjectile as MagicCricleProj_F).Luminosity = Main.rgbToHsl(GetColor(item)).Z;
                proj.localAI[2] = Main.rgbToHsl(GetColor(item)).Y;
                proj.netUpdate = true;
                return false;
            }
            return true;
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            return true;
        }
    }
}
