using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaponReset.Command;
using WeaponReset.Content.General;
using Terraria.ID;
using InnoVault.PRT;
using WeaponReset.Content.Dusts.Particles;
using Terraria.Audio;

namespace WeaponReset.Content.Weapons.SPAtkSwords.Skills
{
    public class ChangePower : SwingHelper_GeneralSwing
    {
        public ChangePower(ModProjectile modProjectile, Setting setting, PreAtk preAtk, PostAtk postAtk, OnAtk onAtk, SwingHelper swingHelper, Player player) : base(modProjectile, setting, preAtk, postAtk, onAtk, swingHelper, player)
        {
            playSound = SoundID.Item8;
        }

        public override void AI()
        {
            base.AI();
            if (modProjectile is SPAtkSwordProj swordProj && (int)Projectile.ai[0] != 2)
            {
                if(!Player.controlUseTile)
                {
                    Projectile.ai[1] = 0; // 重置
                    Projectile.ai[0] = 2; // 停止充能
                    return;
                }
                Player.fullRotation = 0;
                var item = swordProj.SpawnItem.GetGlobalItem<SPAtkSwordItems>();
                if (Player.controlUseTile && (int)Projectile.ai[0] == 1) // 充能
                {
                    Projectile.extraUpdates = 0;
                    if (Projectile.ai[1] > 10 && Player.CheckMana(4, true))
                    {
                        Player.manaRegenDelay = 100;
                        Projectile.ai[1] = 0;
                        if(Projectile.soundDelay-- <= 0)
                        {
                            Projectile.soundDelay = 10;
                            SoundEngine.PlaySound(playSound, Projectile.position);
                        }
                        if (item.Charged < item.ChargedMax)
                        {
                            if(Player.name == "weapon reset")
                                item.Charged = item.ChargedMax;
                            else
                                item.Charged += item.ChargedMax / 60;
                        }
                        else
                        {
                            item.Charged = item.ChargedMax;
                            Projectile.ai[1] = 0; // 重置
                            Projectile.ai[0] = 2; // 停止充能
                            return;
                        }
                    }
                }
            }
        }
    }
}
