﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using WeaponReset.Content.Weapons;
using WeaponReset.Content.Weapons.Bows;
using WeaponReset.Content.Weapons.Guns;
using WeaponReset.Content.Weapons.OreSwords;
using WeaponReset.Content.Weapons.ShortSwords;
using WeaponReset.Content.Weapons.SPAtkSwords;

namespace WeaponReset
{
    public class WeaponResetPlayer : ModPlayer
    {
        /// <summary>
        /// 玩家下落速度
        /// </summary>
        public float playerFallSpeed;
        /// <summary>
        /// 玩家无敌时间
        /// </summary>
        public int PlayerImmune;
        /// <summary>
        /// 玩家无敌时间被命中
        /// </summary>
        public bool PlayerImmuneHit;
        /// <summary>
        /// 矿石剑类防御
        /// </summary>
        public int OreSwordDef;
        public bool AltPressed;
        public delegate void ModifyByHit(Player player, Entity target, ref Player.HurtModifiers hurtModifiers);
        private ModifyByHit _OnModifyByHit;
        public event ModifyByHit OnModifyByHit
        {
            add
            {
                _OnModifyByHit -= value;
                _OnModifyByHit += value;
            }
            remove => _OnModifyByHit -= value;
        }
        public override void ResetEffects()
        {
            PlayerImmuneHit = false;
            if (PlayerImmune > 0)
            {
                PlayerImmune--;
                Player.immune = true;
                Player.immuneTime = PlayerImmune;
            }
            if (OreSwordDef > 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Smoke, Main.rand.NextFloatDirection(), -2, 0, Color.White, 0.5f);
                    --OreSwordDef;
                    //if( <= 0)
                    //{
                    //    NetMessage.SyncDisconnectedPlayer(Player.whoAmI);
                    //}
                }
                if (OreSwordDef <= 0 && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SyncDisconnectedPlayer(Player.whoAmI);
                }
            }
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if (PlayerImmune > 0)
            {
                PlayerImmuneHit = true;
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            _OnModifyByHit?.Invoke(Player, npc, ref modifiers);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            _OnModifyByHit?.Invoke(Player, proj, ref modifiers);
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (WeaponReset.UseResetBind.JustPressed) // 按下
            {
                OreSwordItems.CanResetWeapon = !OreSwordItems.CanResetWeapon;
                SPAtkSwordItems.CanResetWeapon = !SPAtkSwordItems.CanResetWeapon;
                ShortSwordsItem.CanResetWeapon = !ShortSwordsItem.CanResetWeapon;
                foreach (Item item in Player.inventory)
                {
                    if (item.TryGetGlobalItem<OreSwordItems>(out _) || item.TryGetGlobalItem<SPAtkSwordItems>(out _) || item.TryGetGlobalItem<ShortSwordsItem>(out _))
                    {
                        int prefix = item.prefix;
                        int count = item.stack;
                        int rare = item.rare;
                        item.SetDefaults(item.type);
                        item.Prefix(prefix);
                        //item.prefix = prefix;
                        item.stack = count;
                        item.rare = rare;
                    }
                }
            }
            if (Main.keyState.GetPressedKeys().Contains(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
            {
                if (!AltPressed)
                {
                    AltPressed = true;
                    if (Player.HeldItem.TryGetGlobalItem<GunsItem>(out _))
                    {
                        bool findAll = true;
                        for (int i = GunsItem.FindGunsBulletIndex; i < 58; i++)
                        {
                            if (GunsItem.FindGunsBulletIndex != i && Player.inventory[i].stack > 0 && ItemLoader.CanChooseAmmo(Player.HeldItem, Player.inventory[i], Player))
                            {
                                GunsItem.FindGunsBulletIndex = i;
                                findAll = false;
                                break;
                            }
                        }
                        if (findAll)
                        {
                            for (int i = 0; i < 58; i++)
                            {
                                if (Player.inventory[i].stack > 0 && ItemLoader.CanChooseAmmo(Player.HeldItem, Player.inventory[i], Player))
                                {
                                    GunsItem.FindGunsBulletIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                    else if (Player.HeldItem.TryGetGlobalItem<BowsItem>(out _))
                    {
                        bool findAll = true;
                        for (int i = BowsItem.FindBowsArrowIndex; i < 58; i++)
                        {
                            if (BowsItem.FindBowsArrowIndex != i && Player.inventory[i].stack > 0 && ItemLoader.CanChooseAmmo(Player.HeldItem, Player.inventory[i], Player))
                            {
                                BowsItem.FindBowsArrowIndex = i;
                                findAll = false;
                                break;
                            }
                        }
                        if (findAll)
                        {
                            for (int i = 0; i < 58; i++)
                            {
                                if (Player.inventory[i].stack > 0 && ItemLoader.CanChooseAmmo(Player.HeldItem, Player.inventory[i], Player))
                                {
                                    BowsItem.FindBowsArrowIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                AltPressed = false;
            }
        }
        public override void UpdateEquips()
        {
            if (OreSwordDef > 0)
            {
                Player.statDefense.AdditiveBonus += 0.5f;
                Player.GetCritChance(DamageClass.Melee) += 15f;
            }
        }
        public override void UpdateLifeRegen()
        {
            if(OreSwordDef > 0)
            {
                Player.lifeRegen += 5;
            }
        }
        public override void PostUpdateEquips()
        {
            if (playerFallSpeed > 0)
            {
                Player.maxFallSpeed = playerFallSpeed;
                playerFallSpeed = -1;
            }
        }
        #region 联机同步
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)WeaponReset.NetUpdateWho.Player);
            packet.Write((byte)WeaponReset.NetUpdatePlayer.ResetEffect);
            packet.Write(Player.whoAmI);
            packet.Write(PlayerImmune);
            packet.Write(PlayerImmuneHit);
            packet.Write(OreSwordDef);
            packet.Send(toWho, fromWho);
        }
        public virtual void NetResetEffect(BinaryReader reader)
        {
            PlayerImmune = reader.ReadInt32();
            PlayerImmuneHit = reader.ReadBoolean();
            OreSwordDef = reader.ReadInt32();
        }
        #endregion
    }
}
