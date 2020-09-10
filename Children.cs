using System.Collections;
using ItemAPI;
using UnityEngine;

namespace LostItems
{
	public class Children : PlayerItem
	{

		public static void Init()
		{
			//The name of the item
			string itemName = "Reto's Son";
			string resourceName = "LostItems/sprites/reto_junkan";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<Children>();

			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "The Son of Reto";
			string longDesc = "This artifact upon activation summons the sons of Reto.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);
			item.consumable = true;
			item.quality = ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(580);
		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
			PlayerController player = this.LastOwner;
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(541).gameObject, player.specRigidbody.UnitCenter, Vector2.left, 1f, false, true, false);
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(580).gameObject, player.specRigidbody.UnitCenter, Vector2.right, 1f, false, true, false);
		}
	}
}
