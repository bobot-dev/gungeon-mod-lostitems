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
	public class StarterGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Gold Mauser", "gold_starter");
			Game.Items.Rename("outdated_gun_mods:gold_mauser", "bot:gold_mauser");
			gun.gameObject.AddComponent<StarterGun>();
			gun.SetShortDescription("wipshort");
			gun.SetLongDescription("wiplong");
			gun.SetupSprite(null, "gold_starter_idle_001", 24);
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
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			
			Gun gun2 = PickupObjectDatabase.GetById(182) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.encounterTrackable.EncounterGuid = "gun made by NotABot for alabamahit";
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
			projectile.baseData.damage *= 0.75f;
			projectile.baseData.speed *= 1f;

			this.gun.DefaultModule.ammoCost = 1;
			base.PostProcessProjectile(projectile);
			//projectile.OnHitEnemy.
			projectile.gameObject.AddComponent<StarterGunProjectile>();

		}
		public void Pickup(PlayerController player)
		{
			this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			if (player.CurrentGun)
			{
				this.ProcessGunShader(player.CurrentGun);
			}

		}
		
		private void ProcessGunShader(Gun g)
		{
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == this.m_glintShader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(this.m_glintShader);

			//material.SetColor("_OverrideColor", new Color(252f, 204f, 45f));
			//material.SetColor("_OverrideColor", new Color(1f, 1f, 1f));
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;
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
				if (gun.ClipShotsRemaining <= 1)
				{
					ETGModConsole.Log("reload");
					float stealthChance = UnityEngine.Random.Range(1, 11);

					if (stealthChance >= 10)
					{

						ConsumableStealthItem smokebomb = PickupObjectDatabase.GetById(462).GetComponent<ConsumableStealthItem>();
						poofVfx = smokebomb.poofVfx.gameObject;
						
						player.ChangeSpecialShaderFlag(1, 1f);

						player.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
						player.ChangeSpecialShaderFlag(1, 1f);
						player.OnDidUnstealthyAction += this.BreakStealth;
						player.healthHaver.OnDamaged += this.OnDamaged;
						player.SetIsStealthed(true, "sg");

					}
				}
			}
		}

		private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			this.BreakStealth(this.gun.CurrentOwner as PlayerController);
		}

		private void BreakStealth(PlayerController obj)
		{
			obj.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
			obj.ChangeSpecialShaderFlag(1, 0f);
			obj.OnDidUnstealthyAction -= this.BreakStealth;
			obj.healthHaver.OnDamaged -= this.OnDamaged;
			obj.SetIsStealthed(false, "sg");
			//obj.SetCapableOfStealing(false, "StealthOnReloadPressed", null);
		}
		//public bool OnlyOnClipEmpty;
		public GameObject poofVfx;
		// Token: 0x04000026 RID: 38
		private bool HasReloaded;
		private Shader m_glintShader;


	}

	internal class StarterGunProjectile : MonoBehaviour
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

