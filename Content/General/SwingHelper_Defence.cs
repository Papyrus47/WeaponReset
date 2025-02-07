using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using WeaponReset.Command;

namespace WeaponReset.Content.General
{
    /// <summary>
    /// 防御
    /// </summary>
    public class SwingHelper_Defence : SwingHelper_GeneralSwing
    {
        public bool CanDmg = false;
        public SwingHelper_Defence(ModProjectile modProjectile, Setting setting, PreAtk preAtk, PostAtk postAtk, OnAtk onAtk, SwingHelper swingHelper, Player player) : base(modProjectile, setting, preAtk, postAtk, onAtk, swingHelper, player)
        {
        }
        public override void AI()
        {
            Projectile.spriteDirection = Player.direction * setting.SwingDirectionChange.ToDirectionInt();
            Player.heldProj = Projectile.whoAmI;
            Projectile.hide = true;

            switch ((int)Projectile.ai[0])
            {
                case 0: // 渐变
                    Projectile.ai[1]++;
                    float time = Projectile.ai[1] / preAtk.PreTime;
                    SwingHelper.Change_Lerp(setting.StartVel, time, setting.VelScale, time * 2, setting.VisualRotation, time);
                    SwingHelper.ProjFixedPlayerCenter(Player, -setting.SwingLenght / 2, true);
                    SwingHelper.SwingAI(setting.SwingLenght, Player.direction, 0);
                    preAtk.OnUse?.Invoke(this);
                    if (time > 1)
                    {
                        Projectile.ai[1] = 0;
                        Projectile.ai[0]++;
                        preAtk.OnChange?.Invoke(this);
                        SoundEngine.PlaySound(playSound, Player.position);
                        SyncData();
                    }
                    break;
                case 1: // 挥舞
                    SwingHelper.ProjFixedPlayerCenter(Player, -setting.SwingLenght / 2, true);
                    Projectile.extraUpdates = 4;
                    Projectile.ai[1]++;
                    float swingTime = Projectile.ai[1] / (onAtk.SwingTime * (Projectile.extraUpdates + 1));
                    if (swingTime > 1)
                    {
                        Projectile.ai[1] = 0;
                        Projectile.ai[0]++;
                        onAtk.OnChange?.Invoke(this);
                        SyncData();
                        break;
                    }
                    onAtk.OnUse?.Invoke(this);
                    swingTime = onAtk.TimeChange.Invoke(swingTime);

                    SwingHelper.SwingAI(setting.SwingLenght, Player.direction, swingTime * setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt());
                    break;
                case 2: // 超时
                    Projectile.ai[1]++;
                    Projectile.extraUpdates = 0;
                    if (Projectile.ai[1] > postAtk.PostMaxTime)
                    {
                        SyncData();
                        SkillTimeOut = true;
                        break;
                    }
                    SwingHelper.ProjFixedPlayerCenter(Player, -setting.SwingLenght / 2, true);
                    SwingHelper.SetNotSaveOldVel();
                    postAtk.OnEnd?.Invoke(this);
                    SwingHelper.SwingAI(setting.SwingLenght, Player.direction, setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt() * (1 + Projectile.ai[1] * 0.001f));
                    break;
            }
        }
        public override bool? CanDamage() => CanDmg ? base.CanDamage() : false;
    }
}
