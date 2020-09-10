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
	public class RebelGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Rebel Ally Gun", "ally_gun");
			Game.Items.Rename("outdated_gun_mods:rebel_ally_gun", "bot_nt:rebel_gun");
			gun.gameObject.AddComponent<RebelGun>();
			gun.SetShortDescription("cheater");
			gun.SetLongDescription("you shouldn't have this so no descripion for you :P");
			gun.SetupSprite(null, "ally_gun_idle_001", 24);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(182) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.2f;
			gun.DefaultModule.cooldownTime = 0.25f;
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.InfiniteAmmo = true;
			gun.SetBaseMaxAmmo(1);
			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			Gun gun2 = PickupObjectDatabase.GetById(38) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.encounterTrackable.EncounterGuid = "the gun used by rebel's allies";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000038E0 File Offset: 0x00001AE0
		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x == null;
			if (flag)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			projectile.baseData.damage *= 0.75f;
			projectile.baseData.speed *= 1f;

			this.gun.DefaultModule.ammoCost = 1;
			base.PostProcessProjectile(projectile);
			//projectile.OnHitEnemy.
			projectile.gameObject.AddComponent<RebelGunProjectile>();

		}

		protected void Update()
		{


			bool flag = this.gun.CurrentOwner;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = !this.gun.PreventNormalFireAudio;
				bool flag5 = flag4;
				bool flag6 = flag5;
				if (flag6)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag7 = !this.gun.IsReloading && !this.HasReloaded;
				bool flag8 = flag7;
				bool flag9 = flag8;
				if (flag9)
				{
					this.HasReloaded = true;
				}
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
		}


		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);

			}
		}
		private bool HasReloaded;
	}

	internal class RebelGunProjectile : MonoBehaviour
	{

		public void Start()
		{
			this.projectile = base.GetComponent<Projectile>();
			this.player = (this.projectile.Owner as PlayerController);
			Projectile proj = this.projectile;
		}
		private Projectile projectile;
		private PlayerController player;
	}

}

