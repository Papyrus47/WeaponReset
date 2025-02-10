using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using static System.Net.Mime.MediaTypeNames;

namespace WeaponReset.Command.GlobalNPCs
{
    public class GeneralGlobalNPC : GlobalNPC
    {
        public struct NPCBuffs
        {
            public int Conuts;
            public int Strength;
        }
        public NPCBuffs Vulnerable;
        public override bool InstancePerEntity => true;
        public override void Load()
        {
            On_NPC.StrikeNPC_int_float_int_bool_bool_bool += On_NPC_StrikeNPC_int_float_int_bool_bool_bool;
            On_NPC.StrikeNPC_HitInfo_bool_bool += On_NPC_StrikeNPC_HitInfo_bool_bool;
        }

        public static int On_NPC_StrikeNPC_HitInfo_bool_bool(On_NPC.orig_StrikeNPC_HitInfo_bool_bool orig, NPC self, NPC.HitInfo hit, bool fromNet, bool noPlayerInteraction)
        {
            if (self.TryGetGlobalNPC<GeneralGlobalNPC>(out var npc) && npc.Vulnerable.Conuts > 0)
            {
                npc.Vulnerable.Conuts--;
                hit.Damage += (int)(hit.Damage * (npc.Vulnerable.Strength / 10f));
                if (npc.Vulnerable.Conuts <= 0)
                    npc.Vulnerable.Strength = 0;
            }
            return orig.Invoke(self, hit, fromNet, noPlayerInteraction);
        }

        public static int On_NPC_StrikeNPC_int_float_int_bool_bool_bool(On_NPC.orig_StrikeNPC_int_float_int_bool_bool_bool orig, NPC self, int Damage, float knockBack, int hitDirection, bool crit, bool fromNet, bool noPlayerInteraction)
        {
            if (self.TryGetGlobalNPC<GeneralGlobalNPC>(out var npc) && npc.Vulnerable.Conuts > 0)
            {
                npc.Vulnerable.Conuts--;
                Damage = (int)(Damage * (1f + npc.Vulnerable.Strength / 10f));
            }
            return orig.Invoke(self, Damage, knockBack, hitDirection, crit, fromNet, noPlayerInteraction);
        }

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Vulnerable.Conuts);
            binaryWriter.Write(Vulnerable.Strength);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            Vulnerable.Conuts = binaryReader.ReadInt32();
            Vulnerable.Strength = binaryReader.ReadInt32();
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Vulnerable.Conuts > 0)
            {
                drawColor = Color.Lerp(drawColor, Color.Purple, 0.5f);
            }
        }
    }
}
