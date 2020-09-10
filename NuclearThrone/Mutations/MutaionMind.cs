using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionMind : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Open Mind";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/OpenMind-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionMind>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "A chest spawns after killing a boss";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive   effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }
        public override void Pickup(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.handleDragunRewards));

            base.Pickup(player);

        }
        private void handleDragunRewards(float damage, bool fatal, HealthHaver enemy)
        {
            bool flag = enemy != null && enemy.aiActor != null && fatal;
            if (flag)
            {
                bool flag2 = enemy.aiActor.healthHaver.IsBoss == true;
                if (flag2)
                {
                    PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
                    Chest s_Chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(primaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
                    s_Chest.IsLocked = true;

                   // Chest.Spawn(s_Chest, );
                }
            }
        }



    }
}
