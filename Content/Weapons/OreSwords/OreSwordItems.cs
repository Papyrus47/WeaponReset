using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;

namespace WeaponReset.Content.Weapons.OreSwords
{
    public class OreSwordItems : BasicWeaponsItems
    {
        public static int DefCD;
        public static int DefCDMax = 120;
        public override HashSet<int> ResetWeaponID => new()
        {
            ItemID.CopperBroadsword,
            ItemID.IronBroadsword,
            ItemID.SilverBroadsword,
            ItemID.GoldBroadsword,
            ItemID.PlatinumBroadsword,
            ItemID.LeadBroadsword,
            ItemID.TungstenBroadsword,
            ItemID.TinBroadsword,
            ItemID.WoodenSword,
            ItemID.PalmWoodSword,
            ItemID.AshWoodSword,
            ItemID.BorealWoodSword
        };
        public override void SetDefaults(Item entity)
        {
            if (CanResetWeapon)
            {
                entity.noUseGraphic = true;
                entity.noMelee = true;
                entity.scale *= 2;
                entity.useStyle = ItemUseStyleID.Rapier;
                entity.UseSound = null;
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (DefCD > 0)
                DefCD--;
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (DefCD <= 0)
                return;
            Texture2D tex = TextureAssets.BlackTile.Value;
            spriteBatch.Draw(tex, position, frame, Color.White with { A = 200} * 0.8f, 0, origin, new Vector2(scale, scale * ((float)DefCD / DefCDMax)) * 1.3f, SpriteEffects.None, 0f);
        }
        public override void HoldItem(Item item, Player player)
        {
            if (!CanResetWeapon)
                return;
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
