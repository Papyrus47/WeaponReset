using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponReset.Content.Weapons.MagicStaff
{
    public class MagicStaffItem : BasicWeaponsItems<MagicStaffItem>
    {
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
                113, // 魔法飞弹
                ItemID_Chinese.魔刺,
                ItemID_Chinese.流星法杖,
                ItemID_Chinese.裂天剑,
                ItemID_Chinese.烈焰火鞭,
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
                ItemID_Chinese.彩虹魔杖,
                ItemID_Chinese.蝙蝠权杖,
                ItemID_Chinese.爆裂藤蔓,
                ItemID_Chinese.暴雪法杖,
                ItemID_Chinese.暗影束法杖
            ]);
        }
    }
}
