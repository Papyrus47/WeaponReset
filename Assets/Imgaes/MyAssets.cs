using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Assets.Imgaes
{
    public static class MyAssets
    {
        public static string AssetPath = "WeaponReset/Assets/Imgaes/";
        public static Asset<Texture2D> MagicCircle = ModContent.Request<Texture2D>(AssetPath + nameof(MagicCircle));
    }
}
