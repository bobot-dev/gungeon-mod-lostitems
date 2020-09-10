using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using HutongGames.PlayMaker.Actions;

namespace LostItems
{
    class Revenge : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Revenge";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "LostItems/sprites/altist_active";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Revenge>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Creeper, Aww Man";
            string longDesc = "WIP";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 300);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.


            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.SPECIAL;
            item.PlaceItemInAmmonomiconAfterItemById(353);
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        
        float duration = 30f;
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
            //user.healthHaver.NextShotKills = true;
            ETGModConsole.Log("start");
            RagePassiveItem rageitem = PickupObjectDatabase.GetById(353).GetComponent<RagePassiveItem>();
            RageOverheadVFX = rageitem.OverheadVFX.gameObject;
            user.PlayEffectOnActor(this.RageOverheadVFX, Vector3.zero, true, true, false);
        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
            //user.healthHaver.NextShotKills = false;
            ETGModConsole.Log("end");
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            //return base.CanBeUsed(user);

            if (GameManager.Instance.PrimaryPlayer.healthHaver.IsAlive)
            {
                return !base.CanBeUsed(user);
            } 
            else
            {
                return base.CanBeUsed(user);
            }
            
        }
        public GameObject RageOverheadVFX;
    }
}
