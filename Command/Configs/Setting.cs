using Terraria.ModLoader.Config;

namespace WeaponReset.Command.Configs
{
    public class Setting : ModConfig
    {
        public static Setting Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public List<Item> SettingNoResetWeapon = new();
        public override void OnLoaded()
        {
            base.OnLoaded();
            Instance = this;
        }
    }
}
