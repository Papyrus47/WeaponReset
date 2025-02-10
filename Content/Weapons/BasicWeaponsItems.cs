using Mono.Cecil;
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
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            bool addVulnerable = false;
            for (int i = 0;i<tooltips.Count;i++)
            {
                var tooltip = tooltips[i];
                if (tooltip.Text.Contains("<Vulnerable>"))
                {
                    tooltip.Text = tooltip.Text.Replace("<Vulnerable>", WeaponReset.VulnerableNameText.Value);
                    if (!addVulnerable)
                    {
                        TooltipLine item1 = new(Mod, "VulnerableTooltip", WeaponReset.VulnerableNameText.Value + ":" + WeaponReset.VulnerableTooltipText.Value);
                        item1.OverrideColor = Color.LightPink;
                        tooltips.Add(item1);
                    }
                }
            }
        }
    }
}
