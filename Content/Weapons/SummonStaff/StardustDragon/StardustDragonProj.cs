using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.StardustDragon
{
    public class StardustDragonProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.StardustDragon1;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.FindNPC_ClosestToPlayer(player, 1000f);
            if (target != null)
            {
                if ((int)ai[0] < 20)
                {
                    ai[0]++;
                    projectile.netUpdate = true;
                    return true;
                }
                if (player.dead)
                    player.stardustDragon = false;

                if (player.stardustDragon)
                    projectile.timeLeft = 2;
                Vector2 center = player.Center;
                if (projectile.Distance(center) > 2000f)
                {
                    projectile.Center = center;
                    projectile.netUpdate = true;
                }
                Vector2 vel = target.Center - projectile.Center;
                projectile.velocity += vel * (projectile.OwnerMinionAttackTargetNPC == null ? 0.01f : 0.05f);
                if (projectile.velocity.Length() > 30)
                    projectile.velocity *= 0.95f;
                if (vel.Length() < 300)
                    projectile.velocity *= 0.95f;
                if (projectile.velocity.Length() < 5)
                    projectile.velocity *= 1.5f;

                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

                int dirChangeToNetUpdate = projectile.direction;
                projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
                if (dirChangeToNetUpdate != projectile.direction)
                    projectile.netUpdate = true;
                return false;
            }
            return true;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 1.2f;
        }
    }
}
