using InnoVault.PRT;

namespace WeaponReset.Content.Dusts.Particles
{
    public class ElectricParticle : BasePRT
    {
        public float ScaleAll = 1f;
        public override void SetProperty()
        {
            Lifetime = Main.rand.Next(1,4); // 存在时间
            SetLifetime = true;//将这个值设置为true，粒子将受到寿命限制
            PRTDrawMode = PRTDrawModeEnum.AlphaBlend;
            Frame = TexValue.Frame();
            Origin = Frame.Size() / 2;
        }
        public override void AI()
        {
            Velocity = Velocity.RotatedByRandom(0.3).SafeNormalize(default); // 随机旋转速度
            Rotation = Velocity.ToRotation();
            base.AI();
        }
        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TexValue, Position - Main.screenPosition, Frame, Color, Rotation, Origin, new Vector2(Scale, 1) * ScaleAll, SpriteEffects.None, 0);
            return false;
        }
    }
}
