using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Items.BowsBottle
{
    public class MeleeBottle : BasicBowsBottle
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.consumable = false; // 不消耗
            Item.height = 26;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID_Chinese.玻璃瓶, 1).Register();
        }
        public override void ProjAI(Projectile projectile)
        {
            if(projectile.timeLeft > 2)
                projectile.timeLeft--;
            if(projectile.extraUpdates <= 0)
                projectile.extraUpdates = 1;
            projectile.velocity.Y += 0.05f;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 1.15f;
        }
    }
}
