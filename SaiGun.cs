using Gungeon;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;


namespace LostItems
{
	// Token: 0x02000008 RID: 8
	public class SaiGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("sai", "sai");
			Game.Items.Rename("outdated_gun_mods:sai", "bot_head:sai");
			gun.gameObject.AddComponent<SaiGun>();
			gun.SetShortDescription("Keeps winning, hopefully...");
			gun.SetLongDescription("This gun is incredably powerful but it is know to backfire quite offen...\n\n A gun designed by a gunsmith called Sai. ");
			gun.SetupSprite(null, "sai_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(38) as Gun, true, false);
			//GunExt.AddProjectileModuleFrom(gun, "blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.5f;
			gun.DefaultModule.cooldownTime = 0.3f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 6;
			gun.SetBaseMaxAmmo(300);
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			Gun gun2 = PickupObjectDatabase.GetById(480) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.encounterTrackable.EncounterGuid = "local man just keeps winning";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.RemovePeskyQuestionmark();
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
			projectile.baseData.damage *= 1000f;
			projectile.baseData.speed *= 1f;
			this.gun.DefaultModule.ammoCost = 1;
			base.PostProcessProjectile(projectile);
			projectile.gameObject.AddComponent<LostGunProjectile>();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003673 File Offset: 0x00001873
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			int roulette = UnityEngine.Random.Range(1, 7);
			string rouletteValue = "shot number " + roulette;
			ETGModConsole.Log(rouletteValue, false);
			switch (roulette)
			{
				case 6:
					AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
					player.healthHaver.ApplyDamage(10000f, Vector2.zero, "RNG", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
					break;
				default:
					gun.PreventNormalFireAudio = true;
					AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
					break;
			}
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
				AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);
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
				//projectile.sprite.GetSpriteIdByName("sai_projectile_001");
				//this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("sai_projectile_001");
				//ETGModConsole.Log("hi - Initialized", false);
			}


			private Projectile projectile;

			private PlayerController player;
		}
	}
}
