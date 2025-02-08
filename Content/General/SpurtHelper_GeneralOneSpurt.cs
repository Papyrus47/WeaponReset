using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using WeaponReset.Command;

namespace WeaponReset.Content.General
{
    /// <summary>
    /// 单次突刺
    /// </summary>
    public class SpurtHelper_GeneralOneSpurt : ProjSkill_Instantiation
    {
        public SpurtHelper SpurtHelper;
        public Player Player;
        public Setting setting;
        public SpurtHelper_GeneralOneSpurt(ModProjectile modProj,Setting setting, SpurtHelper spurtHelper, Player player) : base(modProj)
        {
            this.setting = setting;
            SpurtHelper = spurtHelper;
            Player = player;
        }
        public class Setting
        {
            /// <summary>
            /// 突刺持续时间
            /// 以委托方式获取，以便在游戏中动态修改
            /// </summary>
            public Func<int> UseTimeMax;
            /// <summary>
            /// 突刺长度
            /// 以委托方式获取，以便在游戏中动态修改
            /// </summary>
            public Func<float> SpurtLength;
            public Func<bool> ActivationCondition;
        }
        public Action<SpurtHelper_GeneralOneSpurt> OnUse;
        public override void AI()
        {
            OnUse?.Invoke(this);
            Projectile.ai[0]++;
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter,true); // 中心位置更改
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Player.direction;
            Player.itemAnimation = Player.itemTime = 2;
            Player.ChangeDir(Projectile.direction);
            Player.heldProj = Projectile.whoAmI;

            SpurtHelper.Update(Projectile.Center, Player.direction, Projectile.velocity);
        }
        public virtual void SyncData()
        {
            if (Main.myPlayer == Player.whoAmI)
                Projectile.netUpdate = true;
        }
        public override void OnSkillActive()
        {
            Projectile.ai[0] = Projectile.ai[1] = Projectile.ai[2] = 0;
            SkillTimeOut = false;
            Projectile.originalDamage = Player.GetWeaponDamage(Player.HeldItem);
            Projectile.damage = Projectile.originalDamage;
            Projectile.knockBack = Player.GetWeaponKnockback(Player.HeldItem);
            Projectile.CritChance = Player.GetWeaponCrit(Player.HeldItem);
            TheUtility.ResetProjHit(Projectile);
            Projectile.numHits = 0;
            SpurtHelper.Change(setting.SpurtLength.Invoke(), Projectile.velocity, setting.UseTimeMax.Invoke(), Projectile.Size, 1, (spurt, sb, _) => SpurtHelper.ASpurtDraw_Proj(spurt, sb, Projectile.ai[0] / setting.UseTimeMax.Invoke()));
            if (Main.myPlayer == Player.whoAmI)
                Projectile.velocity = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);
            SyncData();
        }
        public override void OnSkillDeactivate()
        {
            Projectile.ai[0] = Projectile.ai[1] = Projectile.ai[2] = 0;
            SkillTimeOut = false;
            Projectile.originalDamage = Player.GetWeaponDamage(Player.HeldItem);
            Projectile.damage = Projectile.originalDamage;
            Projectile.knockBack = Player.GetWeaponKnockback(Player.HeldItem);
            Projectile.CritChance = Player.GetWeaponCrit(Player.HeldItem);
            TheUtility.ResetProjHit(Projectile);
            SyncData();
            Projectile.numHits = 0;
        }
        public override bool PreDraw(SpriteBatch sb, ref Color lightColor)
        {
            SpurtHelper.Draw(sb);
            return false;
        }
        public override bool ActivationCondition() => setting?.ActivationCondition?.Invoke() == true;
        public override bool SwitchCondition() => Projectile.ai[0] / setting.UseTimeMax.Invoke() > 1;
    }
}
