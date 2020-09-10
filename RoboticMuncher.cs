using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using HutongGames.PlayMaker.Actions;


namespace LostItems
{
	// Token: 0x0200002E RID: 46
	internal class RoboticMuncher : PlayerItem
	{

		// Token: 0x060000FC RID: 252 RVA: 0x00008F18 File Offset: 0x00007118
		public static void Init()
		{
			string itemName = "Robotic Muncher";
			string resourceName = "LostItems/sprites/robot_muncher";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<RoboticMuncher>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Be carful it might be radioactive";
			string longDesc = "This little guy will eat unwanted guns for you and give you some ammo. If you're more like it maybe some other helpful items. if you give it higher quality food it might give you something to help you live a bit longer. \n\n This muncher seems to use tech from another world...";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0f);
			item.consumable = false;
			item.quality = PickupObject.ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(403);
		}


		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_CHR_muncher_eat_01", base.gameObject);
			bool flag = user.CurrentGun.CanActuallyBeDropped(user);
			if (flag)
			{
				Gun currentGun = user.CurrentGun;
				PickupObject.ItemQuality quality = currentGun.quality;
				switch (quality)
				{
					case PickupObject.ItemQuality.D:
						user.inventory.DestroyCurrentGun();
						this.MuncherSpawn();
						break;

					case PickupObject.ItemQuality.C:
						user.inventory.DestroyCurrentGun();
						this.MuncherSpawn();
						break;

					case PickupObject.ItemQuality.B:
						user.inventory.DestroyCurrentGun();
						this.MuncherSpawn();
						break;

					case PickupObject.ItemQuality.A:
						user.inventory.DestroyCurrentGun();
						LootEngine.SpawnItem(ETGMod.Databases.Items["Junk Heart"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;

					case PickupObject.ItemQuality.S:
						user.inventory.DestroyCurrentGun();
						LootEngine.SpawnItem(ETGMod.Databases.Items["Junk Heart"].gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;

					default:
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(127).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;

				}
			}
		}

		private void MuncherSpawn() 
		{
			PlayableCharacters characterIdentity = this.LastOwner.characterIdentity;
			bool flag = characterIdentity == PlayableCharacters.Robot;
			if (flag)
			{
				int rngItemSpawnR = UnityEngine.Random.Range(1, 5);
				switch(rngItemSpawnR)
				{
					case 1:
						//armour
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;
					case 2:
						//ammo
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;
					case 3:
						//junk
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(127).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;
					default:
						//red ammo
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
						break;
				}
			}
			else
			{
				if (this.LastOwner.healthHaver.GetCurrentHealthPercentage() >= 1f)
				{
					int rngAmmoSpawn = UnityEngine.Random.Range(1, 3);
					switch (rngAmmoSpawn)
					{
						case 1:
							//ammo
							LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
							break;
						default:
							//red ammo
							LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
							break;
					}
				}
				else
				{
					int rngHeartSpawn = UnityEngine.Random.Range(1, 3);
					switch (rngHeartSpawn)
					{
						case 1:
							//heart
							LootEngine.SpawnItem(PickupObjectDatabase.GetById(85).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
							break;
						default:
							//half heart
							LootEngine.SpawnItem(PickupObjectDatabase.GetById(73).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
							break;
					}
				}
			}
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentGun.CanActuallyBeDropped(user);
		}
	}
}