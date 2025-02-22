using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace WeaponReset.Content.Weapons.SummonStaff.BatOfLight
{
    public class BatOfLightProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.BatOfLight;
        public override bool PreAI(Projectile projectile)
        {
            int index = (int)projectile.ai[1];
            if(Main.npc.IndexInRange(index) && Main.npc[index].CanBeChasedBy())
            {
                if(projectile.ai[0] < 15 && projectile.ai[0] > 0 && ai[0] < 500)
                {
                    projectile.ai[0] = 80;
                    projectile.netUpdate = true;
                    if (projectile.OwnerMinionAttackTargetNPC != null)
                    {
                        projectile.extraUpdates = 1;
                        projectile.ai[1] = Main.player[projectile.owner].MinionAttackTargetNPC;
                    }
                    else
                    {
                        projectile.extraUpdates = 0;
                        int target = -1;
                        projectile.Minion_FindTargetInRange(1000, ref target, true);
                        if (target != -1)
                            projectile.ai[1] = target;
                    }
                    TheUtility.ResetProjHit(projectile);
                }
            }
            if ((int)projectile.ai[0] == 0 && ai[0] > 0)
            {
                projectile.extraUpdates = 0;
                Main.player[projectile.owner].Heal(Math.Min(50,(int)ai[0] / 10));
                projectile.netUpdate = true;
                ai[0] = 0;
            }
            return base.PreAI(projectile);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ai[0] += hit.Damage;
        }
    }
}
