using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using WeaponReset.Command;
using static System.Net.Mime.MediaTypeNames;

namespace WeaponReset.Content.Weapons.MagicStaff
{
    public class MagicCricleProj : ModProjectile
    {
        public override string Texture => MagicCriclePath;
        public Player Player => Main.player[Projectile.owner];
        public Color DrawColor => Main.hslToRgb(Projectile.ai[0], Projectile.localAI[2],Luminosity ) * (Projectile.localAI[0]  / AlphaTime) * (MathHelper.SmoothStep(2.5f,0.5f, Projectile.ai[1] / Player.HeldItem.useTime * 1.5f));
        public int SpawnItemType;
        public float Luminosity;
        public const int AlphaTime = 30;
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
            //Projectile.minion = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
            writer.Write(SpawnItemType);
            writer.Write(Luminosity);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            Projectile.localAI[2] = reader.ReadSingle();
            SpawnItemType = reader.ReadInt32();
            Luminosity = reader.ReadSingle();
        }
        public override void AI()
        {
            if (Player.dead || !Player.active) // 清除弹幕的机制
            {
                Projectile.Kill();
                return;

            }
            if(SpawnItemType == 0)
                SpawnItemType = Player.HeldItem.type;
            if (SpawnItemType != Player.HeldItem.type && Projectile.timeLeft > 60)
                Projectile.timeLeft = 60;
            Projectile.scale = MathHelper.Lerp(Projectile.scale, Projectile.localAI[1] + Projectile.ai[1] * 0.2f - MathHelper.SmoothStep(Projectile.localAI[1] + Projectile.ai[1] * 0.2f, 0f,Projectile.timeLeft / 60f), 0.1f);
            //if (Player.controlUseTile) // 持续右键
            //{
            //    Projectile.timeLeft = 2;
            //    Player.itemTime = Player.itemAnimation = 2;
            //    Projectile.netUpdate = true;
            //}
            if (Main.netMode != NetmodeID.Server) // 不是服务器情况下
            {
                if (Projectile.localAI[0] < AlphaTime)
                    Projectile.localAI[0]++;

                if (Projectile.soundDelay <= 0 && Projectile.minionPos <= 1) // 产生声音
                {
                    Projectile.soundDelay = Player.HeldItem.useTime;
                    SoundEngine.PlaySound(SoundID.Item8, Player.position);
                }
            }
            if (Player.whoAmI == Main.myPlayer) // 不是客户端情况下
            {
                Projectile.netUpdate = true;
                Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(default);
            }
            //Projectile.Center = Player.MountedCenter + Projectile.velocity.SafeNormalize(default) * Projectile.ai[2];
            Projectile.rotation = Projectile.velocity.ToRotation();
            //Player.itemRotation = MathF.Atan2(Projectile.velocity.Y * Player.direction, Projectile.velocity.X * Player.direction) - MathHelper.PiOver2 * Player.direction;
            //Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Player.itemRotation);

            if (Player.whoAmI == Main.myPlayer && Projectile.timeLeft > 60) // 不是客户端情况下
            {
                Projectile.netUpdate = true;
                if (Projectile.ai[1]++ > Player.HeldItem.useTime * 1.5f)
                {
                    Projectile.ai[1] = 0;
                    if (Player.CheckMana(Player.HeldItem, -1, true))
                    {
                        //Projectile.localAI[2]++;
                        //var pos = Player.Center;
                        Player fackPlayer = Player.clientClone();
                        fackPlayer.Center = Projectile.Center;
                        fackPlayer.itemTime = fackPlayer.itemAnimation = 2;
                        fackPlayer.inventory[fackPlayer.selectedItem] = Player.HeldItem.Clone();
                        TheUtility.Player_ItemCheck_Shoot(fackPlayer, fackPlayer.HeldItem, fackPlayer.GetWeaponDamage(fackPlayer.HeldItem));
                    }
                    //if (Projectile.localAI[2] == 5 && Player.ownedProjectileCounts[Projectile.type] < 3)
                    //{
                    //    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<MagicCricleProj>(), 0, 0f, Player.whoAmI, Projectile.ai[0], 0f, Projectile.ai[2] * 1.05f);
                    //    proj.localAI[1] = Projectile.localAI[1] + Player.ownedProjectileCounts[Projectile.type] * 2;
                    //}
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server) // 不是服务器情况下
            {
                for(int i = 0; i < 50; i++)
                {
                    Dust.NewDustDirect(Projectile.Center + Projectile.velocity, 1, 1, DustID.FireworksRGB, Main.rand.NextFloatDirection() * Projectile.ai[2] * 0.05f, Main.rand.NextFloatDirection() * Projectile.ai[2] * 0.05f, 0, DrawColor).noGravity = true;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient) // 不是客户端情况下
                TheUtility.Player_ItemCheck_Shoot(Player, Player.HeldItem, Player.GetWeaponDamage(Player.HeldItem));
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            CustomVertexInfo[] vertexInfos =
                [
                    new(Projectile.Center + (new Vector2(-Projectile.width,-Projectile.height)* 0.5f) * Projectile.scale,DrawColor with {A = 0},new Vector3(0,0,0)), // 左上角
                    new(Projectile.Center + (new Vector2(Projectile.width,-Projectile.height)* 0.5f)* Projectile.scale,DrawColor with {A = 0},new Vector3(1,0,0)), // 右上角
                    new(Projectile.Center + (new Vector2(Projectile.width,Projectile.height) * 0.5f) * Projectile.scale,DrawColor with {A = 0},new Vector3(1,1,0)), // 右下角
                    new(Projectile.Center + (new Vector2(Projectile.width,Projectile.height) * 0.5f) * Projectile.scale,DrawColor with {A = 0},new Vector3(1,1,0)), // 右下角
                    new(Projectile.Center + (new Vector2(-Projectile.width,Projectile.height)* 0.5f) * Projectile.scale,DrawColor with {A = 0},new Vector3(0,1,0)), // 左下角
                    new(Projectile.Center + (new Vector2(-Projectile.width,-Projectile.height)* 0.5f) * Projectile.scale,DrawColor with {A = 0},new Vector3(0,0,0)), // 左上角
                ];
            Vector2 pos = default;
            for (int i = 0; i<vertexInfos.Length; i++)
            {
                pos += vertexInfos[i].Position ;
            }
            pos /= (float)vertexInfos.Length;
            for (int i = 0; i < vertexInfos.Length; i++) // 边缘旋转
            {
                Vector2 vel = pos - vertexInfos[i].Position;
                vel = vel.RotatedBy(Main.GlobalTimeWrappedHourly * 2.5f);
                vertexInfos[i].Position = pos + vel;
            }
            List<CustomVertexInfo> lightInfos =new();
            List<CustomVertexInfo> lightInfos2 = new();
            for (int i = 0; i < vertexInfos.Length; i++) // 中心旋转
            {
                Vector2 vel = pos - vertexInfos[i].Position;
                vel.X *= 0.3f;
                vel = vel.RotatedBy(Projectile.rotation);
                vertexInfos[i].Position = pos + vel - Main.screenPosition;
                lightInfos.Add(new CustomVertexInfo(pos + vel * 1.3f - Main.screenPosition, vertexInfos[i].Color * 0.5f, vertexInfos[i].TexCoord));
                lightInfos2.Add(new CustomVertexInfo(pos + vel * 0.6f - Main.screenPosition, vertexInfos[i].Color * 0.5f, vertexInfos[i].TexCoord));
            }
            //CustomVertexInfo[] drawInfos = TheUtility.GenerateTriangle(vertexInfos.ToList()).ToArray();
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            //var origin = gd.RasterizerState;
            //RasterizerState rasterizerState = new()
            //{
            //    CullMode = CullMode.None,
            //    FillMode = FillMode.WireFrame
            //};
            //gd.RasterizerState = rasterizerState;
            gd.Textures[0] = MagicCircle.Value;
            //gd.Textures[0] = TextureAssets.MagicPixel.Value;
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, lightInfos2.ToArray(), 0, 2);
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, lightInfos.ToArray(), 0, 2);
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, lightInfos.ToArray(), 0, 2);
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, vertexInfos, 0, 2);
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, vertexInfos, 0, 2);
            //gd.RasterizerState = origin;
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
