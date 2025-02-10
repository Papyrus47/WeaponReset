using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using WeaponReset.Command.Configs;

namespace WeaponReset.Content.Weapons.Guns
{
    public class GunsItem : GlobalItem
    {
        public static HashSet<int> WeaponResetID;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return lateInstantiation && WeaponResetID?.Contains(entity.type) == true;
        }
        public override void Load()
        {
            // On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;\
            WeaponResetID ??= new HashSet<int>();
            WeaponResetID.TryAddArray([
                ItemID_Chinese.ºìÀ³µÂÇ¹,
                ItemID_Chinese.ìÝ·¢Ç¹,
                ItemID_Chinese.»ðÇ¹,
                ItemID_Chinese.¶áÃüÇ¹,
                ItemID_Chinese.ö±µ¯Ç¹,
                ItemID_Chinese.×óÂÖÊÖÇ¹,
                ItemID_Chinese.ÃÔÄãöè,
                ItemID_Chinese.Èý·¢ÁÔÇ¹,
                ItemID_Chinese.ËÄ¹Üö±µ¯Ç¹,
                ItemID_Chinese.ÊÖÇ¹,
                ItemID_Chinese.·ï»Ë±¬ÆÆÇ¹,
                ItemID_Chinese.ÆøÀ®°È,
                ItemID_Chinese.·¢ÌõÊ½Í»»÷²½Ç¹,
                ItemID_Chinese.öùÓã»ú¹ØÇ¹,
                ItemID_Chinese.Âêè§±¬ÆÆÇ¹,
                ItemID_Chinese.ÎÚ×È³å·æÇ¹,
                ItemID_Chinese.¾ÞÊÞöè,
                ItemID_Chinese.Î¬ÄÉË¹ÍòÄÜÇ¹,
                ItemID_Chinese.Õ½Êõö±µ¯Ç¹,
                ItemID_Chinese.¾Ñ»÷²½Ç¹,
                ItemID_Chinese.ÓñÃ×ÌÇ²½Ç¹,
                ItemID_Chinese.Á´Ê½»úÇ¹,
                ItemID_Chinese.ÍâÐÇö±µ¯Ç¹,
                ItemID_Chinese.Ì«¿Õº£ëà»úÇ¹,
                ]);
        }
        public override void SetDefaults(Item entity)
        {
            entity.holdStyle = ItemHoldStyleID.HoldFront;
        }
        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            float timeMax = (float)player.itemAnimation / player.itemAnimationMax;
            float factor = timeMax;
            factor = MathF.Pow(Math.Abs(factor - 0.5f), 1.2f);
            if(timeMax >= 0.55f)
                player.itemRotation -= factor * player.direction * MathHelper.PiOver4 * 0.25f; // ÐÞ¸ÄÍæ¼ÒÐý×ª½Ç¶È
            else
                player.itemRotation += factor * player.direction * MathHelper.PiOver4 * 0.25f; // ÐÞ¸ÄÍæ¼ÒÐý×ª½Ç¶È
        }
        public override void HoldStyle(Item item,Player player, Rectangle heldItemFrame)
        {
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.velocity.X / 10f);
            player.itemRotation = player.compositeFrontArm.rotation + MathHelper.PiOver2 * player.direction;
            player.itemLocation += new Vector2(player.width * -player.direction,player.height * -0.5f);
        }

        //public static void On_Player_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool(On_Player.orig_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool orig, Player self, Item sItem, ref int projToShoot, ref float speed, ref bool canShoot, ref int totalDamage, ref float KnockBack, out int usedAmmoItemId, bool dontConsume)
        //{
        //    if(!Setting.Instance.RangedUseReload)
        //    {
        //        orig.Invoke(self, sItem, ref projToShoot, ref speed, ref canShoot, ref totalDamage, ref KnockBack, out usedAmmoItemId, dontConsume);
        //        return;
        //    }
        //    #region ÏÂÃæÊÇ×Ô¼ºµÄÑ¡È¡µ¯Ò©½øÐÐÏûºÄ
        //    usedAmmoItemId = 0;
        //    Item ammo = null;
        //    canShoot = ammo != null;
        //    bool shootWithNoAmmo = false;
        //    if (!canShoot && !ItemLoader.NeedsAmmo(sItem, self))
        //    {
        //        ammo = ContentSamples.ItemsByType[sItem.useAmmo];
        //        if (ammo.ammo == sItem.useAmmo)
        //            canShoot = shootWithNoAmmo = true;
        //    }
        //    #endregion
        //}
    }
}