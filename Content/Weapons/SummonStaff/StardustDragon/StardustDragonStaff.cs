using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.StardustDragon
{
    public class StardustDragonStaff : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.StardustDragonStaff;
        public override void SetDefaults(Item entity)
        {
            entity.StatsModifiedBy.Add(Mod);
        }
    }
}
