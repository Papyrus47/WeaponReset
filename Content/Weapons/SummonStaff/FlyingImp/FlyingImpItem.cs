using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.FlyingImp
{
    public class FlyingImpItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ImpStaff;
        public override void SetDefaults(Item entity)
        {
            entity.StatsModifiedBy.Add(Mod);
        }
    }
}
