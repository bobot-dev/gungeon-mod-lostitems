using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class BalencePatch : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "BalencePath";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/sprites/demonetization";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BalencePatch>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "wip";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        public override void Pickup(PlayerController player)
        {
            
            base.Pickup(player);
        }

        protected override void Update()
        {
            PlayerController lastOwner = this.Owner;
            base.Update();
            PickupObject.ItemQuality q = lastOwner.inventory.CurrentGun.quality;
            switch (lastOwner.inventory.CurrentGun.quality)
            {
                case PickupObject.ItemQuality.D:
                    lastOwner.CurrentGun.CustomBossDamageModifier = 1.5f;
                    break;
                case PickupObject.ItemQuality.C:
                    lastOwner.CurrentGun.CustomBossDamageModifier = 1.3f;
                    break;
                case PickupObject.ItemQuality.B:
                    lastOwner.CurrentGun.CustomBossDamageModifier = 1.1f;
                    break;
                //case PickupObject.ItemQuality.A:
                    //lastOwner.CurrentGun.CustomBossDamageModifier = 1.5f;
                    //break;
                case PickupObject.ItemQuality.S:
                    lastOwner.CurrentGun.CustomBossDamageModifier = -1.1f;
                    break;
                default:
                    lastOwner.CurrentGun.CustomBossDamageModifier = 0f;
                    break;
            }
        }
    }
}
