using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaponReset.Command.Configs;

namespace WeaponReset.Content.Weapons
{
    public abstract class BasicWeaponsItems : GlobalItem
    {
        public static bool CanResetWeapon = true;
        public abstract HashSet<int> ResetWeaponID { get; }
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (Setting.Instance.SettingNoResetWeapon.Count > 0)
            {
                foreach (var item in Setting.Instance.SettingNoResetWeapon)
                {
                    if (entity.type == item.type)
                        return false;
                }
            }
            return lateInstantiation && ResetWeaponID.Contains(entity.type);
        }
    }
}
