using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.Hornet
{
    public class HornetProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Hornet;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.FindNPC_ClosestToPlayer(player,1500f,true);
            if(target != null)
            {
                if (player.dead)
                    player.hornetMinion = false;

                if (player.hornetMinion)
                    projectile.timeLeft = 2;
                float shootSpeed = 20f;
                if (player.strongBees)
                    shootSpeed = 15f;
                if (projectile.frameCounter++ > 1)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;

                    if (projectile.frame >= Main.projFrames[projectile.type])
                        projectile.frame = 0;
                }
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 pos = player.Center + (target.Center - player.Center).SafeNormalize(default).RotatedBy(ai[2]) * ai[1];
                    Vector2 vel = (pos - projectile.Center);
                    projectile.velocity = vel.SafeNormalize(default) * (vel.Length() > 10 ? 10 : vel.Length());
                    projectile.spriteDirection = projectile.direction = (target.Center - projectile.Center).X < 0 ? 1 : -1;
                    if (ai[0]++ > shootSpeed) // 发射弹幕
                    {
                        ai[0] = 0;
                        ai[1] = Main.rand.NextFloat(-120, 120);
                        ai[2] = Main.rand.NextFloat(6.28f);
                        Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center,
                            (target.Center - projectile.Center).SafeNormalize(default) * 8, 374, projectile.damage, projectile.knockBack, projectile.owner, 0, target.whoAmI);
                        if (Main.rand.NextBool(35, 100))
                        {
                            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center,
                            (target.Center - projectile.Center).SafeNormalize(default) * 10, 374, projectile.damage, projectile.knockBack, projectile.owner, 0, target.whoAmI);

                            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center,
                            (target.Center - projectile.Center).SafeNormalize(default) * 15, 374, projectile.damage, projectile.knockBack, projectile.owner, 0, target.whoAmI);
                        }
                    }
                    projectile.netUpdate = true;
                }
                return false;
            }
            return true;
        }
    }
}
