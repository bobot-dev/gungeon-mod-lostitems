using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionStress : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Stress";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Stress-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionStress>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Fire rate increases as hp gets lower";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            //player.PostProcessProjectile += this.PostProcessProjectile;
            playerHP = player.healthHaver.GetCurrentHealthPercentage();
            player.healthHaver.OnDamaged += this.PlayerTookDamage;
            string playerHPMSG = playerHP + "";
            ETGModConsole.Log(playerHPMSG, false);


            float curFireRate = Owner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float curReload = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
            float newFireRate = curFireRate + (1 - playerHP);
            float newReload = curReload + (1 - playerHP);
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, Owner);
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload, Owner);
            fireRateBuff = newFireRate - curFireRate;
            reloadBuff = newReload - curReload;
            statsChanged = true;
        }
        private void PlayerTookDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            playerHP = Owner.healthHaver.GetCurrentHealthPercentage();
            string playerHPMSG = playerHP + "";
            if (statsChanged == true) { 

                ETGModConsole.Log(playerHPMSG, false);
                //if (fireRateBuff <= 0) return;
                float curFireRate2 = Owner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
                float newFireRate2 = curFireRate2 - fireRateBuff;
                Owner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate2, Owner);
                fireRateBuff = -1;

                ETGModConsole.Log(playerHPMSG, false);
               // if (reloadBuff <= 0) return;
                float curReload2 = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
                float newReload2 = curReload2 - reloadBuff;
                Owner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newReload2, Owner);
                reloadBuff = -1;
            }
            

            float curFireRate = Owner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float newFireRate = curFireRate + ((1 - playerHP)*2);
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, Owner);
            fireRateBuff = newFireRate - curFireRate;


            float curReload = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
            float newReload = curReload - ((1 - playerHP) / 4);
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload, Owner);
            reloadBuff = newReload - curReload;
        }
        float playerHP = 0;
        bool statsChanged = false;
        float fireRateBuff = -1;
        float reloadBuff = -1;
    }

}
