using static System.Net.Mime.MediaTypeNames;

namespace WeaponReset.Content.Weapons.SummonStaff.RetaniAndSpazma
{
    public class RetaniminiProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Retanimini;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.OwnerMinionAttackTargetNPC;
            if(target != null)
            {
                if (player.Distance(projectile.Center) > 800)
                    return true;

                if (player.dead)
                    player.twinsMinion = false;

                if (player.twinsMinion)
                    projectile.timeLeft = 2;

                projectile.frameCounter++;
                if(projectile.frameCounter > 3)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame > Main.projFrames[projectile.type] - 1)
                        projectile.frame = 0;
                }

                Vector2 vel = (target.Center - projectile.Center);
                projectile.rotation = vel.ToRotation() + MathHelper.Pi;
                if (vel.Length() > 200)
                    projectile.velocity = vel.SafeNormalize(default).RotatedBy(0.01f * projectile.minionPos) * 10;
                else if (vel.Length() < 160)
                    projectile.velocity *= 0.95f;
                else
                    projectile.velocity = projectile.velocity.RotatedBy(0.01f * projectile.minionPos);
                ai[0]++;
                if ((ai[0] > 45 || (ai[1] > 5 && ai[0] > 5)) && player.whoAmI == Main.myPlayer && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.Center, 0, 0))
                {
                    ai[0] = 0;
                    if (++ai[1] > 15)
                        ai[1] = 0;
                    int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, vel.SafeNormalize(default) * 8f, 389, (int)(projectile.damage * 1.15f), 0f, player.whoAmI);
                    Main.projectile[proj].timeLeft = 300;
                    projectile.netUpdate = true;
                }
                return false;
            }
            return true;
        }
    }
}
