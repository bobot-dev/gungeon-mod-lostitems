using System;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Collections.Generic;
using MultiplayerBasicExample;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityYV : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Pop Pop";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Yv_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityYV>();

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
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0f);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}
		public override void Pickup(PlayerController owner)
		{
			PlayerController player = owner;
			owner.PostProcessProjectile += this.HandleProjectileFired;
			base.Pickup(player);
			player.CurrentGun.OnReloadPressed += this.OnReload;

		}
		float reloadBuff = -1;
		protected override void DoEffect(PlayerController user)
		{
			//Play a sound effect
			

			//Activates the effect
			StartEffect(user);
		}

		private void StartEffect(PlayerController user)
		{
			Gun gun = user.CurrentGun;
			//user.healthHaver.NextShotKills = true;
			//Shoot();  

			switch(gun.DefaultModule.shootStyle)
			{

				case ProjectileModule.ShootStyle.Automatic:
				case ProjectileModule.ShootStyle.SemiAutomatic:
					gun.CurrentAmmo -= gun.DefaultModule.ammoCost;
					gun.ClipShotsRemaining -= 1;
					break; 

				case ProjectileModule.ShootStyle.Burst:
					gun.CurrentAmmo -= gun.DefaultModule.burstShotCount;
					gun.ClipShotsRemaining -= gun.DefaultModule.burstShotCount;
					break;

				case ProjectileModule.ShootStyle.Charged:
				case ProjectileModule.ShootStyle.Beam:
					return;

			}

			user.forceFireDown = true;
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);



		}

		private void HandleProjectileFired(Projectile firedProjectile, float arg2)
		{
			if (LastOwner.forceFireDown == true)
			{
				LastOwner.forceFireDown = false;
				GameObject gameObject = SpawnManager.SpawnProjectile(firedProjectile.gameObject, firedProjectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (LastOwner.CurrentGun == null) ? 0f : LastOwner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag3 = component != null;
				bool flag4 = flag3;
				if (flag4)
				{
					component.SetOwnerSafe(LastOwner, "Player");
					component.Shooter = LastOwner.specRigidbody;
				}
			}


			
			float curReload = LastOwner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
			float newReload = curReload / 2f;
			LastOwner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newReload, LastOwner);
			reloadBuff = newReload - curReload;

			float curReload2 = LastOwner.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
			float newReload2 = curReload2 - reloadBuff;
			LastOwner.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newReload2, LastOwner);

			reloadBuff = -1;
		}


		public void OnReload(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (LastOwner.forceFireDown == true)
			{
				LastOwner.forceFireDown = false;
			}
		}

		public override bool CanBeUsed(PlayerController user)
		{

			Gun gun = user.CurrentGun;
			if (gun.InfiniteAmmo == false)
			{
				if (gun.DefaultModule.shootStyle.Equals(ProjectileModule.ShootStyle.Beam) || gun.DefaultModule.shootStyle.Equals(ProjectileModule.ShootStyle.Charged))
				{
					return false;
				}
				else if (gun.IsReloading == false && gun.CurrentAmmo >= gun.DefaultModule.ammoCost * 2 && gun.ClipShotsRemaining >= 2)
				{
					if (gun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Burst)
					{
						if (gun.ClipShotsRemaining >= gun.DefaultModule.burstShotCount * 2 && gun.CurrentAmmo >= gun.DefaultModule.burstShotCount * 2)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}





		}


	}
}

