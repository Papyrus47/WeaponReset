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

namespace WeaponReset.Content.Weapons.SPAtkSwords.Skills
{
    public class ChangePower : SwingHelper_NoUse
    {
        public ChangePower(Player player, SwingHelper swingHelper, ModProjectile proj) : base(player, swingHelper, proj)
        {
        }
        public override void AI()
        {
            base.AI();
            if (modProjectile is SPAtkSwordProj swordProj)
            {
                var item = swordProj.SpawnItem.GetGlobalItem<SPAtkSwordItems>();
                if (player.controlUseTile && player.CheckMana(10, true) && Main.netMode != NetmodeID.MultiplayerClient) // 充能
                {
                    swordProj.SpawnItem.NetStateChanged();
                    if (item.Charged < item.ChargedMax)
                        item.Charged += 10;
                    else
                        item.Charged = item.ChargedMax;
                }
                var prt = PRTLoader.NewParticle<GlowBall>(Projectile.Center, Projectile.velocity * 0.1f, swordProj.GetColor(1), Main.rand.NextFloat(0.5f, 1.3f));
            }
        }
    }
}
