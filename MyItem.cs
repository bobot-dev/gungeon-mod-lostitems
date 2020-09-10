using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LostItems
{
	class MyItem : PassiveItem
	{
		public static void Init()
		{
			//The name of the item
			string itemName = "idk";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/sprites/wip";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<MyItem>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "abiliy of rebel";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot ally, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			//ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}

		public override void Pickup(PlayerController player)
		{


			base.Pickup(player);
			

			BlinkPassiveItem Scarf = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();
			this.ScarfPrefab = Scarf.ScarfPrefab;

			//ETGModConsole.Log( + "", false);
			Shader.Find("_OverrideColor");
			Color ScarfColor = new Color(0.18039215686f, 0f, 0.30196078431f);
			ScarfPrefab.ScarfMaterial.SetColor("_OverrideColor", ScarfColor);
			//ScarfPrefab.ScarfMaterial.SetColor("_Color", Color.black);
			ScarfPrefab.ScarfLength /= 2;
			//ScarfPrefab.ScarfMaterial.GetColor
			//ScarfPrefab.ScarfMaterial.mainTexture;
			//this.m_scarf = Scarf.m_scarf;

			if (this.ScarfPrefab)
			{
				this.m_scarf = UnityEngine.Object.Instantiate<GameObject>(this.ScarfPrefab.gameObject).GetComponent<ScarfAttachmentDoer>();
				this.m_scarf.Initialize(player);
			}


		}

		public ScarfAttachmentDoer ScarfPrefab;
		private ScarfAttachmentDoer m_scarf;
	}
		
}
