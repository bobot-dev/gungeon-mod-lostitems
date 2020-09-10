using System;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Collections.Generic;
using MultiplayerBasicExample;
using System.Reflection;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityChicken : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Thrown Weapon";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Yv_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityChicken>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "ability of YV";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.5f);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}
		protected override void DoEffect(PlayerController user)
		{
			bool flag = user.CurrentGun != null && !user.CurrentGun.InfiniteAmmo;
			if (flag)
			{
				user.CurrentGun.PrepGunForThrow();
				typeof(Gun).GetField("m_prepThrowTime", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(user.CurrentGun, 999);
				user.CurrentGun.CeaseAttack(true, null);
			}
		}

	}
}

