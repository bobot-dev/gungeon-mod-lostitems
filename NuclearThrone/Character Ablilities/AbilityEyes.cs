using System;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Collections.Generic;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityEyes : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Telekinesis";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Eyes_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityEyes>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "ability of eyes";
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
		public override void Pickup(PlayerController owner)
		{
			this.blackHole = new BlackHoleDoer();
			blackHole.affectsBullets = false;
			blackHole.affectsDebris = true;
			blackHole.affectsEnemies = true;
			blackHole.affectsPlayer = false;

			PlayerController player = owner;
			base.Pickup(player);

		}

		protected override void DoEffect(PlayerController player)
		{

			StartEffect(player);
			StartCoroutine(ItemBuilder.HandleDuration(this, duration, player, EndEffect));
			//base.StartCoroutine(this.HandleShield(player));
		}

		private void StartEffect(PlayerController user)
		{
			blackHoleObjectToSpawn = blackHole.gameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(blackHoleObjectToSpawn, user.CenterPosition, Quaternion.identity);

		}

		private void EndEffect(PlayerController user)
		{

		}

		public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}

		// Token: 0x0400007E RID: 126
		private float duration = 3f;
		BlackHoleDoer blackHole;
		public GameObject blackHoleObjectToSpawn;

	}

}
