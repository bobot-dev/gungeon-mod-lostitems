using System.Collections.Generic;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace LostItems
{
    class Shine : PlayerItem
    {
       
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Shine";
            
            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "LostItems/sprites/Shine"; 

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Shine>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "wip";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot"); 

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 300);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            //StunDuration = 5f;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float speedBuff = -1;
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_OBJ_ironcoin_flip_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
            

        }

        
        private void StartEffect(PlayerController user)
        {
           
            float stunChance = Random.Range(1, 5);
            if (stunChance >= 2)
            {
                stuned = true;
                RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
                List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null)
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        this.AffectEnemy(activeEnemies[i]);
                    }
                }
            }
            else {
                stuned = false;
                
                float curSpeed = user.stats.GetBaseStatValue(PlayerStats.StatType.MovementSpeed);
                float newSpeed = curSpeed - 1f;
                user.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, newSpeed, user);
                speedBuff = newSpeed - curSpeed;
            }
        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
            if (stuned == false) { 
                if (speedBuff <= 0) return;
                float curSpeed = user.stats.GetBaseStatValue(PlayerStats.StatType.MovementSpeed);
                float newSpeed = curSpeed - speedBuff;
                user.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, newSpeed, user);
                speedBuff = -1;
            }
        }
        protected void AffectEnemy(AIActor target)
        {
            StunDuration = 5f;
            if (target && target.behaviorSpeculator)
            {
                target.behaviorSpeculator.Stun(this.StunDuration, true);
            }
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }

        
        float StunDuration;
        bool stuned;

    }
}
