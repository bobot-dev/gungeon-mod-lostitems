using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionFace : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Scarier Face";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Scarier_Face2-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionFace>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Less enemy hp";
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
            percentageHealthReduction = 0.2f;
            AIActor.HealthModifier *= Mathf.Clamp01(1f - this.percentageHealthReduction);
        }
        public float percentageHealthReduction;
    }
}
