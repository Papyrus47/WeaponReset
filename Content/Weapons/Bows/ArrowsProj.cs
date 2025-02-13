using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using WeaponReset.Content.Items.BowsBottle;
using static WeaponReset.Content.Weapons.Bows.BowsItem;

namespace WeaponReset.Content.Weapons.Bows
{
    public class ArrowsProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public Item UseBottel = new(0);
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if(source is BowUseBottleSource bottleSource)
            {
                UseBottel = bottleSource.UseBottle;
                if (projectile.owner == Main.myPlayer)
                    projectile.netUpdate = true;
            }
        }
        public override void AI(Projectile projectile)
        {
            (UseBottel?.ModItem as BasicBowsBottle)?.ProjAI(projectile);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            (UseBottel?.ModItem as BasicBowsBottle)?.OnHitNPC(Main.player[projectile.owner], target, hit, damageDone);
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            UseBottel ??= new(0);
            binaryWriter.Write(UseBottel.type);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            UseBottel ??= new(0);
            UseBottel.type = binaryReader.ReadInt32();
        }
    }
}
