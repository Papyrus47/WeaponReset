using WeaponReset.Command;

namespace WeaponReset.Content.General
{
    public class SwingHelper_NoUse : ProjSkill_Instantiation
    {
        public Player Player;
        public SwingHelper SwingHelper;
        public float Length = 30;
        public SwingHelper_NoUse(Player player, SwingHelper swingHelper, ModProjectile proj) : base(proj)
        {
            this.Player = player;
            this.SwingHelper = swingHelper;
        }
        public override void AI()
        {
            //Projectile.localAI[0] += player.velocity.X / 10;
            //Projectile.localAI[0] %= 6.28f;
            //float rotation = Projectile.localAI[0] % (MathHelper.PiOver4 * 2) - MathHelper.PiOver4;
            SkillTimeOut = false;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Player.velocity.X / 10f); // 设置玩家的前臂为合成状态，并设置其伸展程度为Full
            Player.itemRotation = Player.compositeFrontArm.rotation; // 设置玩家的手臂角度为玩家的前臂的旋转角度

            if (Player.velocity.X != Player.oldVelocity.X && Main.myPlayer == Projectile.owner)
                Projectile.netUpdate = true;

            Player.heldProj = Projectile.whoAmI;
            SwingHelper.Change(Vector2.UnitY, Vector2.One, 0); // 起始位置,缩放
            SwingHelper.ProjFixedPos(Player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Player.velocity.X / 10f), -Length * 0.02f);
            SwingHelper.SetSwingActive(); // 激活挥舞
            Projectile.spriteDirection = Player.direction; // 弹幕贴图朝向与玩家一致
            Player.fullRotationOrigin = Player.Size * 0.5f;
            Player.fullRotation = Player.velocity.X * 0.05f;
            //player.legRotation = -player.fullRotation;
            SwingHelper.SwingAI(Length, Player.direction, Player.velocity.X / 10f * Player.direction - MathHelper.PiOver2);
        }
        public override void OnSkillActive()
        {
            base.OnSkillActive();
            SkillTimeOut = false;
        }
        public override bool ActivationCondition() => true; // 无条件激活
        public override bool SwitchCondition() => true; // 无条件切换
        public override bool? CanDamage() => false; // 无法造成伤害
        public override bool PreDraw(SpriteBatch sb, ref Color lightColor)
        {
            SwingHelper.Swing_Draw_ItemAndTrailling(lightColor, null, null); // 用这个只绘制弹幕本体
            // 禁用绘制
            return false;
        }
    }
}
