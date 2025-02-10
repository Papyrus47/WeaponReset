using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using WeaponReset.Content.Weapons.SPAtkSwords;

namespace WeaponReset.Content.Weapons.ShortSwords
{
    public class ShortSwordsItem : BasicWeaponsItems<ShortSwordsItem>
    {
        public static LocalizedText NoShiftText;
        public static LocalizedText ShiftText;
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            ResetWeaponID ??=
                [
                    ItemID_Chinese.铜短剑,
                    ItemID_Chinese.银短剑,
                    ItemID_Chinese.金短剑,
                    ItemID_Chinese.铂金短剑,
                    ItemID_Chinese.铅短剑,
                    ItemID_Chinese.锡短剑,
                    ItemID_Chinese.罗马短剑,
                    ItemID_Chinese.铁短剑,
                    ItemID_Chinese.钨短剑,
                ];
        }
        public override void SetDefaults(Item entity)
        {
            if (CanResetWeapon)
            {
                entity.noUseGraphic = true;
                entity.noMelee = true;
                entity.useStyle = ItemUseStyleID.Rapier;
                entity.useTurn = false;
                entity.UseSound = null;
                entity.StatsModifiedBy.Add(Mod);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                string text = ShiftText.Value;
                text = text.Replace("<left>", Language.GetTextValue("Mods.WeaponReset.Left"));
                text = text.Replace("<right>", Language.GetTextValue("Mods.WeaponReset.Right"));
                tooltips.Add(new(Mod, "OnShift_ShowText", text));
            }
            else
            {
                tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.UseResetBind.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            }
            base.ModifyTooltips(item, tooltips);
        }
        public override void HoldItem(Item item, Player player)
        {
            if (!CanResetWeapon && item?.ModItem?.Mod is not WeaponReset)
                return;
            // 这里是手持时候的效果
            // 我们需要在这里生成弹幕
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ShortSwordsProj>()] <= 0) // 生成手持弹幕
            {
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.position, Vector2.Zero, ModContent.ProjectileType<ShortSwordsProj>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
                Main.projectile[proj].originalDamage = Main.projectile[proj].damage;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(item?.ModItem?.Mod is WeaponReset || !CanResetWeapon)
            {
                return true;
            }
            return false;
        }
    }
}
