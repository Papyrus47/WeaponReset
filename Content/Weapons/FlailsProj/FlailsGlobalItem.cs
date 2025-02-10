using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.FlailsProj
{
    public class FlailsGlobalItem : BasicWeaponsItems<FlailsGlobalItem>
    {
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            ResetWeaponID ??= new();
            ResetWeaponID.TryAddArray([
                ItemID_Chinese.链锤,
                ItemID_Chinese.烈焰链锤,
                ItemID_Chinese.链球,
                ItemID_Chinese.血肉之球,
                ItemID_Chinese.阳炎之怒,
                ItemID_Chinese.蓝月,
                ItemID_Chinese.滴滴怪跛子,
                ItemID_Chinese.太极连枷,
                ItemID_Chinese.花冠,
                ItemID_Chinese.花冠_饥荒联动
                ]);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            //if (Main.keyState.PressingShift())
            //{
            //    string text = ShiftText.Value;

            //    tooltips.Add(new(Mod, "OnShift_ShowText", text));
            //}
            //else
            //{
            //    tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.UseResetBind.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            //}
            base.ModifyTooltips(item, tooltips);
        }
    }
}
