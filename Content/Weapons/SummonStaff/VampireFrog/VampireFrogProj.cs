using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace WeaponReset.Content.Weapons.SummonStaff.VampireFrog
{
    public class VampireFrogProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.VampireFrog;
        public override void AI(Projectile projectile)
        {
            if(Main.myPlayer == projectile.owner) // 吐舌头最长的一帧
            {
                if ((projectile.frame == Main.projFrames[projectile.type] - 1 || projectile.frame == Main.projFrames[projectile.type] - 4))
                {
                    if (ai[0] == 0) // 发射弹幕
                    {
                        ai[0] = 1;
                        projectile.netUpdate = true;
                        Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitX * projectile.spriteDirection, ModContent.ProjectileType<LightBallProj>(), projectile.damage / 2, projectile.knockBack * 2f, projectile.owner);
                    }
                }
                else
                {
                    ai[0] = 0;
                }    
            }
        }
    }
}
