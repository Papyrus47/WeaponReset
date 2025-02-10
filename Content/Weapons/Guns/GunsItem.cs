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
                ItemID_Chinese.������ǹ,
                ItemID_Chinese.�ݷ�ǹ,
                ItemID_Chinese.��ǹ,
                ItemID_Chinese.����ǹ,
                ItemID_Chinese.����ǹ,
                ItemID_Chinese.������ǹ,
                ItemID_Chinese.������,
                ItemID_Chinese.������ǹ,
                ItemID_Chinese.�Ĺ�����ǹ,
                ItemID_Chinese.��ǹ,
                ItemID_Chinese.��˱���ǹ,
                ItemID_Chinese.������,
                ItemID_Chinese.����ʽͻ����ǹ,
                ItemID_Chinese.�������ǹ,
                ItemID_Chinese.��觱���ǹ,
                ItemID_Chinese.���ȳ��ǹ,
                ItemID_Chinese.������,
                ItemID_Chinese.ά��˹����ǹ,
                ItemID_Chinese.ս������ǹ,
                ItemID_Chinese.�ѻ���ǹ,
                ItemID_Chinese.�����ǲ�ǹ,
                ItemID_Chinese.��ʽ��ǹ,
                ItemID_Chinese.��������ǹ,
                ItemID_Chinese.̫�պ����ǹ,
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
                player.itemRotation -= factor * player.direction * MathHelper.PiOver4 * 0.25f; // �޸������ת�Ƕ�
            else
                player.itemRotation += factor * player.direction * MathHelper.PiOver4 * 0.25f; // �޸������ת�Ƕ�
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
        //    #region �������Լ���ѡȡ��ҩ��������
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