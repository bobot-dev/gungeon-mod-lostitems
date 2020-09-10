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
	public class Kunia : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Throwing Kunai", "kunai");
			Game.Items.Rename("outdated_gun_mods:throwing_kunai", "bot:kunai");
			gun.gameObject.AddComponent<Kunia>();
			gun.SetShortDescription("");
			gun.SetLongDescription("");
			gun.SetupSprite(null, "kunai_idle_001", 24);
			
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.SetAnimationFPS(gun.reloadAnimation, 1);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(8) as Gun, true, false);
			//gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(519) as Gun, true, false)

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 1.3f;
			gun.DefaultModule.cooldownTime = 0.5f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.SetBaseMaxAmmo(100);
			gun.InfiniteAmmo = true;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.encounterTrackable.EncounterGuid = "idea by sirwow";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.RemovePeskyQuestionmark();
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
			projectile.baseData.damage *= 0.5f;
			projectile.baseData.speed *= 1f;
			this.gun.DefaultModule.ammoCost = 1;
			base.PostProcessProjectile(projectile);
			projectile.gameObject.AddComponent<KuniaProjectile>();
		}
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
		}
	}

	internal class KuniaProjectile : MonoBehaviour
	{

		public void Start()
		{
			this.projectile = base.GetComponent<Projectile>();
			this.player = (this.projectile.Owner as PlayerController);
			Projectile projectile = this.projectile;
			this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("kunia_projectile_001");
		}


		private Projectile projectile;

		private PlayerController player;
	}
}



