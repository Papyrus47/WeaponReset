using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using WeaponReset.Content.Items.BowsBottle;

namespace WeaponReset.Content.Weapons.Bows
{
    public class BowsItem : BasicWeaponsItems<BowsItem>
    {
        public class BowUseBottleSource : EntitySource_ItemUse_WithAmmo
        {
            public Item UseBottle;
            public BowUseBottleSource(Player player, Item item, int ammoItemId, string context = null) : base(player, item, ammoItemId, context)
            {
            }
        }
        /// <summary>
        /// 寻找弹药索引
        /// </summary>
        public static int FindBowsArrowIndex;
        /// <summary>
        /// 寻找瓶子索引
        /// </summary>
        public static int FindBowsBottleIndex;
        /// <summary>
        /// 消耗的瓶子
        /// </summary>
        public Item UseBottle;
        public override void Load()
        {
            NoShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(NoShiftText), () => "No Shift");
            ShiftText = Language.GetOrRegister("Mods." + GetType().Namespace + "." + nameof(ShiftText), () => "Shift");

            // On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;\
            ResetWeaponID ??= new HashSet<int>();
            /*
                弩ID:
                ItemID_Chinese.钴连弩,
                ItemID_Chinese.钯金连弩,
                ItemID_Chinese.秘银连弩,
                ItemID_Chinese.山铜连弩,
                ItemID_Chinese.精金连弩,
                ItemID_Chinese.钛金连弩,
                ItemID_Chinese.神圣连弩,
                ItemID_Chinese.叶绿连弩,
             */
            ResetWeaponID.TryAddArray([
                ItemID_Chinese.木弓,
                ItemID_Chinese.铜弓,
                ItemID_Chinese.红木弓,
                ItemID_Chinese.锡弓,
                ItemID_Chinese.乌木弓,
                ItemID_Chinese.铁弓,
                ItemID_Chinese.暗影木弓,
                ItemID_Chinese.铅弓,
                ItemID_Chinese.银弓,
                ItemID_Chinese.钨弓,
                5852, // 灰烬木弓
                ItemID_Chinese.金弓,
                ItemID_Chinese.铂金弓,
                ItemID_Chinese.恶魔弓,
                ItemID_Chinese.肌腱弓,
                ItemID_Chinese.血雨弓,
                ItemID_Chinese.蜂膝弓,
                ItemID_Chinese.地狱之翼弓,
                ItemID_Chinese.熔火之怒,
                ItemID_Chinese.珍珠木弓,
                ItemID_Chinese.骸骨弓,
                ItemID_Chinese.冰雪弓,
                ItemID_Chinese.代达罗斯风暴弓,
                ItemID_Chinese.暗影焰弓,
                ItemID_Chinese.幽灵凤凰,
                ItemID_Chinese.脉冲弓,
                ItemID_Chinese.尖桩发射器,
                ItemID_Chinese.空中祸害,
                ItemID_Chinese.海啸,
                ItemID_Chinese.日暮,
                ItemID_Chinese.幻影弓,
                #region 弩
                ItemID_Chinese.钴连弩,
                ItemID_Chinese.钯金连弩,
                ItemID_Chinese.秘银连弩,
                ItemID_Chinese.山铜连弩,
                ItemID_Chinese.精金连弩,
                ItemID_Chinese.钛金连弩,
                ItemID_Chinese.神圣连弩,
                ItemID_Chinese.叶绿连弩
                #endregion
                ]);

            On_Player.ChooseAmmo += On_Player_ChooseAmmo;
            On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
            On_Player.GetProjectileSource_Item_WithPotentialAmmo += On_Player_GetProjectileSource_Item_WithPotentialAmmo;
        }

        public static IEntitySource On_Player_GetProjectileSource_Item_WithPotentialAmmo(On_Player.orig_GetProjectileSource_Item_WithPotentialAmmo orig, Player self, Item item, int ammoItemId)
        {
            if (ResetWeaponID.Contains(item.type) && item.TryGetGlobalItem<BowsItem>(out var bow))
            {
                return new BowUseBottleSource(self, item, ammoItemId, null)
                {
                    UseBottle = bow.UseBottle // 消耗的瓶子
                };
            }
            return orig.Invoke(self, item, ammoItemId);
        }

