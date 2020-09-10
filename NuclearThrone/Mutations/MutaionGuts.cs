using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionGuts : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Gamma Guts";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Crystal_SMASH-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionGuts>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Corpses fly & hit harder";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 2f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }

		public override void Pickup(PlayerController player)
		{

            
            SpeculativeRigidbody specRigidbody = player.specRigidbody;
            specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));

            this.player = player;
            base.Pickup(player);

        }

        private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
        {

           

            AIActor aiActor = rigidbodyCollision.OtherRigidbody.aiActor;

            if (this.m_owner && rigidbodyCollision.OtherRigidbody && rigidbodyCollision.OtherRigidbody.aiActor)
            {
                if (aiActor.IsNormalEnemy && !aiActor.IsHarmlessEnemy)
                {
                    if (aiActor.healthHaver.GetCurrentHealth() <= 20f)
                    {
                        Owner.ReceivesTouchDamage = false;
                    }
                    else
                    {
                        Owner.ReceivesTouchDamage = true;
                    }
                    aiActor.healthHaver.ApplyDamage(20f, Owner.LastCommandedDirection, "Gamma Guts", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                   // player.ReceivesTouchDamage = true;
                }
            }
            
        }
        PlayerController player;
        HealthHaver enemy;
    }
}
