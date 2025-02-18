using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;

namespace WeaponReset.Content.Weapons.SummonStaff.VampireFrog
{
    public class LightBallProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.Size = new Vector2(16);
            Projectile.penetrate = 1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void AI()
        {
            NPC target = Projectile.FindTargetWithinRange(400,true);
            Projectile.ai[0]++;
            Projectile.scale = (Projectile.ai[0] / 2) % 1.5f + 1f;
            if (Projectile.ai[0] < 10)
            {
                Projectile.damage = 0;
            }
            else
            {
                Projectile.damage = Projectile.originalDamage;
            }
            if (target != null)
            {
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
                Projectile.velocity = (Projectile.velocity * 10 + (target.Center - Projectile.Center).SafeNormalize(default) * 10f) / 11f;
            }
            else
            {
                Projectile.damage = 0;
                Projectile.tileCollide = true;
                Projectile.velocity.Y += 0.4f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.Red with { A = 0} * 0.1f;
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * 4, 0f, tex.Size() * 0.5f, Projectile.scale * 0.3f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * 3, 0f, tex.Size() * 0.5f, Projectile.scale * 0.7f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * 2, 0f, tex.Size() * 0.5f, Projectile.scale * 0.9f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, 0f, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(Projectile.friendly)
                Main.player[Projectile.owner].Heal(1);
        }
    }
}
