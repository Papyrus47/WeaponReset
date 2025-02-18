using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace WeaponReset.Content.Weapons.SummonStaff
{
    public abstract class SummonGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public float[] ai = new float[6];
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(ai[0]);
            binaryWriter.Write(ai[1]);
            binaryWriter.Write(ai[2]);
            binaryWriter.Write(ai[3]);
            binaryWriter.Write(ai[4]);
            binaryWriter.Write(ai[5]);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            ai[0] = binaryReader.ReadSingle();
            ai[1] = binaryReader.ReadSingle();
            ai[2] = binaryReader.ReadSingle();
            ai[3] = binaryReader.ReadSingle();
            ai[4] = binaryReader.ReadSingle();
            ai[5] = binaryReader.ReadSingle();
        }
    }
}
