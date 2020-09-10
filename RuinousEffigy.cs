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
	public class RuinousEffigy : GunBehaviour
	{



		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Ruinous Effigy", "syntest");
			Game.Items.Rename("outdated_gun_mods:ruinous_effigy", "bot:ruinous_effigy");
			gun.gameObject.AddComponent<RuinousEffigy>();
			gun.SetShortDescription("test");
			gun.SetLongDescription("From many wings of ruin blows a wind that will reshape this dead world.");
			GunExt.SetupSprite(gun, null, "syntest_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);

			//Gun component = Game.Items["shellegun"].GetComponent<Gun>();
			

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(121) as Gun, true, false);
			//gun.DefaultModule.projectiles[0] = component.alternateVolley.projectiles[0].projectiles[0];
			Gun gun3 = PickupObjectDatabase.GetById(383) as Gun;

			//gun.alternateVolley = gun3.Volley;


			
			//gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			//gun.DefaultModule.customAmmoType = "hammer";

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 1.2f;

			gun.DefaultModule.cooldownTime = 0f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.SetBaseMaxAmmo(0);
			//gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;
			//gun.IsUndertaleGun = true;

			//gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;

			//gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			//gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
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
			//projectile.AppliesFire = false;
			var beam = projectile.gameObject.AddComponent<BasicBeamController>();
			//projectile.sprite.GetSpriteIdByName("ruinous_rffigy_beam_middle_001");
			//beam.sprite.GetSpriteIdByName("ruinous_rffigy_beam_middle_001");
			//beam.ContinueBeamArtToWall = true;
			beam.AdjustPlayerBeamTint(new Color(0.56470588235f, 0.01176470588f, 0.98823529411f), 0, 0);
			//projectile.DefaultTintColor = ;
			//projectile.HasDefaultTint = true;
			//projectile.SetBeamProjectileSpriteRight("ruinous_rffigy_beam_middle_001", 16, 16, null, null);
			//projectile.beam
			//projectile.baseData.speed *= 0.7f;
			///	projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);

			gunId = gun.PickupObjectId;

			gun.PlaceItemInAmmonomiconAfterItemById(563);
			gun.RemovePeskyQuestionmark();

		}


		//public static GunBehaviour lGun = gun;
		//public tick

		public override void PostProcessProjectile(Projectile projectile)
		{

			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
			base.PostProcessProjectile(projectile);
		}
		private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
		{
			if (fatal == true)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetById(595).gameObject, sourceProjectile.specRigidbody.UnitCenter, new Vector2(0,0), 1f, false, true, false);
			}
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

		public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile)
		{
			
			return base.OnPreFireProjectileModifier(gun, projectile);
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

		private bool HasReloaded;



	}
}
