using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace WeaponReset.Content.Weapons.SummonStaff.Flinx
{
    public class FlinxProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == 951; // 小雪怪
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            NPC target = projectile.FindTargetWithinRange(500);
            if (target != null)
            {
                if (projectile.Distance(player.position) > 2000)
                {
                    projectile.position = player.position;
                    return true;
                }
                if (player.HasMinionAttackTargetNPC)
                    target = projectile.OwnerMinionAttackTargetNPC;
                if (player.dead)
                    player.flinxMinion = false;

                if (player.flinxMinion)
                    projectile.timeLeft = 2;

                projectile.tileCollide = true;
                projectile.spriteDirection = projectile.direction = projectile.velocity.X > 0 ? 1 : -1;
                ai[1] = target.Bottom.Y - projectile.Center.Y;
                if (projectile.Distance(target.Center) < 400 && ai[1] > -50)
                {
                    ai[0]++;
                    projectile.netUpdate = true;
                    if (ai[0] < 20) // 蓄力
                    {
                        projectile.frame = 0;
                        //if (projectile.velocity.Y != 0)
                        //{
                        //    ai[0] = 0;
                        //}
                        projectile.velocity.X *= 0.9f;
                        projectile.rotation += 0.4f;
                        if ((int)ai[0] % 10 == 0 && Main.netMode != NetmodeID.Server)
                        {
                            Dust.NewDustPerfect(
                                projectile.Center,
                                DustID.SnowBlock, // 雪块粒子
                                new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-3f, 0f)),
                                0,
                                new Color(200, 230, 255), // 冷蓝色调
                                1.2f
                            );

                            // 添加冰晶闪光
                            Dust.NewDustDirect(
                            projectile.position,
                            projectile.width,
                                projectile.height,
                                DustID.Frost, // 霜冻粒子
                                Scale: 0.5f
                            ).noGravity = true;
                        }
                    }
                    else if ((int)ai[0] == 20) // 进行冲刺
                    {
                        projectile.velocity.X = target.Center.X - projectile.Center.X > 0 ? 50 : -50;
                        projectile.velocity.Y = Math.Clamp(target.Center.Y - projectile.Center.Y, -4, 4); // 移动
                    }
                    else // 冲刺
                    {
                        //Vector2 frontPos = projectile.Center + new Vector2(projectile.direction * projectile.width * 0.6f, projectile.height * 0.5f);
                        //Dust.NewDustPerfect(
                        //    frontPos,
                        //    DustID.Frost, // 霜火花
                        //    Vector2.UnitY * -Main.rand.NextFloat(1f, 3f),
                        //    0,
                        //    Color.White,
                        //    1.2f
                        //);
                        if (Math.Abs(projectile.velocity.X) < 6)
                        {
                            ai[0] = 0;
                        }
                        projectile.velocity.X *= 0.99f; // 减速
                        if (projectile.velocity.Y == 0)
                            projectile.velocity.Y = -5;
                        projectile.rotation += projectile.velocity.X * 0.1f; // 冲刺翻滚
                    }
                    projectile.velocity.Y += 0.21f;
                }
                else
                {
                    ai[0] = 0; // 重置冲刺CD
                    projectile.velocity.Y += 0.1f;
                    projectile.velocity.X = Math.Clamp(target.Center.X - projectile.Center.X, -8, 8); // 移动
                    projectile.rotation = projectile.velocity.X * 0.025f;
                    projectile.netUpdate = true;
                    projectile.frameCounter++;
                    if (projectile.frameCounter > 3)
                    {
                        projectile.frame++;
                        projectile.frameCounter = 0;

                        if (projectile.frame < 2 || projectile.frame >= Main.projFrames[projectile.type])
                            projectile.frame = 2;
                    }
                    if (ai[1] < 0)
                        projectile.velocity.Y = -4;
                }
                return false;
            }
            ai[1] = 0;
            return true;
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (ai[1] > 0)
            {
                fallThrough = true; // 穿透
            }
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ai[0] > 3)
            {
                target.AddBuff(BuffID.Frostburn2, 100); // 霜焰效果
            }
            else
            {
                projectile.velocity.X += (projectile.Center.X - target.Center.X) * 0.8f; // 弹开效果
            }
        }
    }
}
