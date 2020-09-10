using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace LostItems
{
    class LootBox : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Loot Box";
            
            //Refers to an embedded png in the project. Make sure to embed your sprites! Google it.
            string resourceName = "LostItems/sprites/Loot_Box"; 

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<LootBox>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "It's not illigal and kids love it";
            string longDesc = "This *NEW AND IMPROVED* Lament Configurum no longer gives curse because that could leed to the harm of a customer and then you couldn't give us money. It also only works once this isn't a charity.\n\n This great item contains very very good loot (but mostly klobbes).";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot"); 

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 200);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = true;
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(525);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 0f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        //Doubles the damage, makes the next shot kill the player, and stores the amount we buffed the player for later
        private void StartEffect(PlayerController user)
        {
            int gunRng = UnityEngine.Random.Range(1, 100);
            PlayerController player = this.LastOwner;
            switch (gunRng)
            {
                case 1:
                    PickupObject.ItemQuality itemQualityS = PickupObject.ItemQuality.S;
                    PickupObject itemOfTypeAndQualityS = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQualityS, GameManager.Instance.RewardManager.GunsLootTable, false);
                    LootEngine.SpawnItem(itemOfTypeAndQualityS.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    break;
                case 2:
                case 3:
                    PickupObject.ItemQuality itemQualityA = PickupObject.ItemQuality.A;
                    PickupObject itemOfTypeAndQualityA = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQualityA, GameManager.Instance.RewardManager.GunsLootTable, false);
                    LootEngine.SpawnItem(itemOfTypeAndQualityA.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    PickupObject.ItemQuality itemQualityB = PickupObject.ItemQuality.B;
                    PickupObject itemOfTypeAndQualityB = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQualityB, GameManager.Instance.RewardManager.GunsLootTable, false);
                    LootEngine.SpawnItem(itemOfTypeAndQualityB.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    PickupObject.ItemQuality itemQualityC = PickupObject.ItemQuality.C;
                    PickupObject itemOfTypeAndQualityC = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQualityC, GameManager.Instance.RewardManager.GunsLootTable, false);
                    LootEngine.SpawnItem(itemOfTypeAndQualityC.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                    PickupObject.ItemQuality itemQualityD = PickupObject.ItemQuality.D;
                    PickupObject itemOfTypeAndQualityD = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQualityD, GameManager.Instance.RewardManager.GunsLootTable, false);
                    LootEngine.SpawnItem(itemOfTypeAndQualityD.gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    break;
                default:
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(31).gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
                    break;

            }
        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {

        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
