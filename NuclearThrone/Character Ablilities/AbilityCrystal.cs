using System;
using UnityEngine;
using ItemAPI;
using System.Collections;
using System.Collections.Generic;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityCrystal : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Shield";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Crystal_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityCrystal>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "ability of crystal";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.5f);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}
		public override void Pickup(PlayerController owner)
		{
			owner.OnRollStarted += this.OnRollStarted;

			PlayerController player = owner;
			base.Pickup(player);

		}

		protected override void DoEffect(PlayerController player)
		{

			StartEffect(player);
			StartCoroutine(ItemBuilder.HandleDuration(this, duration, player, EndEffect));
			//base.StartCoroutine(this.HandleShield(player));
		}

		private void StartEffect(PlayerController user)
		{
			hasStopped = false;
			user.ChangeSpecialShaderFlag(2, 1f);
			user.MovementModifiers += this.NoMotionModifier;
			//if (this.ModifiesDodgeRoll)
			//{

			//}

			//user.MovementModifiers Z
			//this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			//this.m_glintShader = Shader.Find("Brave/ItemSpecific/MetalSkinShader");
			//this.ProcessGunShader(user);
			user.IsGunLocked = true;
			//this.m_activeDuration2 = this.duration;
			this.m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
			user.sprite.usesOverrideMaterial = true;
			user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
			//user.
			this.EnableVFX(user);
			//this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			//Material material = new Material(this.m_glintShader);
			/*
			material.SetColor("_OverrideColor", new Color(252f, 204f, 45f));
            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;
			*/
			//user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
			//user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/LootGlintAdditivePass"));
			SpeculativeRigidbody specRigidbody = user.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			//user.healthHaver.IsVulnerable = false;

			//user.healthHaver.IsVulnerable = true;

		}
		private void OnRollStarted(PlayerController obj, Vector2 dirVec)
		{
			EndEffect(obj);
		}

		private void NoMotionModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
		{
			voluntaryVel = Vector2.zero;
		}

		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			Projectile component = otherRigidbody.GetComponent<Projectile>();
			bool flag = component != null && !(component.Owner is PlayerController);
			if (flag)
			{
				PassiveReflectItem.ReflectBullet(component, true, LastOwner.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
				PhysicsEngine.SkipCollision = true;
			}
		}

		private void EnableVFX(PlayerController user)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(46, 0, 99));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000114A8 File Offset: 0x0000F6A8
		private void DisableVFX(PlayerController user)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
		}

		private void EndEffect(PlayerController user)
		{
			if (hasStopped == true)
			{
				return;
			}
			this.DisableVFX(user);
			user.ChangeSpecialShaderFlag(2, 0f);
			user.MovementModifiers -= this.NoMotionModifier;
			user.IsGunLocked = false;
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
			//user.healthHaver.IsVulnerable = false;
			user.ClearOverrideShader();
			user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
			SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			specRigidbody2 = null;
			hasStopped = true;
		}

		public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}

		// Token: 0x0400007E RID: 126
		private float duration = 3f;

		// Token: 0x0400007F RID: 127
		private bool m_usedOverrideMaterial;
		bool hasStopped;

	}

}
