using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace WeaponReset.Command.Configs
{
    public class Setting : ModConfig
    {
        public static Setting Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DefaultValue(false)]
        public bool DrawScaleWeapon;
        public override void OnLoaded()
        {
            base.OnLoaded();
            Instance = this;
        }
    }
}
