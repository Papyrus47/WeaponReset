using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace WeaponReset
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class WeaponReset : Mod
	{
		public static ModKeybind UseResetBind;
        public override void Load()
        {
            if(!Main.dedServ)
            {
                UseResetBind = KeybindLoader.RegisterKeybind(this, nameof(UseResetBind), Microsoft.Xna.Framework.Input.Keys.F);
            }
        }
        public override void Unload()
        {
            UseResetBind = null;
        }
    }
}
