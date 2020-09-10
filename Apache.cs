using System;
using Brave.BulletScript;
using Gungeon;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace LostItems
{
	// Token: 0x02000020 RID: 32
	public class Apache : GunBehaviour
	{

		private PlayerController player;
		GameUIAmmoType CustomAmmo;

		public string customAmmoType;

		ProjectileModule module;
		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Apache Thunder", "apache");
			Game.Items.Rename("outdated_gun_mods:apache_thunder", "bot:apache");
			gun.gameObject.AddComponent<Apache>();
			gun.SetShortDescription("apache");
			gun.SetLongDescription("A weapons once belonging by a strange treveler.");
			gun.SetupSprite(null, "apache_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(464) as Gun, true, false);



			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 2f;

			gun.DefaultModule.cooldownTime = 0.25f;
			//gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.SetBaseMaxAmmo(100);
			gun.gunHandedness = GunHandedness.OneHanded;

			gun.quality = PickupObject.ItemQuality.SPECIAL;
			Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;


			//gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;

			//gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			//gun.DefaultModule.customAmmoType = 

			gun.barrelOffset.transform.localPosition += new Vector3(3f, 3f, 0f);
			//gun.muzzleFlashEffects = gun2.muzzleFlashEffects;

			Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;
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
			projectile.baseData.damage *= 10f;
			//projectile.baseData.speed *= 0.7f;
			projectile.SetProjectileSpriteRight("apache_projectile_001", 64, 64);



			//gun.DefaultModule.customAmmoType = "locrtfsf_idle_001";

			gun.encounterTrackable.EncounterGuid = "why did i make this whats wrong with me";
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			gunId = gun.PickupObjectId;

			gun.ForcedPositionInAmmonomicon = 1000000;
			//gun.RemovePeskyQuestionmark();

		}

		//public static GunBehaviour lGun = gun;


		// Token: 0x060000A1 RID: 161 RVA: 0x00006510 File Offset: 0x00004710
		protected void Update()
		{
			PlayerController player = gun.CurrentOwner as PlayerController;

			if (gun.CurrentOwner)
			{
				if (player.HasPickupID(39) || player.HasPickupID(19) || player.HasPickupID(129) || player.HasPickupID(108))
				{
					Gun rpg = PickupObjectDatabase.GetById(39).GetComponent<Gun>();
					//tools.CopyExplosionData(rpg.DefaultModule.ex);
				}

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

		public static int gunId;

		public static bool useAlt = true;

		private bool HasReloaded;
		public static ActiveAmmunitionData ammoData;
	}
}




/*
using Gungeon;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;
using System.Collections;
using ItemAPI;
using System;
using Dungeonator;
using MultiplayerBasicExample;

namespace LostItems
{
	// Token: 0x02000008 RID: 8
	public class LostGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("lost gun", "locrtfsf");
			Game.Items.Rename("outdated_gun_mods:lost_gun", "bot:lost_gun");
			gun.gameObject.AddComponent<LostGun>();
			gun.SetShortDescription("No longer lost");
			gun.SetLongDescription("wip");
			gun.SetupSprite(null, "locrtfsf_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(57) as Gun, true, false);
			//GunExt.AddProjectileModuleFrom(gun, "blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.5f;
			gun.DefaultModule.cooldownTime = 0.3f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.SetBaseMaxAmmo(1000);
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		protected void Update()
		{
			bool flag = this.gun.CurrentOwner;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = !this.gun.PreventNormalFireAudio;
				bool flag4 = flag3;
				if (flag4)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag5 = !this.gun.IsReloading && !this.HasReloaded;
				bool flag6 = flag5;
				if (flag6)
				{
					this.HasReloaded = true;
				}
			}
		}
		// Token: 0x06000033 RID: 51 RVA: 0x000035DC File Offset: 0x000017DC
		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x == null;
			bool flag2 = flag;
			if (flag2)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			projectile.baseData.damage *= 1f;
			projectile.baseData.speed *= 1f;
			this.gun.DefaultModule.ammoCost = 1;
			base.PostProcessProjectile(projectile);
			projectile.gameObject.AddComponent<LostGunProjectile>();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003673 File Offset: 0x00001873
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
		}
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			bool flag2 = flag;
			if (flag2)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_spellactionrevolver_reload_01", base.gameObject);
			}
		}

		private bool HasReloaded;

		internal class LostGunProjectile : MonoBehaviour
		{

			public void Start()
			{
				this.projectile = base.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				Projectile proj = this.projectile;
				//This determines what sprite you want your projectile to use.
				//projectile.sprite.GetSpriteIdByName("locrtfsf_projectile_001");
				this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("locrtfsf_projectile_001");
			}


			private Projectile projectile;

			private PlayerController player;
		}
	}
}
*/
