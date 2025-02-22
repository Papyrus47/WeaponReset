using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.RetaniAndSpazma
{
    public class SpazmaminiProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Spazmamini;
        public override bool PreAI(Projectile projectile)
        {
            
            Player player = Main.player[projectile.owner];
            NPC target = projectile.OwnerMinionAttackTargetNPC;
            if (target != null)
            {
                if (player.Distance(projectile.Center) > 1200)
                {
                    ai[0] = 0;
                    return true;
                }
                if (player.dead)
                    player.twinsMinion = false;

                if (player.twinsMinion)
                    projectile.timeLeft = 2;

                projectile.frameCounter++;
                if (projectile.frameCounter > 3)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame > Main.projFrames[projectile.type] - 1)
                        projectile.frame = 0;
                }

                Vector2 vel = target.Center - projectile.Center;
                
                if (ai[0] > 60 && vel.Length() < 200 && player.whoAmI == Main.myPlayer && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.Center, 0, 0))
                {
                    ai[0]++;
                    if (ai[0] > 180)
                    {
                        projectile.velocity = vel.SafeNormalize(default) * 10;
                        ai[0] = 0;
                        return false;
                    }
                    int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, vel.SafeNormalize(default) * 5f, 101, (int)(projectile.damage * 1.15f), 0f, player.whoAmI);
                    Main.projectile[proj].timeLeft = 300;
                    Main.projectile[proj].hostile = false;
                    Main.projectile[proj].friendly = true;
                    Main.projectile[proj].usesIDStaticNPCImmunity = true;
                    Main.projectile[proj].idStaticNPCHitCooldown = 3;
                    projectile.netUpdate = true;
                    if (vel.Length() > 100)
                        projectile.velocity = vel.SafeNormalize(default).SafeNormalize(default).RotatedBy(0.01f * projectile.minionPos) * 1.5f;
                    else
                        projectile.velocity = (projectile.velocity * 5 + vel.SafeNormalize(default).RotatedBy(0.01f * projectile.minionPos) * -5) / 6f;
                    projectile.rotation = vel.ToRotation() + MathHelper.Pi;
                }
                else if (vel.Length() > 100)
                {
                    projectile.velocity = vel.SafeNormalize(default).RotatedBy(0.005 * projectile.minionPos) * 10;
                    if (projectile.oldVelocity != projectile.velocity)
                        ai[0]++;
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
                    projectile.ai[0] = 2;
                }
                return false;
            }
            return true;
        }
    }
}
