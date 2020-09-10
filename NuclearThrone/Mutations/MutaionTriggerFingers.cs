using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionTriggerFingers : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Trigger Fingers";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Racing_Mind2-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionTriggerFingers>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Kills lower reload time";
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
            //player.GunChanged += this.RemoveEffect;
            player.OnKilledEnemy += this.OnKill;
            player.OnRoomClearEvent += this.RemoveEffect;
            player.CurrentGun.OnReloadPressed += this.OnReload;
            player.OnDidUnstealthyAction += this.RemoveEffect;
        }

        private void OnKill(PlayerController player)
        {

            if (statsChanged == true)
            {
                return;
            }

            if (HasReloaded == true)
            {
                RemoveEffect(player);

            }

            float curFireRate = Owner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float newFireRate = curFireRate + 2;
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, Owner);
            fireRateBuff = newFireRate - curFireRate;


            float curReload = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
            float newReload = curReload - 0.5f;
            Owner.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload, Owner);
            reloadBuff = newReload - curReload;
            EnableVFX(Owner);

            statsChanged = true;
        }

        public void RemoveEffect(PlayerController player)
        {
            if (statsChanged == true) { 
                DisableVFX(Owner);
               // if (fireRateBuff <= 0) return;
                float curFireRate = Owner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
                float newFireRate = curFireRate - fireRateBuff;
                Owner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, Owner);
                fireRateBuff = -1;

               // if (reloadBuff >= 0) return;
                float curReload = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
                float newReload = curReload - reloadBuff;
                Owner.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload, Owner);
                reloadBuff = -1;
                statsChanged = false;
                HasReloaded = false;
            }
            
        }

        public void OnReload(PlayerController player, Gun gun, bool bSOMETHING)
        {
            HasReloaded = true;
        }

        private void EnableVFX(PlayerController Owner)
        {
            //Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            //outlineMaterial.SetColor("_OverrideColor", new Color(76f, 252f, 252f));
            //Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            //outlineMaterial.SetColor("_OverrideColor", new Color(0.3f, 0.988f, 0.988f));
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 1f, 0f));
        }

        private void DisableVFX(PlayerController Owner)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }

        bool statsChanged = false;
        float fireRateBuff = -1;
        float reloadBuff = -1;
        bool HasReloaded = false;

    }
}
