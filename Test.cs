using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace LostItems
{
    class Test : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Test";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "LostItems/sprites/wip";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Test>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Spawns all items from this mod";
            string longDesc = "this item is mainly just here so i can test all my items quickly";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 0f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            //AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
            

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        //Doubles the damage, makes the next shot kill the player, and stores the amount we buffed the player for later
        private void StartEffect(PlayerController user)
        {
            useCount += 1;
            switch (useCount)
            {
                case 1:
                    AkSoundEngine.PostEvent("Play_WPN_kthulu_blast_01", base.gameObject);
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Lost Gun"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 2:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Sai"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 3:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Shadow Relic"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 4:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Cloak of the Lost"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 5:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Robotic Muncher"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 6:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Junk Heart"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 7:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Loot Box"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 8:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Nerf Gun"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 9:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Mop"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 10:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["CoopTest"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 11:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Father and Son Bond"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 12:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["bob"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 13:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Reto's Sons"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 14:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Blessed Orb"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 15:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Ban Gun"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 16:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Gungeonpop"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 17:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Gungeonpop Updated"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 18:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Mistake Charm"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                case 19:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Revenge"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    break;
                default:
                    LootEngine.SpawnItem(ETGMod.Databases.Items["Test"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
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
        float useCount = 0;
    }
}
