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
	public class CustomGun : GunBehaviour
	{



		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("custom gun", "syntest");
			Game.Items.Rename("outdated_gun_mods:custom_gun", "bot:custom_gun");
			gun.gameObject.AddComponent<CustomGun>();
			gun.SetShortDescription("test");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "syntest_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(GunConfigManager.Instance.configuration.ProjectileToCopy) as Gun, true, false);
			Gun gun3 = PickupObjectDatabase.GetById(383) as Gun;

			//gun.alternateVolley = gun3.Volley;

			gun.DefaultModule.ammoCost = GunConfigManager.Instance.configuration.AmmoCost;
			gun.DefaultModule.shootStyle = (PickupObjectDatabase.GetById(GunConfigManager.Instance.configuration.ProjectileToCopy) as Gun).DefaultModule.shootStyle;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = GunConfigManager.Instance.configuration.ReloadTime;

			gun.DefaultModule.cooldownTime = GunConfigManager.Instance.configuration.FireRate;
			gun.InfiniteAmmo = GunConfigManager.Instance.configuration.InfiniteAmmo;
			gun.DefaultModule.numberOfShotsInClip = GunConfigManager.Instance.configuration.ClipSize;
			gun.SetBaseMaxAmmo(GunConfigManager.Instance.configuration.MaxAmmo);
			gun.gunHandedness = GunHandedness.OneHanded;

			gun.quality = GunConfigManager.Instance.configuration.Quality;

			gun.DefaultModule.angleVariance = GunConfigManager.Instance.configuration.Spread;


			//gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
			Guid.NewGuid().ToString();
			//gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = GunConfigManager.Instance.configuration.Damage;
			//projectile.baseData.speed *= 0.7f;
			///	projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);

			gunId = gun.PickupObjectId;

			gun.PlaceItemInAmmonomiconAfterItemById(88);
			gun.RemovePeskyQuestionmark();

		}



		//public static GunBehaviour lGun = gun;
		public override void PostProcessProjectile(Projectile projectile)
		{
			//projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
			base.PostProcessProjectile(projectile);
		}


		// Token: 0x060000A1 RID: 161 RVA: 0x00006510 File Offset: 0x00004710
		protected void Update()
		{


			if (gun.CurrentOwner)
			{

				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
		}


		// Token: 0x060000A3 RID: 163 RVA: 0x00006629 File Offset: 0x00004829
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			if (gun.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam)
			{
				AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
			}

		}


		// Token: 0x060000A4 RID: 164 RVA: 0x00006644 File Offset: 0x00004844
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				GunConfigManager.Instance.UpdateConfiguration();
				//gcf.UpdateConfiguration();


				gun.DefaultModule.ammoCost = GunConfigManager.Instance.configuration.AmmoCost;
				gun.DefaultModule.projectiles[0] = (PickupObjectDatabase.GetById(GunConfigManager.Instance.configuration.ProjectileToCopy) as Gun).DefaultModule.projectiles[0];
				gun.DefaultModule.shootStyle = (PickupObjectDatabase.GetById(GunConfigManager.Instance.configuration.ProjectileToCopy) as Gun).DefaultModule.shootStyle;

				gun.reloadTime = GunConfigManager.Instance.configuration.ReloadTime;
				gun.DefaultModule.cooldownTime = GunConfigManager.Instance.configuration.FireRate;
				gun.InfiniteAmmo = GunConfigManager.Instance.configuration.InfiniteAmmo;
				gun.DefaultModule.numberOfShotsInClip = GunConfigManager.Instance.configuration.ClipSize;
				gun.SetBaseMaxAmmo(GunConfigManager.Instance.configuration.MaxAmmo);
				gun.quality = GunConfigManager.Instance.configuration.Quality;
				gun.DefaultModule.angleVariance = GunConfigManager.Instance.configuration.Spread;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
				projectile.baseData.damage = GunConfigManager.Instance.configuration.Damage;


				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
			}
		}

		public GunConfigManager gunConfig = new GunConfigManager();
	

		public static int gunId;

		private bool HasReloaded;



	}
}
