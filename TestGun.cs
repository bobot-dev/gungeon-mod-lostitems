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
	public class TestGun : GunBehaviour
	{



		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("test gun", "syntest");
			Game.Items.Rename("outdated_gun_mods:test_gun", "bot:gun_test");
			gun.gameObject.AddComponent<TestGun>();
			gun.SetShortDescription("test");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "syntest_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(812) as Gun, true, false);
			Gun gun3 = PickupObjectDatabase.GetById(383) as Gun;

			//gun.alternateVolley = gun3.Volley;


			
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			//gun.DefaultModule.customAmmoType = "hammer";

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
			gun.IsUndertaleGun = true;

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

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			//projectile.baseData.damage *= 1.2f;
			//projectile.baseData.speed *= 0.7f;
		///	projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);

			gunId = gun.PickupObjectId;

			gun.PlaceItemInAmmonomiconAfterItemById(88);
			gun.RemovePeskyQuestionmark();

		}


		//public static GunBehaviour lGun = gun;
		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
			base.PostProcessProjectile(projectile);
		}

		public override void OnInitializedWithOwner(GameActor actor)
		{
			Gun boneGun = PickupObjectDatabase.GetById(812) as Gun;
			gun.DefaultModule.projectiles[0] = boneGun.DefaultModule.projectiles[0];
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
			isBeam = false;
			base.OnInitializedWithOwner(actor);
		}

		private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
		{
			if(fatal == true && isBeam == true)
			{
				homingRadius = 16f;
				homingAngularVelocity = 400;

				Gun Seed = PickupObjectDatabase.GetById(812) as Gun;
				sourceProjectile = Seed.DefaultModule.projectiles[0];

				PlayerController player = gun.CurrentOwner as PlayerController;

				int count = 1;

				foreach (Vector2 number in pos)
				{
					GameObject gameObject = SpawnManager.SpawnProjectile(sourceProjectile.gameObject, hitRigidbody.sprite.WorldCenter + pos[count], rotation[count], true);
					Projectile component = gameObject.GetComponent<Projectile>();
					component.Owner = player;
					component.Shooter = player.specRigidbody;

					hitRigidbody.aiActor.IsWorthShootingAt = false;
					HomingModifier homingModifier = component.gameObject.GetComponent<HomingModifier>();
					if (homingModifier == null)
					{
						homingModifier = component.gameObject.AddComponent<HomingModifier>();
						homingModifier.HomingRadius = 0f;
						homingModifier.AngularVelocity = 0f;
						
					}
					float num = 1f;
					homingModifier.HomingRadius += this.homingRadius * num;
					homingModifier.AngularVelocity += this.homingAngularVelocity * num;

					count++;
				}

			}

		}

		List<Vector2> pos = new List<Vector2>
		{
			new Vector2(2, 0),
			new Vector2(1f, -1f),
			new Vector2(0, -2),
			new Vector2(-1f, -1f),
			new Vector2(-2, 0),
			new Vector2(-1f, 1f),
			new Vector2(0, 2),
			new Vector2(1f, 1f)
		};

		List<Quaternion> rotation = new List<Quaternion>
		{
			Quaternion.Euler(0, 0, 0),
			Quaternion.Euler(0, 0, 45),
			Quaternion.Euler(0, 0, 90),
			Quaternion.Euler(0, 0, 135),
			Quaternion.Euler(0, 0, 180),
			Quaternion.Euler(0, 0, 225),
			Quaternion.Euler(0, 0, 270),
			Quaternion.Euler(0, 0, 315)
		};

		public float homingRadius;
		public float homingAngularVelocity;


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

		public override void OnDropped()
		{
			base.OnDropped();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006644 File Offset: 0x00004844
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				
				Gun beamGun = PickupObjectDatabase.GetById(383) as Gun;
				Gun boneGun = PickupObjectDatabase.GetById(812) as Gun;
				Gun clipSprite = PickupObjectDatabase.GetById(760) as Gun;
				if (isBeam == true)
				{
					gun.DefaultModule.projectiles[0] = boneGun.DefaultModule.projectiles[0];
					gun.DefaultModule.numberOfShotsInClip = 10;
					gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
					isBeam = false;
				} 
				else
				{
					gun.DefaultModule.projectiles[0] = beamGun.DefaultModule.projectiles[0];
					gun.DefaultModule.numberOfShotsInClip = 1;
					gun.DefaultModule.projectiles[0].baseData.damage *= 2;
					gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
					gun.DefaultModule.customAmmoType = clipSprite.DefaultModule.customAmmoType;
					isBeam = true;
				}

				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
			}
		}

		bool isBeam = false;

		public static int gunId;

		private bool HasReloaded;



	}
}
