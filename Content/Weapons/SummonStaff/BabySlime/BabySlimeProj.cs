using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.BabySlime
{
    public class BabySlimeProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.BabySlime;
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.FindNPC_ClosestToPlayer(player, 1000, true);
            if (target != null)
            {
                projectile.tileCollide = true;
                projectile.friendly = true;
                if (player.dead)
                    player.slime = false;

                if (player.slime)
                    projectile.timeLeft = 2;

                if(projectile.velocity.Y == 0f) // 静止在地面的时候
                {
                    if ((int)ai[1] == 1 && Main.myPlayer == player.whoAmI) // 生成弹幕
                    {
                        ai[1] = 0;
                        for (int i = -2; i <= 2; i++)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, -Vector2.UnitY.RotatedBy(i * 0.1f) * 6f, 605, projectile.damage, projectile.knockBack, player.whoAmI); // 史莱姆尖刺弹幕
                            proj.friendly = true;
                            proj.hostile = false;
                            proj.usesIDStaticNPCImmunity = true;
                            proj.idStaticNPCHitCooldown = 5;
                        }
                    }
                    if (ai[0] >= 3) // 命中计数器大于等于3
                    {
                        projectile.velocity.Y = (target.Center.Y - projectile.Center.Y) * 0.5f;
                        ai[1] = 1; // 启用高跳伤害增加
                        ai[0] = 0; // 重置命中计数器
                    }
                    else
                        projectile.velocity.Y = (target.Center.Y - projectile.Center.Y) * (1 + projectile.extraUpdates) * 0.1f;
                    projectile.velocity.Y = Math.Min(-5, projectile.velocity.Y);
                    projectile.frame = 0;
                    projectile.frameCounter = 0;

                    projectile.velocity.X = Math.Clamp((target.Center.X - projectile.Center.X) * 0.1f,-8f,8f); // 跳跃前进
                    projectile.spriteDirection = projectile.direction = projectile.velocity.X >= 0 ? -1 : 1;

                    projectile.netUpdate = true;
                }
                else
                {
                    projectile.frameCounter++;
                    if(projectile.frameCounter > 6)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame = 1;
                    }
                    if ((int)ai[1] == 1)
                    {
                        projectile.velocity.X = Math.Clamp((target.Center.X - projectile.Center.X) * 0.1f, -8f, 8f); // 跳跃前进
                    }
                }
                if (projectile.velocity.Y < 16f)
                {
                    projectile.velocity.Y += 0.7f; // 重力加速度
                    projectile.netUpdate = true;
                }
                if (projectile.velocity.Y < -10)
                    projectile.velocity.Y *= 0.9f;
                return false;
            }
            projectile.extraUpdates = 0;
            return true;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if ((int)ai[1] == 1)
            {
                modifiers.SourceDamage += 2; // 高跳伤害增加
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(projectile, target, hit, damageDone);
            ai[0]++; // 命中计数器
            projectile.netUpdate = true;
        }
    }
}
