using Terraria.DataStructures;
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
            public string Context { get; }
            public SwordQiSource(SwingHelper swingHelper, Draw draw, string context = null)
            {
                this.swingHelper = swingHelper;
                Context = context;
                Draw = draw;
            }
        }
        public SwingHelper swingHelper;
        public delegate void Draw(SpriteBatch sb);
        public Draw draw;
    }
}
