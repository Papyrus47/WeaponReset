using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace WeaponReset.Content.Weapons.SummonStaff.Finch
{
    public class BabyBirdProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.BabyBird;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.FindNPC_ClosestToPlayer(player, 1000,true);
            if (target != null) // Ö´ÐÐAI
            {
                if (player.dead)
                    player.babyBird = false;

                if (player.babyBird)
                    projectile.timeLeft = 2;
                Vector2 vel = target.Center - projectile.Center;
                if (vel.Length() < 200)
                    ai[0]++;
                if (ai[0] > 60 && player.whoAmI == Main.myPlayer)
                {
                    ai[0] = 0;
                    Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, vel.SafeNormalize(default) * 10, ModContent.ProjectileType<ShitProj>(), projectile.damage, projectile.knockBack, player.whoAmI);
                }
                if (vel.Length() > 20)
                    projectile.velocity = (projectile.velocity * 105 + vel.SafeNormalize(default) * 30) / 106f;
                projectile.frameCounter++;
                projectile.spriteDirection = projectile.direction;
                projectile.rotation = projectile.velocity.X * 0.1f;
                if (projectile.frameCounter > 6)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type] - 1)
                    {
                        projectile.frame = 0;
                    }
                }
                return false;
            }
            ai[0] = 0;
            return true;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 3;
        }
    }
}