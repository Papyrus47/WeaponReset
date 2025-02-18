using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;

namespace WeaponReset.Content.Weapons.SummonStaff.Finch
{
    public class ShitProj : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.None}";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1; // 自定义AI
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            // 抛物线运动模拟
            Projectile.velocity.Y += 0.2f; // 重力
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            var dustColor = Main.rand.Next(150, 200);
            // 拖尾特效
            if (Main.rand.NextBool(3) && Main.netMode != NetmodeID.Server)
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Poop, // 使用恶臭气体特效
                    Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1f, 1f),
                    0,
                    new Color(dustColor, dustColor, dustColor), 1.5f
                ).noGravity = true;
            }

            if (Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = 114514;
                SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 1.2f }, Projectile.position);
            }
            // 飞行时随机偏移
            if (Projectile.timeLeft % 15 == 0 && Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
                Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5));
            }
        }
        public override void OnKill(int timeLeft)
        {
            // 生成爆炸特效
            for (int i = 0; i < 15; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                Dust.NewDustPerfect(Projectile.Center, DustID.Poop, speed * 2, 0, default, 1.5f);
            }
        }
    }
}
