using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Items.BowsBottle
{
    public abstract class BasicBowsBottle : ModItem
    {
        public virtual void ProjAI(Projectile projectile) { }
    }
}
