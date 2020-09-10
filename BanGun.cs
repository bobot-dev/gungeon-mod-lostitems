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
	public class BanGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Ban Gun", "fhvnrm");
			Game.Items.Rename("outdated_gun_mods:ban_gun", "bot:ban_gun");
			gun.gameObject.AddComponent<BanGun>();
			gun.SetShortDescription("Hax");
			gun.SetLongDescription("A great weapon for abusive moderators");
			gun.SetupSprite(null, "fhvnrm_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(519) as Gun, true, false);
			//gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(519) as Gun, true, false)

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 1f;
			gun.DefaultModule.cooldownTime = 0.3f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.SetBaseMaxAmmo(50);
			gun.quality = PickupObject.ItemQuality.S;
			gun.encounterTrackable.EncounterGuid = "change this for different guns, so the game doesn't think they are the same gun1";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.RemovePeskyQuestionmark();
			gun.PlaceItemInAmmonomiconAfterItemById(480);
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
			projectile.baseData.damage *= 20f;
			projectile.baseData.speed *= 1f;

			this.gun.DefaultModule.ammoCost = 1;
			base.PostProcessProjectile(projectile);
			projectile.gameObject.AddComponent<BanGunProjectile>();
		}
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_CombineRifle_Shot_01", base.gameObject);
		}
	}
}
