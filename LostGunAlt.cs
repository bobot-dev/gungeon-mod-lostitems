using System;
using Brave.BulletScript;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace LostItems
{
	// Token: 0x02000020 RID: 32
	public class LostGunAlt : GunBehaviour
	{

		private PlayerController player;
		GameUIAmmoType CustomAmmo;

		public string customAmmoType;

		ProjectileModule module;
		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Lost Sidearm", "locrtfsf_alt");
			Game.Items.Rename("outdated_gun_mods:lost_sidearm", "bot:lost_gun_alt");
			gun.gameObject.AddComponent<LostGunAlt>();
			gun.SetShortDescription("No longer lost");
			gun.SetLongDescription("A weapons once belonging by a strange treveler.");
			gun.SetupSprite(null, "locrtfsf_alt_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(32) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.2f;
		//	gun.barrelOffset.transform.localPosition += new Vector3(1f, 0f, 0f);
			gun.DefaultModule.cooldownTime = 0.25f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.SetBaseMaxAmmo(0);
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;

			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			//gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;

			//gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			//gun.DefaultModule.customAmmoType = 

			
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;



			//gun.DefaultModule.customAmmoType = "locrtfsf_alt_idle_001";

			//gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad2";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 6f;
			
			//projectile.baseData.speed *= 0.7f;
			//projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);

			gunId = gun.PickupObjectId;




			//gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			//gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			//gun.DefaultModule.customAmmoType = 



		//	gun.DefaultModule.customAmmoType = "locrtfsf_idle_001";
			//Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;
			//gun.DefaultModule.customAmmoType = gun3.CustomAmmoType;
			
			//gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";


		}
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
			AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", base.gameObject);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006644 File Offset: 0x00004844
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("WPN_icebreaker_reload_01", base.gameObject);
				
			}
		}



		public static int gunId;

		public static bool useAlt = true;

		private bool HasReloaded;




		//		Gun gunAlt;

		internal class LostGunProjectile : MonoBehaviour
		{

			public void Start()
			{
				this.projectile = base.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				Projectile projectile = this.projectile;
				//this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("locrtfsf_projectile_001");

			}

			private Projectile projectile;

			private PlayerController player;

		}
	}
}
