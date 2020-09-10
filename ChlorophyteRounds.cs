using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;


//demonetization
namespace LostItems
{
    class ChlorophyteRounds : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Chlorophyte Rounds";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/sprites/Chlorophyte Rounds";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ChlorophyteRounds>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Chases after your enemy";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item
          //  ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
            item.CanBeDropped = false;
            item.PlaceItemInAmmonomiconAfterItemById(531);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            homingRadius = 64f;
            //homingAngularVelocity = 360f;
            homingAngularVelocity = 1400f;
            player.PostProcessProjectile += this.PostProcessProjectile;
            this.TintColor = Color.green;
            this.TintPriority = 1;
            
        }

        private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
        {
            HomingModifier homingModifier = obj.gameObject.GetComponent<HomingModifier>();
            if (homingModifier == null)
            {
                homingModifier = obj.gameObject.AddComponent<HomingModifier>();
                homingModifier.HomingRadius = 0f;
                homingModifier.AngularVelocity = 0f;
            }
            float num = 1f;
            homingModifier.HomingRadius += this.homingRadius * num;
            homingModifier.AngularVelocity += this.homingAngularVelocity * num;

            obj.AdjustPlayerProjectileTint(this.TintColor, this.TintPriority, 0f);

        }

        public float homingRadius;
        public float homingAngularVelocity;
        public Color TintColor;
        public int TintPriority;
    }
}
