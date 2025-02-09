using InnoVault.PRT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace WeaponReset.Content.Dusts.Particles
{
    public class GlowBall : BasePRT
    {
        public override void SetProperty()
        {
            Lifetime = Main.rand.Next(100, 150); // 存在时间
            SetLifetime = true;//将这个值设置为true，粒子将受到寿命限制
            PRTDrawMode = PRTDrawModeEnum.AlphaBlend;
            Frame = TexValue.Frame();
        }
        public override void AI()
        {
            Velocity = Velocity.RotatedByRandom(0.1) * 0.95f; // 随机旋转速度
            if(Velocity.Length() < 5f && Time < 100)
            {
                Time = 100; // 粒子存在时间限制
            }

            if (LifetimeCompletion > 0.8f) // 粒子存在时间的80%时，颜色变淡
            {
                Color *= 0.9f;
            }
            base.AI();
        }
    }
}
