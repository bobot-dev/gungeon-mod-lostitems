using System;
using System.Runtime.CompilerServices;
using Brave.BulletScript;
using Gungeon;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace LostItems
{
	// Token: 0x02000020 RID: 32
	public class LostGun : GunBehaviour
	{


		ProjectileModule module;
		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			try
			{
				Gun gun = ETGMod.Databases.Items.NewGun("Lost Sidearm", "locrtfsf");
				Game.Items.Rename("outdated_gun_mods:lost_sidearm", "bot:lost_gun");
				gun.gameObject.AddComponent<LostGun>();
				gun.SetShortDescription("No longer lost");
				gun.SetLongDescription("A weapons once belonging by a strange treveler.");
				gun.SetupSprite(null, "locrtfsf_idle_001", 8);
				gun.SetAnimationFPS(gun.shootAnimation, 24);
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(88) as Gun, true, false);

				gun.DefaultModule.ammoCost = 1;
				gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				gun.reloadTime = 1.2f;

				gun.DefaultModule.cooldownTime = 0.25f;
				gun.InfiniteAmmo = true;
				gun.DefaultModule.numberOfShotsInClip = 7;
				gun.SetBaseMaxAmmo(0);
				//gun.gunHandedness = GunHandedness.OneHanded;
				gun.quality = PickupObject.ItemQuality.SPECIAL;
				Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;
				//Gun gun3 = PickupObjectDatabase.GetById(145) as Gun;

				gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
				//ammoData = new ActiveAmmunitionData();

				//gun.RegisterNewCustomAmmunition(ammoData);
				gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
				gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
				//Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;
				//gun.DefaultModule.customAmmoType = gun3.CustomAmmoType;
				//Guid.NewGuid().ToString();

				Guid.NewGuid().ToString();

				//Guid.NewGuid().ToString();

				gun.barrelOffset.transform.localPosition += new Vector3(0f, 0f, 0f);

				

				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				gun.DefaultModule.projectiles[0] = projectile;
				projectile.transform.parent = gun.barrelOffset;
				//projectile.transform.parent = gun.barrelOffset;
				projectile.baseData.damage = 6f;


				projectile.shouldRotate = true;
				//projectile.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
				//projectile.baseData.speed *= 0.7f;
				projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7,7,false);
				//,tk2dBaseSprite.Anchor.LowerLeft,true,7,7,0,0

				gunId = gun.PickupObjectId;

				gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";
				ETGMod.Databases.Items.Add(gun, null, "ANY");

				gun.PlaceItemInAmmonomiconAfterItemById(88);
				gun.RemovePeskyQuestionmark();
			}
			catch (Exception arg)
			{
				LostItemsMod.Log(string.Format("lost gun is broken coz: ", arg), LostItemsMod.TEXT_COLOR_BAD);
			}
			

		}


		//public static GunBehaviour lGun = gun;





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
