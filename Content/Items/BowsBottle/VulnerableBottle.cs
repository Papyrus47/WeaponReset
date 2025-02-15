using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaponReset.Command.GlobalNPCs;

namespace WeaponReset.Content.Items.BowsBottle
{
    public class VulnerableBottle : BasicBowsBottle
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.consumable = true;
            Item.height = 26;
            Item.maxStack = 200;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.GetGlobalNPC<GeneralGlobalNPC>().Vulnerable.Strength < 5) // 最多增加伤害50% 
                target.GetGlobalNPC<GeneralGlobalNPC>().Vulnerable.Strength += 1; // 设置为1强度脆弱
            target.GetGlobalNPC<GeneralGlobalNPC>().Vulnerable.Conuts += 2; // 每个加两层脆弱
        }
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID_Chinese.玻璃瓶, 50).AddIngredient(ItemID_Chinese.夜明锭).Register();
        }
    }
}
