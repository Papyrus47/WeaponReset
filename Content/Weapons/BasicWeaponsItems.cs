using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaponReset.Command.Configs;

namespace WeaponReset.Content.Weapons
{
    public abstract class BasicWeaponsItems<T> : GlobalItem where T : BasicWeaponsItems<T>
    {
        public static bool CanResetWeapon = true;
        public static HashSet<int> ResetWeaponID;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return lateInstantiation && ResetWeaponID.Contains(entity.type);
        }
    }
}
