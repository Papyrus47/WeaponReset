using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeaponReset.Content.Weapons.SummonStaff.DeadlySphere
{
    public class DeadlySphereProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.DeadlySphere;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.SimpleStrikeNPC(hit.Damage / 2, hit.HitDirection, hit.Crit, hit.Knockback, hit.DamageType);
        }
    }
}
