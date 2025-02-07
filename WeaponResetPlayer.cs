using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using WeaponReset.Content.Weapons;

namespace WeaponReset
{
    public class WeaponResetPlayer : ModPlayer
    {
        /// <summary>
        /// 玩家下落速度
        /// </summary>
        public float playerFallSpeed;
        /// <summary>
        /// 玩家无敌时间
        /// </summary>
        public int PlayerImmune;
        /// <summary>
        /// 玩家无敌时间被命中
        /// </summary>
        public bool PlayerImmuneHit;
        public override void ResetEffects()
        {
            PlayerImmuneHit = false;
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if(PlayerImmune > 0)
            {
                PlayerImmuneHit = true;
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (WeaponReset.UseResetBind.JustPressed) // 按下
            {
                BasicWeaponsItems.CanResetWeapon = !BasicWeaponsItems.CanResetWeapon;

                foreach(Item item in Player.inventory)
                {
                    int prefix = item.prefix;
                    int count = item.stack;
                    int rare = item.rare;
                    item.SetDefaults(item.type);
                    item.prefix = prefix;
                    item.stack = count;
                    item.rare = rare;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (playerFallSpeed > 0)
            {
                Player.maxFallSpeed = playerFallSpeed;
                playerFallSpeed = -1;
            }
        }
    }
}
