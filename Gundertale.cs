using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Brave.BulletScript;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace LostItems
{
		// Token: 0x02000020 RID: 32
	public class Gundertale :GunBehaviour
	{



		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Gundertale", "syntest");
			Game.Items.Rename("outdated_gun_mods:gundertale", "bot:bgundertale");
			gun.gameObject.AddComponent<Gundertale>();
			gun.SetShortDescription("sort");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "syntest_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(812) as Gun, true, false);
			Gun gun3 = PickupObjectDatabase.GetById(383) as Gun;

			//gun.alternateVolley = gun3.Volley;

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.2f;

			gun.DefaultModule.cooldownTime = 0.25f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.SetBaseMaxAmmo(0);
			gun.gunHandedness = GunHandedness.OneHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;
			//gun.IsUndertaleGun = true;

			//gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;

			//gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
			//gun.DefaultModule.customAmmoType = 

			gun.DefaultModule.angleVariance = 0f;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;



			//gun.DefaultModule.customAmmoType = "locrtfsf_idle_001";
			//Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;

			//gun.DefaultModule.customAmmoType = gun3.CustomAmmoType;
			Guid.NewGuid().ToString();
			//gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			projectile.gameObject.AddComponent<KthuliberProjectileController>();
			

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			gun.IsUndertaleGun = true;
			gun.encounterTrackable.EncounterGuid = "sands the funni";
			//projectile.baseData.damage *= 1.2f;
			//projectile.baseData.speed *= 0.7f;
			///	projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);



			gun.PlaceItemInAmmonomiconAfterItemById(88);
			gun.RemovePeskyQuestionmark();

		}


	}
}
