using InnoVault.PRT;

namespace WeaponReset.Content.Dusts.Particles
{
    public class Extra98 : BasePRT
    {
        public delegate bool Extra98Update(Extra98 dust);
        public Extra98Update Update = (_) => true;
        public Extra98Update Draw = (_) => true;
        public Vector2 Scale_Vector2;
        public override string Texture => "Terraria/Images/Extra_98";
        //重写这个函数，它会在粒子被生成的时候调用一次，PRT实体都是独立的实例
        //因此这个函数的设置也可以独立作用到每个实例上，在作用上和ModProjectile.SetDefaults类似
        public override void SetProperty()
        {
            PRTDrawMode = PRTDrawModeEnum.AlphaBlend;
            SetLifetime = true;//将这个值设置为true，粒子将受到寿命限制
            Lifetime = 60; // 1s寿命
        }
        public override void AI()
        {
            if (Update?.Invoke(this) != true)
                return;
            //进行一个有趣的亮度变换
            Color.A = (byte)(1 - LifetimeCompletion * 255f);
            //在生命末期，进行淡化处理
            if (LifetimeCompletion > 0.8f)
            {
                Color *= 0.9f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch) => Draw?.Invoke(this) == true;
    }
}
