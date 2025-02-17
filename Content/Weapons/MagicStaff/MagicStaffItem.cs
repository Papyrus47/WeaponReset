using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

namespace WeaponReset.Content.Weapons.MagicStaff
{
    public class MagicStaffItem : BasicWeaponsItems<MagicStaffItem>
    {
        public bool noUseGraphic;
        public override bool InstancePerEntity => true;
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            ResetWeaponID ??= new();
            ResetWeaponID.TryAddArray(
            [
                ItemID_Chinese.钻石法杖,
                ItemID_Chinese.紫晶法杖,
                ItemID_Chinese.狱火叉,
                ItemID_Chinese.幽灵法杖,
                ItemID_Chinese.邪恶三叉戟,
                // ItemID_Chinese.无限智慧巨著, // 这个特殊
                ItemID_Chinese.天候棒,
                ItemID_Chinese.剃刀松,
                ItemID_Chinese.水晶蛇,
                ItemID_Chinese.霹雳法杖,
                ItemID_Chinese.魔晶碎块,
                //113, // 魔法飞弹
                ItemID_Chinese.魔刺,
                ItemID_Chinese.流星法杖,
                ItemID_Chinese.裂天剑,
                //ItemID_Chinese.烈焰火鞭,
                ItemID_Chinese.蓝玉法杖,
                ItemID_Chinese.剧毒法杖,
                5147, // 结霜魔棒
                ItemID_Chinese.火之花,
                ItemID_Chinese.火花魔棒,
                ItemID_Chinese.黄玉法杖,
                ItemID_Chinese.琥珀法杖,
                ItemID_Chinese.红玉法杖,
                ItemID_Chinese.寒霜之花,
                ItemID_Chinese.寒霜法杖,
                ItemID_Chinese.海蓝权杖,
                ItemID_Chinese.共鸣权杖,
                ItemID_Chinese.翡翠法杖,
                ItemID_Chinese.夺命杖,
                ItemID_Chinese.毒液法杖,
                // ItemID_Chinese.彩虹魔杖,
                ItemID_Chinese.蝙蝠权杖,
                ItemID_Chinese.爆裂藤蔓,
                ItemID_Chinese.暴雪法杖,
                ItemID_Chinese.暗影束法杖,
                ItemID_Chinese.大地法杖,
            ]);
        }
        public static Color GetColor(Item item)
        {
            // 我 爱 deep seek 小姐
            switch (item.type)
            {
                //================ 宝石权杖系列 ================
                case ItemID_Chinese.钻石法杖: return new Color(185, 242, 255); // 冷白光 (钻石折射效果)
                case ItemID_Chinese.红玉法杖: return Color.Crimson;          // 深红 (红宝石质感)
                case ItemID_Chinese.蓝玉法杖: return new Color(0, 128, 255);  // 钴蓝 (深海宝石)
                case ItemID_Chinese.黄玉法杖: return new Color(255, 200, 0);  // 琥珀金 (黄玉暖光)
                case ItemID_Chinese.紫晶法杖: return new Color(170, 0, 255);  // 离子紫 (紫水晶光泽)
                case ItemID_Chinese.翡翠法杖: return Color.Lime;             // 荧光绿 (翡翠通透感)
                case ItemID_Chinese.琥珀法杖: return new Color(255, 150, 50); // 熔融琥珀 (流动橙色)
                case ItemID_Chinese.海蓝权杖: return new Color(0, 200, 255);  // 浅海蓝 (浪花效果)

                //=============== 元素法杖 ================
                //-- 火系 --
                case ItemID_Chinese.狱火叉: return new Color(255, 80, 0);   // 熔岩核心色
                case ItemID_Chinese.火之花: return new Color(255, 60, 0);   // 暗火余烬
                case ItemID_Chinese.火花魔棒: return new Color(255, 120, 0);  // 火星迸溅

                //-- 冰系 --
                case ItemID_Chinese.寒霜法杖: return new Color(128, 255, 255); // 极地寒光
                case ItemID_Chinese.暴雪法杖: return new Color(200, 230, 255); // 暴雪云层灰蓝
                case ItemID_Chinese.寒霜之花: return new Color(180, 220, 255); // 冰晶花瓣
                case 5147: return new Color(180, 230, 255); // 结霜魔棒 (霜雾蓝)

                //-- 雷电系 --
                case ItemID_Chinese.霹雳法杖: return new Color(200, 0, 255);  // 电离紫
                case ItemID_Chinese.天候棒: return Color.Gold;              // 雷云金色

                //-- 暗影系 --
                case ItemID_Chinese.暗影束法杖: return new Color(80, 0, 160);   // 深渊紫黑
                case ItemID_Chinese.幽灵法杖: return Color.White; // 半透明灵体

                //============== 生物/自然系 ==============
                case ItemID_Chinese.剃刀松: return new Color(139, 69, 19);  // 松树皮棕
                case ItemID_Chinese.爆裂藤蔓: return new Color(100, 200, 50); // 剧毒植物绿
                case ItemID_Chinese.蝙蝠权杖: return new Color(80, 80, 80);   // 蝙蝠灰
                case ItemID_Chinese.水晶蛇: return Color.HotPink;  // Pink

                //============== 特殊机制武器 ==============
                case ItemID_Chinese.邪恶三叉戟: return new Color(120, 0, 200);  // 腐化紫
                case ItemID_Chinese.魔晶碎块: return new Color(0, 80, 255);   // 晶簇蓝
                case ItemID_Chinese.裂天剑: return new Color(0, 100, 255);  // 空间裂隙蓝
                case ItemID_Chinese.大地法杖:
                case ItemID_Chinese.流星法杖: return new Color(220, 220, 220); // 陨石灰白

                //============== 毒系武器组 ==============
                case ItemID_Chinese.剧毒法杖: return new Color(100, 255, 0);  // 荧光毒液绿
                case ItemID_Chinese.毒液法杖: return new Color(150, 0, 255);  // 神经毒素紫
                case ItemID_Chinese.魔刺: return new Color(0, 255, 50);   // 毒刺尖端的危险绿

                //============= 特殊材质武器 ==============
                case ItemID_Chinese.夺命杖: return new Color(200, 200, 200); // 白骨质感
                case ItemID_Chinese.共鸣权杖: return Color.HotPink * 1.2f; // 声波共振粉
            }
            return Color.White;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                string text = ShiftText.Value;
                //text = text.Replace("<left>", Language.GetTextValue("Mods.WeaponReset.Left"));
                //text = text.Replace("<right>", Language.GetTextValue("Mods.WeaponReset.Right"));
                tooltips.Add(new(Mod, "OnShift_ShowText", text));
            }
            else
            {
                tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.UseResetBind.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            }
            base.ModifyTooltips(item, tooltips);
        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            noUseGraphic = entity.noUseGraphic;
        }
        public override bool CanShoot(Item item, Player player)
        {
            //if (player.altFunctionUse == 2)
            //{
            //    item.noUseGraphic = true;
            //}
            //else
            //{
            //    item.noUseGraphic = noUseGraphic;
            //}
            return base.CanShoot(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<MagicCricleProj>()] < 3 && player.CheckMana(100,true))
            {
                var proj = Projectile.NewProjectileDirect(source, Main.MouseWorld + Main.rand.NextVector2CircularEdge(300,300), velocity, ModContent.ProjectileType<MagicCricleProj>(), damage, knockback, player.whoAmI, Main.rgbToHsl(GetColor(item)).X, 0f,item.Size.Length() * 0.6f);
                proj.localAI[1] = 6f;
                (proj.ModProjectile as MagicCricleProj).Luminosity = Main.rgbToHsl(GetColor(item)).Z;
                proj.localAI[2] = Main.rgbToHsl(GetColor(item)).Y;
                proj.netUpdate = true;
                return false;
            }
            return true;
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            return true;
        }
    }
}
