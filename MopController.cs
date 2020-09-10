  using System;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using UnityEngine;

namespace LostItems
{
	// Token: 0x02000008 RID: 8
	public class MopController : GunBehaviour
	{
		// Token: 0x06000037 RID: 55 RVA: 0x0000369C File Offset: 0x0000189C
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("mop", "the_mop");
			Gungeon.Game.Items.Rename("outdated_gun_mods:mop", "bot:mop");
			gun.gameObject.AddComponent<MopController>();
			gun.SetShortDescription("Mop the floor with them");
			gun.SetLongDescription("It's a mop the jammed don't seem to be angered by it, maybe they feel sorry for you and your poor choices\n\nThis mop was brought to the Gungeon by a shape shifter called Ketra.");
			gun.SetupSprite(null, "the_mop_idle_001", 1);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.SetAnimationFPS(gun.chargeAnimation, 5);
			
			//gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(574) as Gun, true, false);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(541) as Gun, true, false);
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.chargeAnimation.Length.Equals(6);
			gun.SetBaseMaxAmmo(100);
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = 0f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 0;
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			gun.encounterTrackable.EncounterGuid = "mop the floor with them";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.InfiniteAmmo = true;
			Gun gun2 = (Gun)ETGMod.Databases.Items["wonderboy"];
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.IsHeroSword = false;
			
			//gun.HeroSwordDoesntBlank = true;
			gun.DefaultModule.GetCurrentProjectile().baseData.damage = 20f;
			gun.RemovePeskyQuestionmark();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003768 File Offset: 0x00001968
		public void Update()
		{

		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000379C File Offset: 0x0000199C
		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x == null;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			projectile.baseData.damage *= 1f;
			//projectile.spriteAnimator.Stop();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000381C File Offset: 0x00001A1C
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003864 File Offset: 0x00001A64
		public static ProjectileModule CopyFrom(ProjectileModule origin)
		{
			return new ProjectileModule();
		}

	}
}