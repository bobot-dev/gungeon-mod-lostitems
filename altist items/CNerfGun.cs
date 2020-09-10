using Gungeon;
using ItemAPI;
using UnityEngine;


namespace LostItems
{
	// Token: 0x02000008 RID: 8
	public class CNerfGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Nerf Gun", "cnerf_gun");
			Game.Items.Rename("outdated_gun_mods:nerf_gun", "bot:nerf_gun1");
			gun.gameObject.AddComponent<CNerfGun>();
			gun.SetShortDescription("wip");
			gun.SetLongDescription("wip");
			gun.SetupSprite(null, "cnerf_gun_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(24) as Gun, true, false);


			
			Gun gun2 = PickupObjectDatabase.GetById(24) as Gun;


			
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			






			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 5.0f;
			
			

			//GunExt.AddProjectileModuleFrom(gun, "blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1f;
			gun.DefaultModule.cooldownTime = 0.2f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 6;
			gun.SetBaseMaxAmmo(250);
			gun.quality = PickupObject.ItemQuality.D;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.encounterTrackable.EncounterGuid = "its nerf or nothing!!";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.RemovePeskyQuestionmark();
			gun.PlaceItemInAmmonomiconAfterItemById(24);
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
			AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006644 File Offset: 0x00004844
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
			}
		}

		//static LostGunProjectile lostProjectile = new LostGunProjectile();

		public static int gunId;

		public static bool useAlt = true;

		private bool HasReloaded;
	}
}
