using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace ItemAPI
{
    public class JunkHeart : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Junk Heart"; 

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/sprites/junk_heart"; 

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<JunkHeart>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "At least someone has a use for it";
            string longDesc = "A heart produced by the Robotic Muncher when fed high quality weapons.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.CanBeDropped = false;
        }
        public override void Pickup(PlayerController player)
        {
            PlayableCharacters characterIdentity = player.characterIdentity;
            bool flag = characterIdentity == PlayableCharacters.Robot;
            if (flag)
            {
                player.healthHaver.Armor += 1f;
            }
            base.Pickup(player);
        }
    }
}
