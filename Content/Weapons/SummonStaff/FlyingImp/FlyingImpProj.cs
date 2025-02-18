using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.FlyingImp
{
    public class FlyingImpProj : SummonGlobalProj
    {
        public List<int> OnHitNPCs = new();
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.ImpFireball;
        public override void AI(Projectile projectile)
        {
            if(projectile.owner == Main.myPlayer) // 本地端执行追踪
            {
                float distance = 300;
                NPC target = null;
                foreach(NPC npc in Main.npc)
                {
                    float dis = Vector2.Distance(npc.Center, projectile.Center);
                    if(npc.CanBeChasedBy() && !OnHitNPCs.Contains(npc.whoAmI) && dis < distance)
                    {
                        distance = dis;
                        target = npc;
                    }
                }
                if(target!= null)
                {
                    projectile.velocity = (projectile.velocity * 50 + (target.Center - projectile.Center).SafeNormalize(default) * 10) / 51f;
                }
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if(target.onFire || target.onFire2 || target.onFire3)
            {
                modifiers.SourceDamage += 0.2f;
                modifiers.ArmorPenetration += 10;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitNPCs.Add(target.whoAmI); // 记录击中npc的id
        }
    }
}
