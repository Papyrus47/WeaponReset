using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.Smolstar
{
    public class SmolstarItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Smolstar;
        public override void SetDefaults(Item entity)
        {
            entity.StatsModifiedBy.Add(Mod);
        }
    }
}
