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
	internal class AbilityRobot : PlayerItem
	{

		// Token: 0x060000FC RID: 252 RVA: 0x00008F18 File Offset: 0x00007118
		public static void Init()
		{
			string itemName = "Eat Weapon";
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Robot_icon1-export";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<AbilityRobot>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "ability of the robot";
			string longDesc = "";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}


		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_CHR_muncher_eat_01", base.gameObject);
			//bool flag = 
			if (user.inventory.AllGuns.Count() > 1)
			{
					user.inventory.DestroyCurrentGun();
					this.MuncherSpawn(user);
			}
		}

		private void MuncherSpawn(PlayerController player) 
		{  

			if (player.healthHaver.GetCurrentHealthPercentage() >= 1f)
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
				//heart
				LootEngine.SpawnItem(PickupObjectDatabase.GetById(85).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);

			}
			
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return user.inventory.AllGuns.Count() > 1;
		}
	}
}