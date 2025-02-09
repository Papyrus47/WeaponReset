using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using WeaponReset.Content.Weapons.OreSwords;

namespace WeaponReset.Content.Weapons.SPAtkSwords
{
    public class SPAtkSwordItems : BasicWeaponsItems<SPAtkSwordItems>
    {
        public static LocalizedText NoShiftText;
        public static LocalizedText ShiftText;
        public static int DefCD;
        public static int DefCDMax = 120;

        public int Charged;
        public int ChargedMax = 200;
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            ResetWeaponID ??= 
                [
                    ItemID_Chinese.断钢剑,
                    ItemID_Chinese.原版断钢剑,
                    ItemID_Chinese.叶绿军刀,
                    ItemID_Chinese.叶绿双刃刀,
                    ItemID_Chinese.钥匙剑,
                ];
        }
        public override void SetDefaults(Item entity)
        {
            if (CanResetWeapon)
            {
                entity.noUseGraphic = true;
                entity.noMelee = true;
                entity.scale *= 2;
                entity.useStyle = ItemUseStyleID.Rapier;
                entity.useTurn = false;
                entity.UseSound = null;
                entity.StatsModifiedBy.Add(Mod);
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Charged);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Charged = reader.ReadInt32();
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (DefCD > 0)
                DefCD--;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                string text = ShiftText.Value;
                text = text.Replace("<left>", Language.GetTextValue("Mods.WeaponReset.Left"));
                text = text.Replace("<right>", Language.GetTextValue("Mods.WeaponReset.Right"));
                tooltips.Add(new(Mod, "OnShift_ShowText", text));
            }
            else
            {
                tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.UseResetBind.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            }
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (DefCD <= 0)
                return;
            Texture2D tex = TextureAssets.BlackTile.Value;
            spriteBatch.Draw(tex, position, frame, Color.White with { A = 200 } * 0.8f, 0, origin, new Vector2(scale, scale * ((float)DefCD / DefCDMax)) * 1.3f, SpriteEffects.None, 0f);
        }
        public override void HoldItem(Item item, Player player)
        {
            if (!CanResetWeapon)
                return;
            // 这里是手持时候的效果
            // 我们需要在这里生成弹幕
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SPAtkSwordProj>()] <= 0) // 生成手持弹幕
            {
                int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.position, Vector2.Zero, ModContent.ProjectileType<SPAtkSwordProj>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
                Main.projectile[proj].originalDamage = Main.projectile[proj].damage;
            }

        }
    }
}
