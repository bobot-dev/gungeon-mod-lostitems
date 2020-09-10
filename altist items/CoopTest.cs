using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using HutongGames.PlayMaker.Actions;

namespace LostItems
{
	public class CoopTest : PassiveItem
	{

		public static void Init()
		{
			//The name of the item
			string itemName = "Loneliness";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/sprites/lonliness";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<CoopTest>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "Show 'em Who's Boss";
			string longDesc = "Greatly increases damage dealt to bosses.\n\n" +
				"This item was created by a union of Gungeoneers who became fed up with low wages and poor benefits.\n" +
				"Viva la Revolverlucion!";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

			//Adds the actual passive effect to the item

			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.PlaceItemInAmmonomiconAfterItemById(326);
			// Token: 0x0600717A RID: 29050 RVA: 0x002B79A5 File Offset: 0x002B5BA5
			// Token: 0x04007134 RID: 28980
			//public List<StatModifier> modifiers;
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		protected override void Update()
		{
			bool flag = GameManager.Instance.PrimaryPlayer.healthHaver.IsAlive;
			if (flag)
			{
				ETGModConsole.Log(":)", false);

			}
			if (!flag)
			{
				ETGModConsole.Log(":(", false);

			}
			base.Update();
		}
	}
}