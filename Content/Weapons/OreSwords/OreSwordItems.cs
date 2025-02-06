using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace WeaponReset.Content.Weapons.OreSwords
{
    public class OreSwordItems : BasicWeaponsItems
    {
        public override HashSet<int> ResetWeaponID => new()
        {
            ItemID.CopperBroadsword,
            ItemID.IronBroadsword,
            ItemID.SilverBroadsword,
            ItemID.GoldBroadsword,
            ItemID.PlatinumBroadsword,
            ItemID.LeadBroadsword,
            ItemID.TungstenBroadsword,
            ItemID.TinBroadsword
        };
        public override void SetDefaults(Item entity)
        {
            entity.noUseGraphic = true;
            entity.noMelee = true;
            entity.scale *= 2;
            entity.useStyle = ItemUseStyleID.Rapier;
        }
        public override void HoldItem(Item item, Player player)
        {
            // 这里是手持时候的效果
            // 我们需要在这里生成弹幕
            if (player.ownedProjectileCounts[ModContent.ProjectileType<OreSwordProj>()] <= 0) // 生成手持弹幕
            {
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.position, Vector2.Zero, ModContent.ProjectileType<OreSwordProj>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
                Main.projectile[proj].originalDamage = Main.projectile[proj].damage;
            }

        }

    }
}
