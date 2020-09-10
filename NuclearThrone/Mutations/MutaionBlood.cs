using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionBlood : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Blood Lust";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Bloodlust2-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionBlood>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Some kills regenerate HP";
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
            player.OnKilledEnemy += this.OnKill;
            
        }

        private void OnKill(PlayerController player)
        {

            healChance = UnityEngine.Random.Range(1, 25);
            //string healChanceMsg = healChance + "";
            //ETGModConsole.Log(healChanceMsg, false);
            if (healChance >= 20)
            {
                if (player.HasPickupID(ETGMod.Databases.Items["Second Stomach"].PickupObjectId))
                {
                    player.healthHaver.ApplyHealing(0.25f);
                } 
                else
                {
                    player.healthHaver.ApplyHealing(0.5f);
                }
                player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Healing_Sparkles_001") as GameObject, Vector3.zero, true, true, false);

            }
            
        }
        float healChance = 0;

    }
}
