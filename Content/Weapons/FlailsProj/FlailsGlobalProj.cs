using Terraria;

namespace WeaponReset.Content.Weapons.FlailsProj
{
    public class FlailsGlobalProj : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return lateInstantiation && entity.aiStyle == ProjAIStyleID.Flail;
        }
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (!player.active || player.dead || player.noItems || player.CCed || Vector2.Distance(projectile.Center, player.Center) > 900f)
            {
                projectile.Kill();
                return false;
            }
            Vector2 mountedCenter = player.MountedCenter;
            bool doFastThrowDust = false;
            bool flag = true;
            bool flag2 = false;
            int num = 10;
            float num2 = 24f;
            float num3 = 800f;
            float num4 = 3f;
            float num5 = 16f;
            float num6 = 6f;
            float num7 = 48f;
            float num8 = 1f;
            float num9 = 14f;
            int num10 = 60;
            int num11 = 10;
            int num12 = 15;
            int num13 = 10;
            int num14 = num + 5;
            switch (projectile.type)
            {
                case 947:
                case 948:
                    num = 13;
                    num2 = 12f;
                    num5 = 8f;
                    num7 = 13f;
                    break;
                case 25:
                    num = 15;
                    num2 = 14f;
                    num5 = 10f;
                    num7 = 15f;
                    break;
                case 154:
                    num = 15;
                    num2 = 15f;
                    num5 = 11f;
                    num7 = 16f;
                    break;
                case 26:
                    num = 15;
                    num2 = 16f;
                    num5 = 12f;
                    num7 = 16f;
                    break;
                case 35:
                    num = 15;
                    num2 = 17f;
                    num5 = 14f;
                    num7 = 18f;
                    break;
                case 63:
                    num = 13;
                    num2 = 21f;
                    num5 = 20f;
                    num7 = 24f;
                    num12 = 12;
                    break;
                case 757:
                    num = 13;
                    num2 = 22f;
                    num5 = 22f;
                    num7 = 26f;
                    num12 = 12;
                    break;
                case 247:
                    num = 13;
                    num2 = 23f;
                    num12 = 12;
                    break;
            }

            /*
            float meleeSpeed = player.meleeSpeed;
            float num15 = 1f / meleeSpeed;
            */
            float num15 = 1f / player.GetAttackSpeed(DamageClass.Melee);
            num2 *= num15;
            num8 *= num15;
            num9 *= num15;
            num4 *= num15;
            num5 *= num15;
            num6 *= num15;
            num7 *= num15;
            float num16 = num2 * (float)num;
            float num17 = num16 + 160f;
            if ((int)projectile.ai[0] == 6) // 投掷出去降落
            {
                projectile.timeLeft = 2;
                if (player.controlUseTile && player.whoAmI == Main.myPlayer) // 如果玩家按着右键
                {
                    player.itemTime = player.itemAnimation = 2;
                    projectile.damage = (int)(projectile.originalDamage * Math.Min(projectile.velocity.Length() / 2 + 0.2f,20));
                    if(!player.controlUseItem)
                        projectile.velocity = (projectile.velocity * 20 + (Main.MouseWorld - projectile.Center).RotatedBy(0.1f).SafeNormalize(default) * 100) / 21f;
                }
                else if (!player.controlUseItem || projectile.Distance(mountedCenter) > num17) // 超过距离
                {
                    projectile.ai[0] = 4f;
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
                if (!projectile.shimmerWet)
                    projectile.velocity.Y += 0.8f;
                projectile.velocity.X *= 0.95f;
                player.ChangeDir((player.Center.X < projectile.Center.X) ? 1 : (-1));
                return false;
            }
            else if((int)projectile.ai[0] == 4)
            {
                return true;
            }
            return base.PreAI(projectile);
        }
    }
}
