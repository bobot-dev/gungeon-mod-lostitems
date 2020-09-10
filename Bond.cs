using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Data.Sql;

namespace LostItems
{
	public class Bond : PassiveItem
	{
		public static void Init()
		{
			//The name of the item
			string itemName = "Father and Son Bond";

			//Refers to an embedded png in the project. Make sure to embed your sprites! Google it
			string resourceName = "LostItems/sprites/bond";

			//Create new GameObject
			GameObject obj = new GameObject();

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<Bond>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "Bring it on devil";
			string longDesc = "The bond between and this father and his son is so strong, it removes the time needed to reload after an attack and breaks the limits of boss fights.";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

			//Adds the actual passive effect to the item
			item.CanBeDropped = false;
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ChargeAmountMultiplier, 100000, StatModifier.ModifyMethod.ADDITIVE);

			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			bool flag5 = base.Owner.inventory.ContainsGun(541);
			if (flag5 && synergyMsg == 0)
			{
				player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Synergy") as GameObject, Vector3.zero, true, false, false);
				AkSoundEngine.PostEvent("Play_OBJ_synergy_get_01", gameObject);
				string header = "True Bond";
				string text = "True Bond";
				this.Notify(header, text);
				synergyMsg = 1;
			}
			else
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetById(541).gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
			}
		}

		protected override void Update()
		{

			bool flag = base.Owner;
			if (flag)
			{
				bool flag2 = base.Owner.inventory.ContainsGun(541);
				if (flag2)
				{
					bool flag3 = !this.hasCasey;
					if (flag3)
					{
						this.hasCasey = true;
					}
					bool flag4 = base.Owner.CurrentGun.PickupObjectId.Equals(541) && !this.equippedCasey;
					if (flag4)
					{
						if (synergyMsg == 0)
						{

							Owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Synergy") as GameObject, Vector3.zero, true, false, false);

							AkSoundEngine.PostEvent("Play_OBJ_synergy_get_01", gameObject);
							string header = "True Bond";
							string text = "True Bond";
							this.Notify(header, text);

							synergyMsg = 1;
						}
						
						base.Owner.CurrentGun.reloadTime = 0;
						base.Owner.CurrentGun.CustomBossDamageModifier = 2;
						base.Owner.CurrentGun.UsesBossDamageModifier = false;
						


						this.equippedCasey = true;
					}
					else
					{
						this.equippedCasey = false;
					}
				}
				else
				{
					this.hasCasey = false;
					this.equippedCasey = false;
					synergyMsg = 0;
				}
			}
		}
		private bool hasCasey;

		private bool equippedCasey;

		int synergyMsg = 0;

		private void Notify(string header, string text)
		{
			//tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, true, true);
		}

		public override DebrisObject Drop(PlayerController player)
		{

			return base.Drop(player);
		}

	}
}
