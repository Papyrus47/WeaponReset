using System.IO;
using Terraria.DataStructures;
using Terraria.GameContent;
using WeaponReset.Command;

namespace WeaponReset.Content.Weapons
{
    public class SwordQi : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.None}";
        public class SwordQiSource : IEntitySource
        {
            public SwingHelper swingHelper;
            public Draw Draw;
            public Func<float, Color> colorFunc;
            public string Context { get; }
            public SwordQiSource(SwingHelper swingHelper, Func<float, Color> colorFunc, Draw draw = null, string context = null)
            {
                this.swingHelper = swingHelper;
                swingHelper.DrawTrailCount = 6;
                Context = context;
                Draw = draw;
                this.colorFunc = colorFunc;
            }
        }
        public SwingHelper swingHelper;
        public delegate void Draw(SpriteBatch sb);
        public Draw draw;
        public Func<float, Color> colorFunc;
        //public DrawCecheSystem cecheSystem = new();
        //#region 绘制缓存系统
        //public class SwordQiDrawCeche : DrawCecheSystem.Ceche
        //{
        //    public SwingHelper SwingHelper;
        //    public int Time;
        //    public SwordQi swordQi;
        //    public SwordQiDrawCeche(SwingHelper swingHelper, SwordQi swordQi)
        //    {
        //        SwingHelper = swingHelper;
        //        Time = 2;
        //        this.swordQi = swordQi;
        //    }
        //    public override void UpdateCeche()
        //    {
        //        Vector2[] oldVels = SwingHelper.oldVels; // 获得旧速度
        //        //#region 保存旧速度
        //        //for (int i = oldVels.Length - 1; i > 0; i--)
        //        //{
        //        //    oldVels[i] = oldVels[i - 1];
        //        //}
        //        //#endregion
        //        if (Time-- < 0) // 时间到,移除
        //            Remove = true;
        //    }
        //    public override void DrawCeche()
        //    {
        //        SwingHelper.Swing_TrailingDraw(TextureAssets.Extra[201].Value, (_) => swordQi.colorFunc.Invoke(0.5f) * (1f - (swordQi.Projectile.alpha / 255f)), null);
        //    }
        //}
        //public override void PostAI()
        //{
        //    for (int i = 0; i < cecheSystem.CecheList.Count; i++)
        //    {
        //        cecheSystem.CecheList[i].UpdateCeche();
        //        if (cecheSystem.CecheList[i].Remove)
        //        {
        //            cecheSystem.CecheList.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //}
        //public override void PostDraw(Color lightColor)
        //{
        //    for (int i = 0; i < cecheSystem.CecheList.Count; i++)
        //    {
        //        cecheSystem.CecheList[i].DrawCeche();
        //        if (cecheSystem.CecheList[i].Remove)
        //        {
        //            i--;
        //            cecheSystem.CecheList.RemoveAt(i);
        //        }
        //    }
        //}
        //#endregion
        public override void SetDefaults()
        {
            Projectile.ownerHitCheck = true; // 弹幕检查是否隔墙
            Projectile.penetrate = 20; // 弹幕穿透
            Projectile.aiStyle = -1; // 弹幕AI样式
            Projectile.friendly = true; // 弹幕友好为true允许造成伤害
            Projectile.tileCollide = true; // 弹幕碰撞墙壁
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地NPC伤害（本地无敌帧）
            Projectile.localNPCHitCooldown = -1; // -1表示只能命中一次
            Projectile.width = 5;
            Projectile.extraUpdates = 3;
            Projectile.height = 5;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if(source is SwordQiSource swordQiSource)
            {
                swingHelper = swordQiSource.swingHelper;
                swingHelper.SpawnEntity = Projectile;
                for(int i = swingHelper.oldVels.Length - 1; i > 0; i--)
                {
                    if (swingHelper.oldVels[i] != default)
                    {
                        Array.Resize(ref swingHelper.oldVels, i + 1);
                        break;
                    }
                }
                draw = swordQiSource.Draw;
                colorFunc = swordQiSource.colorFunc;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            swingHelper.SendData(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            swingHelper.RendData(reader);
        }
        public override void AI()
        {
            if(Projectile.numHits > 0) // 弹幕击中次数大于0
            {
                Projectile.velocity *= 0.99f; // 弹幕速度减缓
                Projectile.netUpdate = true;
            }
            if (Projectile.velocity.Length() < 1)
            {
                if (Projectile.alpha < 255)
                    Projectile.alpha++;
                else
                    Projectile.Kill();
            }
            //cecheSystem.CecheList.Add(new SwordQiDrawCeche(swingHelper.Clone() as SwingHelper, this));
            //if (Projectile.timeLeft % 2 == 0)
            //{
            //    for (int j = 0; j < swingHelper.oldVels.Length; j++)
            //    {
            //        var dust = Dust.NewDustPerfect(Projectile.Center + swingHelper.oldVels[j], DustID.Smoke, Vector2.Zero, 0, colorFunc.Invoke(1f), 1f);
            //        dust.noGravity = true;
            //    }
            //}
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float r = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + swingHelper.oldVels[0], Projectile.Center + swingHelper.oldVels[^1],swingHelper._halfSizeLength * 2,ref r);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            draw?.Invoke(Main.spriteBatch);
            swingHelper.Swing_TrailingDraw(TextureAssets.Extra[201].Value, (x) => colorFunc.Invoke(x) * (1f - (Projectile.alpha / 255f)), null);
            return false;
        }
    }
}
