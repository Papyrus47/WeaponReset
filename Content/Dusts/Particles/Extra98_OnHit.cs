using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Dusts.Particles
{
    public class Extra98_OnHit : Extra98
    {
        public override void SetProperty()
        {
            base.SetProperty();
            Lifetime = 10;
        }
        public override bool ShouldUpdatePosition() => false;
    }
}
