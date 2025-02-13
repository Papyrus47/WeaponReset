using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Items.BowsBottle
{
    public class IchorBottle : BasicBowsBottle
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.consumable = true;
            Item.height = 26;
            Item.maxStack = 200;
        }
        public override void ProjAI(Projectile projectile)
        {
            Dust.NewDust(projectile.Center + projectile.velocity.SafeNormalize(default) * projectile.width * 0.5f, 5,5, DustID.IchorTorch);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 60);
        }
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID_Chinese.玻璃瓶, 50).AddIngredient(ItemID_Chinese.灵液).Register();
        }
    }
}
