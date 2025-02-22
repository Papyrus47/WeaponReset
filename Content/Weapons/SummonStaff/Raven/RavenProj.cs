using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.Raven
{
    public class RavenProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Raven;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            float length = 800f;
            if (player.HasMinionAttackTargetNPC)
                length = 1200f;
            NPC target = projectile.FindNPC_ClosestToPlayer(player, length);

            if (player.dead)
                player.raven = false;
            if (player.raven)
                projectile.timeLeft = 2;

            if (target != null) // 拥有目标
            {
                if(projectile.ai[0] < -1)
                {
                    projectile.ai[0]++;
                }
                else if ((int)projectile.ai[0] == -1)
                {
                    Vector2 vel = target.Center - projectile.Center;
                    projectile.velocity = (projectile.velocity * 10 + vel * 0.2f) / 11f;
                    if (projectile.Distance(target.Center) < 600)
                    {
                        projectile.ai[0] = 0;
                        projectile.netUpdate = true;
                    }
                }
                else if ((int)projectile.ai[0] == 0)
                {
                    if (projectile.Distance(target.Center) > 600)
                    {
                        projectile.ai[0] = -projectile.minionPos - 1;
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        if (++ai[0] > 3)
                            ai[0] = 0;
                        projectile.ai[0] = ai[0] == 2 ? 30 : 20;
                        Vector2 vel = target.Center - projectile.Center;
                        projectile.velocity = vel.SafeNormalize(default) * (ai[0] == 2 ? 10 : 15);
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    projectile.ai[0]--;
                    projectile.friendly = true;
                    projectile.netUpdate = true;
                }
                projectile.spriteDirection = -projectile.direction;
                projectile.rotation = projectile.velocity.X * 0.03f;

                if (projectile.frameCounter++ > 4)
                {
                    projectile.netUpdate = true;
                    projectile.frameCounter = 0;
                    if (projectile.frame < 4)
                        projectile.frame = 4;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                        projectile.frame = 0;
                }
            }
            else 
            {
                projectile.friendly = false;
                ai[0] = 0;
                ai[1] = 0;
                projectile.ai[0] = -projectile.minionPos - 1;
                Vector2 pos = player.position + new Vector2((100 + MathF.Sin(projectile.minionPos * 0.25f) * 30) * -player.direction,-10 - MathF.Sin(projectile.minionPos * 1.5f) * 33.5f);
                Vector2 vel = pos - projectile.Center;
                if (vel.Length() > 50f)
                    projectile.velocity = (projectile.velocity * 10 + vel.SafeNormalize(default) * 10) / 11f;
                else if(projectile.velocity.Length() > 0.2f)
                    projectile.velocity *= 0.5f;
                projectile.spriteDirection = -projectile.direction;
                projectile.rotation = projectile.velocity.X * 0.03f;

                if (projectile.frameCounter++ > 4)
                {
                    projectile.netUpdate = true;
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= 4)
                        projectile.frame = 0;
                }
            }
            return false;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage += (int)ai[0] == 2 ? 0.5f : 0f;
            if (ai[1] < 12)
                ai[1] += 2;
            projectile.netUpdate     = true;
            modifiers.ArmorPenetration += ai[1];
        }
    }
}
