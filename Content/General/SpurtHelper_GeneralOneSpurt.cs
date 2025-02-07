using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            public Func<float> UseTimeMax;
            /// <summary>
            /// 突刺长度
            /// 以委托方式获取，以便在游戏中动态修改
            /// </summary>
            public Func<float> SpurtLength;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            float time = Projectile.ai[0] / setting.UseTimeMax.Invoke();
            float length = setting.SpurtLength.Invoke();
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter);
        }
    }
}
