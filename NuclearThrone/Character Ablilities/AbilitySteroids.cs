using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;


namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilitySteroids : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Dual Wield";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Steroids_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilitySteroids>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "abiliy of steroids";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);

		}

		protected override void DoEffect(PlayerController player)
		{
			if (player.inventory.DualWielding == true)
			{
				player.inventory.SetDualWielding(false, "synergy");
			} else
			{

				
				//player.inventory.forc
				int indexForGun = this.GetIndexForGun(player, player.inventory.AllGuns[0].PickupObjectId);
				int indexForGun2 = this.GetIndexForGun(player, player.inventory.AllGuns[1].PickupObjectId);
				//player.inventory.du
				player.inventory.SwapDualGuns();

				player.inventory.SetDualWielding(true, "synergy");


				bool flag2 = indexForGun >= 0 && indexForGun2 >= 0;
				if (flag2)
				{
					while (player.inventory.CurrentGun.PickupObjectId != player.inventory.AllGuns[1].PickupObjectId)
					{
						player.inventory.ChangeGun(1, false);
					}
				}
				player.inventory.SwapDualGuns();
				bool flag3 = player.CurrentGun && !player.CurrentGun.gameObject.activeSelf;
				if (flag3)
				{
					player.CurrentGun.gameObject.SetActive(true);
				}
				bool flag4 = player.CurrentSecondaryGun && !player.CurrentSecondaryGun.gameObject.activeSelf;
				if (flag4)
				{
					player.CurrentSecondaryGun.gameObject.SetActive(true);
				}
				//player.GunChanged += this.HandleGunChanged;
			}

		}

		private int GetIndexForGun(PlayerController p, int gunID)
		{
			for (int i = 0; i < p.inventory.AllGuns.Count; i++)
			{
				bool flag = p.inventory.AllGuns[i].PickupObjectId == gunID;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

	public override bool CanBeUsed(PlayerController user)
		{
			if (user.inventory.AllGuns.Count > 1 && user.inventory.CurrentGun.PickupObjectId != user.inventory.AllGuns[1].PickupObjectId)
			{
				return true;
			} 
			else
			{
				return false;
			}
			//return base.CanBeUsed(user);
		}
	}

}
