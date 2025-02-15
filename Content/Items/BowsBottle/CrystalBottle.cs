using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Items.BowsBottle
{
    public class CrystalBottle : BasicBowsBottle
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.consumable = true;
            Item.height = 26;
            Item.maxStack = 200;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) => Projectile.NewProjectile(target.GetSource_OnHit(player), target.Center, Main.rand.NextVector2Unit() * 8, 90, hit.SourceDamage, hit.Knockback, player.whoAmI);
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID_Chinese.玻璃瓶, 50).AddIngredient(ItemID_Chinese.水晶碎块).Register();
        }
    }
}
