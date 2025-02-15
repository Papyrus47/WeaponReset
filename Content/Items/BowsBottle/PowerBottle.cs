using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Items.BowsBottle
{
    public class PowerBottle : BasicBowsBottle
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.consumable = true;
            Item.height = 26;
            Item.maxStack = 200;
        }
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID_Chinese.玻璃瓶, 50).AddIngredient(ItemID_Chinese.蘑菇,50).Register();
        }
        public override void ProjAI(Projectile projectile)
        {
            Dust.NewDust(projectile.Center + projectile.velocity.SafeNormalize(default) * projectile.width * 0.5f, 5, 5, DustID.Firework_Red);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 1.3f;
        }
    }
}
