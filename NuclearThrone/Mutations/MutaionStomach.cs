using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionStomach : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Second Stomach";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Eyes_is_pregnant-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionStomach>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "More HP from medkits";
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
			this.HealingImprovedBy = 2f;
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyHealing = (Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>)Delegate.Combine(healthHaver.ModifyHealing, new Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>(this.ModifyIncomingHealing));
			base.Pickup(player);
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x002D920C File Offset: 0x002D740C
		private void ModifyIncomingHealing(HealthHaver source, HealthHaver.ModifyHealingEventArgs args)
		{
			HealingReceivedModificationItem needle = PickupObjectDatabase.GetById(259).GetComponent<HealingReceivedModificationItem>();
			OnImprovedHealingVFX = needle.OnImprovedHealingVFX.gameObject;

			if (args == EventArgs.Empty)
			{
				return;
			}
			if (this.OnImprovedHealingVFX != null)
			{
				source.GetComponent<PlayerController>().PlayEffectOnActor(this.OnImprovedHealingVFX, Vector3.zero, true, false, false);
			}
			args.ModifiedHealing *= this.HealingImprovedBy;
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x002D9274 File Offset: 0x002D7474
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyHealing = (Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>)Delegate.Remove(healthHaver.ModifyHealing, new Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>(this.ModifyIncomingHealing));
			//debrisObject.GetComponent<HealingReceivedModificationItem>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x002D92BD File Offset: 0x002D74BD
		protected override void OnDestroy()
		{
			if (this.m_pickedUp)
			{
				HealthHaver healthHaver = this.m_owner.healthHaver;
				healthHaver.ModifyHealing = (Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>)Delegate.Combine(healthHaver.ModifyHealing, new Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>(this.ModifyIncomingHealing));
			}
			base.OnDestroy();
		}

		// Token: 0x04007697 RID: 30359
		public float HealingImprovedBy;

		// Token: 0x04007698 RID: 30360
		public GameObject OnImprovedHealingVFX;
	}
}

