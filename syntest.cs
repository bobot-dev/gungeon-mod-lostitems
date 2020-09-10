using Gungeon;
using UnityEngine;
using ItemAPI;
using System;
using System.Collections;

namespace LostItems
{
	// Token: 0x02000008 RID: 8
	public class syntest : GunBehaviour
	{

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Test Gun", "syntest");
			Game.Items.Rename("outdated_gun_mods:test_gun", "bot:test_gun-");
			gun.gameObject.AddComponent<syntest>();
			gun.SetShortDescription("wip");
			gun.SetLongDescription("wip");
			gun.SetupSprite(null, "syntest_idle_001", 8);

			Gun MakeshiftCannon = PickupObjectDatabase.GetById(180) as Gun;

			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 24);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 4);
			Gun gun2 = PickupObjectDatabase.GetById(762) as Gun;
			GunExt.AddProjectileModuleFrom(gun, "makeshift_cannon", true, false);
			gun.SetBaseMaxAmmo(444);
			RandomProjectileReplacementItem component = PickupObjectDatabase.GetById(524).GetComponent<RandomProjectileReplacementItem>();
			Projectile replacementProjectile = component.ReplacementProjectile;
			//Gun gun3 = MakeshiftCannon.PickupObjectDatabase.GetById(329) as Gun;

			gun.DefaultModule.usesOptionalFinalProjectile = MakeshiftCannon.DefaultModule.usesOptionalFinalProjectile;
			gun.DefaultModule.numberOfFinalProjectiles = MakeshiftCannon.DefaultModule.numberOfFinalProjectiles;
			gun.DefaultModule.finalProjectile = MakeshiftCannon.DefaultModule.finalProjectile;
			gun.DefaultModule.finalCustomAmmoType = MakeshiftCannon.DefaultModule.finalCustomAmmoType;
			gun.DefaultModule.finalAmmoType = MakeshiftCannon.DefaultModule.finalAmmoType;
			gun.DefaultModule.ammoCost = MakeshiftCannon.DefaultModule.ammoCost;
			gun.DefaultModule.shootStyle = MakeshiftCannon.DefaultModule.shootStyle;
			gun.damageModifier = MakeshiftCannon.damageModifier;
			gun.reloadTime = MakeshiftCannon.reloadTime;
			gun.DefaultModule.cooldownTime = MakeshiftCannon.DefaultModule.cooldownTime;
			gun.DefaultModule.numberOfShotsInClip = MakeshiftCannon.DefaultModule.numberOfShotsInClip;
			gun.DefaultModule.angleVariance = MakeshiftCannon.DefaultModule.angleVariance;
			gun.barrelOffset.transform.localPosition += MakeshiftCannon.barrelOffset.transform.localPosition;
			gun.quality = PickupObject.ItemQuality.EXCLUDED; 
			gun.encounterTrackable.EncounterGuid = "test test this is a test bla bla bla";
			gun.gunClass = MakeshiftCannon.gunClass;
			gun.CanBeDropped = MakeshiftCannon.CanBeDropped;
			//Gun gun4 = MakeshiftCannon.PickupObjectDatabase.GetById(519) as Gun;;
			gun.muzzleFlashEffects = MakeshiftCannon.muzzleFlashEffects;
			//Gun gun5 = MakeshiftCannon.PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = MakeshiftCannon.finalMuzzleFlashEffects;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = MakeshiftCannon.projectile;

		}

		// Token: 

	}
}
