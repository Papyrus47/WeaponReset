using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.SummonStaff.Spiders
{
    public class SpidersProj : SummonGlobalProj
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type >= ProjectileID.VenomSpider && entity.type <= ProjectileID.DangerousSpider;
        public override void AI(Projectile projectile)
        {
            projectile.knockBack = 0f;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            NPC npc = null;
            float dis = 200;
            foreach (NPC n in Main.npc)
            {
                if (n.Distance(projectile.Center) < dis && n.CanBeChasedBy() && target.whoAmI != n.whoAmI)
                {
                    npc = n;
                    dis = n.Distance(projectile.Center);
                }
            }

            if (npc == null)
            {
                modifiers.SourceDamage += 1f;
                modifiers.ScalingArmorPenetration += 1f;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slow, 60);
            NPC npc = null;
            float dis = 200;
            foreach(NPC n in Main.npc)
            {
                if (n.Distance(projectile.Center) < dis && n.CanBeChasedBy() && target.whoAmI != n.whoAmI)
                {
                    npc = n;
                    dis = n.Distance(projectile.Center);
                }
            }
            if (npc != null) // 有目标
            {
                Vector2 vel = (target.Center - projectile.Center).SafeNormalize(default);
                Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_OnHit(target), projectile.Center, vel, ProjectileID.PoisonFang, projectile.damage / 2, projectile.knockBack, projectile.owner);
                proj.velocity = vel * 10;
                proj.penetrate = -1;
            }
        }
    }
}
