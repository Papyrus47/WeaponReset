using System;
using System.Collections.Generic;
using System.IO;
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
        public static LocalizedText VulnerableNameText;
        public static LocalizedText VulnerableTooltipText;
        public enum NetUpdateWho : byte
        {
            Player = 0
        }

        public enum NetUpdatePlayer : byte
        {
            /// <summary>
            /// 同步效果
            /// </summary>
            ResetEffect = 0,
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            //// 读取ID
            NetUpdateWho msgType = (NetUpdateWho)reader.ReadByte();
            if (msgType == NetUpdateWho.Player)
            {
                NetUpdatePlayer playerMsg = (NetUpdatePlayer)reader.ReadByte();
                if (playerMsg == NetUpdatePlayer.ResetEffect) // 玩家重置效果
                {
                    int PlayerWhoAmI = reader.ReadInt32();
                    Player player = Main.player[PlayerWhoAmI]; // 获取玩家
                    WeaponResetPlayer weaponResetPlayer = player.GetModPlayer<WeaponResetPlayer>(); // 获取玩家的WeaponResetPlayer
                    weaponResetPlayer.NetResetEffect(reader);

                    if (Main.netMode == NetmodeID.Server) // 服务器发送同步数据
                    {
                        weaponResetPlayer.SyncPlayer(-1, whoAmI, false);
                    }
                }
            }
        }
        public override void Load()
        {
            if(!Main.dedServ)
            {
                VulnerableNameText = Language.GetOrRegister("Mods." + GetType().Namespace + ".NPCBuffs." + nameof(VulnerableNameText)); // 注册语言文本
                VulnerableTooltipText = Language.GetOrRegister("Mods." + GetType().Namespace + ".NPCBuffs." + nameof(VulnerableTooltipText)); // 注册语言文本
                Main.mapEnabled = true; // 开启地图
                UseResetBind = KeybindLoader.RegisterKeybind(this, nameof(UseResetBind), Microsoft.Xna.Framework.Input.Keys.F);
            }
        }
        public override void Unload()
        {
            UseResetBind = null;
        }
    }
}
