using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace WeaponReset.Content.Weapons.SummonStaff.BabySlime
{
    public class BabySlimeStaff : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.SlimeStaff;
        public override void SetDefaults(Item entity)
        {
            entity.StatsModifiedBy.Add(Mod);
        }
    }
}