        public static void On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool(On_Player.orig_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool orig, Player self, Item sItem, ref int projToShoot, ref float speed, ref bool canShoot, ref int totalDamage, ref float KnockBack, out int usedAmmoItemId, bool dontConsume)
        {
            orig.Invoke(self, sItem, ref projToShoot, ref speed, ref canShoot, ref totalDamage, ref KnockBack, out usedAmmoItemId, dontConsume);

            if (ResetWeaponID.Contains(sItem.type)) // 是弓箭类
            {
                for (int i = FindBowsBottleIndex; i < self.inventory.Length; i++)
                {
                    if (self.inventory[i].stack > 0 && self.inventory[i].ModItem is BasicBowsBottle)
                    {
                        if (FindBowsBottleIndex != i)
                            FindBowsBottleIndex = i;

                        if (sItem.TryGetGlobalItem<BowsItem>(out var bow)) // 弓类物品
                        {
                            if (self.inventory[i].consumable && !dontConsume && ItemLoader.ConsumeItem(self.inventory[i],self))
                                self.inventory[i].stack--;
                            Item item = new(self.inventory[i].type);
                            bow.UseBottle = item; // 消耗的瓶子赋值
                            break;
                        }
                    }
                }
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            if (WeaponReset.SpBind1.JustPressed) // 特殊按键1按下
            {
                bool findAll = true;
                for (int i = FindBowsBottleIndex; i < 58; i++)
                {
                    if (FindBowsBottleIndex != i && player.inventory[i].stack > 0 && player.inventory[i].ModItem is BasicBowsBottle)
                    {
                        FindBowsBottleIndex = i;
                        findAll = false;
                        break;
                    }
                }
                if (findAll)
                {
                    for (int i = 0; i < 58; i++)
                    {
                        if (player.inventory[i].stack > 0 && player.inventory[i].ModItem is BasicBowsBottle)
                        {
                            FindBowsBottleIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        //public override bool IsLoadingEnabled(Mod mod) => false; // 注释掉以加载
        public static Item On_Player_ChooseAmmo(On_Player.orig_ChooseAmmo orig, Player self, Item weapon)
        {
            if (weapon.TryGetGlobalItem<BowsItem>(out _))
            {
                for (int i = FindBowsArrowIndex; i < 58; i++)
                {
                    if (self.inventory[i].stack > 0 && ItemLoader.CanChooseAmmo(weapon, self.inventory[i], self))
                    {
                        return self.inventory[i];
                    }
                }
            }
            return orig.Invoke(self, weapon);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {
                string text = ShiftText.Value;
                tooltips.Add(new(Mod, "OnShift_ShowText", text));
            }
            else
            {
                tooltips.Add(new(Mod, "OnShift_HideText", NoShiftText.WithFormatArgs(WeaponReset.SpBind1.GetAssignedKeys(Terraria.GameInput.InputMode.Keyboard).FirstOrDefault("None")).Value));
            }
            base.ModifyTooltips(item, tooltips);
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player localPlayer = Main.LocalPlayer;
            if (localPlayer.HeldItem == item) // 玩家手持物品
            {
                #region 绘制子弹类型
                Item drawItem = localPlayer.ChooseAmmo(item); // 选择弹药
                if (drawItem != null && !drawItem.IsAir) // 存在才开始下面的绘制内容
                {
                    Texture2D tex = TextureAssets.Item[drawItem.type].Value;
                    Rectangle rect = default;
                    if (Main.itemAnimations[drawItem.type] == null)
                        rect = tex.Frame(horizontalFrames: 1, verticalFrames: 1, 0, 0, 0, 0);
                    else
                        rect = tex.Frame(1, Main.itemAnimations[drawItem.type].FrameCount, 0, Main.itemAnimations[drawItem.type].Frame, 0, 0);
                    spriteBatch.Draw(tex, Main.LocalPlayer.Top + new Vector2(0, -20) - Main.screenPosition, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);
                }
                #endregion
                #region 绘制瓶子类型
                if(FindBowsBottleIndex != -1 && localPlayer.inventory[FindBowsBottleIndex] != null && localPlayer.inventory[FindBowsBottleIndex].stack > 0 && localPlayer.inventory[FindBowsBottleIndex].ModItem is BasicBowsBottle)
                {
                    Texture2D tex = TextureAssets.Item[localPlayer.inventory[FindBowsBottleIndex].type].Value;
                    Main.instance.LoadItem(localPlayer.inventory[FindBowsBottleIndex].type);
                    Rectangle rect = default;
                    if (Main.itemAnimations[drawItem.type] == null)
                        rect = tex.Frame(horizontalFrames: 1, verticalFrames: 1, 0, 0, 0, 0);
                    else
                        rect = tex.Frame(1, Main.itemAnimations[drawItem.type].FrameCount, 0, Main.itemAnimations[drawItem.type].Frame, 0, 0);
                    spriteBatch.Draw(tex, Main.LocalPlayer.Top + new Vector2(0, -40 - rect.Height) - Main.screenPosition, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);
                }
                #endregion
            }
        }
    }
}
