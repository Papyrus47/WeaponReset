using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using WeaponReset.Command.GlobalNPCs;

namespace WeaponReset.Content.Weapons.SummonStaff.Smolstar
{
    public class SmolstarProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Smolstar;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.FindNPC_ClosestToPlayer(player, 900f);
            if (target != null)
            {
                if (projectile.minionPos == 0)
                    return true;

                if (player.dead)
                    player.smolstar = false;

                if (player.smolstar)
                    projectile.timeLeft = 2;
                Vector2 vel = target.Center - projectile.Center;
                ai[5] = target.whoAmI;
                projectile.extraUpdates = 0;
                switch ((int)ai[0])
                {
                    case < 3: // 挥舞
                        if (vel.Length() > 20 && (int)ai[1] == 0)
                        {
                            projectile.velocity = vel.SafeNormalize(default) * 15;
                            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                            projectile.netUpdate = true;
                        }
                        else
                        {
                            projectile.extraUpdates = 2;
                            ai[1] = 1;
                            ai[2]++;
                            if (ai[2] > 20)
                            {
                                projectile.extraUpdates = 0;
                                ai[1] = ai[2] = ai[3] = ai[4] = 0;
                                ai[0]++;
                                projectile.netUpdate = true;
                            }
                            projectile.velocity = projectile.velocity.RotatedBy(0.1).SafeNormalize(default) * 10;
                            projectile.rotation = projectile.velocity.ToRotation();
                        }
                        break;
                    default:
                        if (projectile.ai[0] == -1)
                        {
                            ai[0] = ai[1] = ai[2] = ai[3] = ai[4] = 0;
                        }
                        return true;
                }
                return false;
            }
            return true;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.minionPos != 0 && target.GetGlobalNPC<GeneralGlobalNPC>().SmolstarMark > 0)
            {
                modifiers.SourceDamage += 0.15f;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.minionPos == 0)
            {
                target.GetGlobalNPC<GeneralGlobalNPC>().SmolstarMark = 120;
            }
        }
    }
}
