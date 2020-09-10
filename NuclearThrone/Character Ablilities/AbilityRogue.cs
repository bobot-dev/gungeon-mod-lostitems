using System.Collections.Generic;
using UnityEngine;
using ItemAPI;
using System.Collections;


namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityRogue : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Portal Strike";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Rogue_icon2-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityRogue>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "abiliy of rogue";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);

			DirectionalAttackActiveItem airStrike = PickupObjectDatabase.GetById(252).GetComponent<DirectionalAttackActiveItem>();
			reticleQuad = airStrike.reticleQuad.gameObject;
			//doesBarrage = airStrike.doesBarrage;
			doesBarrage = true;
			//doesBarrage = true;
			AudioEvent = airStrike.AudioEvent;
			//barrageVFX = airStrike.barrageVFX.gameObject;
			barrageExplosionData = airStrike.barrageExplosionData as ExplosionData;
			//SkipTargetingStep = airStrike.SkipTargetingStep;
			SkipTargetingStep = false;
			barrageExplosionData.damageToPlayer = 3;

			this.initialWidth = 3f;
			this.finalWidth = 3f;
			this.startDistance = 5f;
			this.attackLength = 10f;
			this.BarrageColumns = 1;
			this.barrageRadius = 1.5f;
			this.delayBetweenStrikes = 0.25f;

		}
		public override void Update()
		{
			base.Update();
			if (base.IsCurrentlyActive && this.m_extantReticleQuad)
			{
				Vector2 centerPosition = this.m_currentUser.CenterPosition;
				Vector2 normalized = (this.m_currentUser.unadjustedAimPoint.XY() - centerPosition).normalized;
				this.m_extantReticleQuad.transform.localRotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(normalized));
				Vector2 v = centerPosition + normalized * this.startDistance + (Quaternion.Euler(0f, 0f, -90f) * normalized * (this.initialWidth / 2f)).XY();
				this.m_extantReticleQuad.transform.position = v;
			}
		}

		// Token: 0x060071B3 RID: 29107 RVA: 0x002C4478 File Offset: 0x002C2678
		protected override void DoEffect(PlayerController user)
		{
			base.IsCurrentlyActive = true;
			this.m_currentUser = user;
			Vector2 centerPosition = user.CenterPosition;
			Vector2 normalized = (user.unadjustedAimPoint.XY() - centerPosition).normalized;
			if (this.SkipTargetingStep)
			{
				this.DoActiveEffect(user);
			}
			else
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.reticleQuad);
				this.m_extantReticleQuad = gameObject.GetComponent<tk2dSlicedSprite>();
				this.m_extantReticleQuad.dimensions = new Vector2(this.attackLength * 16f, this.initialWidth * 16f);
				if (normalized != Vector2.zero)
				{
					this.m_extantReticleQuad.transform.localRotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(normalized));
				}
				Vector2 v = centerPosition + normalized * this.startDistance + (Quaternion.Euler(0f, 0f, -90f) * normalized * (this.initialWidth / 2f)).XY();
				this.m_extantReticleQuad.transform.position = v;
			}
		}

		// Token: 0x060071B4 RID: 29108 RVA: 0x002C4680 File Offset: 0x002C2880
		protected override void DoActiveEffect(PlayerController user)
		{
			if (this.m_isDoingBarrage)
			{
				return;
			}
			if (this.m_extantReticleQuad)
			{
				UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
			}
			Vector2 vector = user.CenterPosition;
			Vector2 normalized = (user.unadjustedAimPoint.XY() - vector).normalized;
			vector += normalized * this.startDistance;
			if (this.doesBarrage)
			{
				List<Vector2> targets = this.AcquireBarrageTargets(vector, normalized);
				user.StartCoroutine(this.HandleBarrage(targets));
			}
			else
			{
				base.IsCurrentlyActive = false;
			}
			if (!string.IsNullOrEmpty(this.AudioEvent))
			{
				AkSoundEngine.PostEvent(this.AudioEvent, base.gameObject);
			}
		}
		private IEnumerator HandleBarrage(List<Vector2> targets)
		{
			this.m_isDoingBarrage = true;
			while (targets.Count > 0)
			{
				Vector2 currentTarget = targets[0];
				targets.RemoveAt(0);
				Exploder.Explode(currentTarget, this.barrageExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
				yield return new WaitForSeconds(this.delayBetweenStrikes / (float)this.BarrageColumns);
			}
			yield return new WaitForSeconds(0.25f);
			this.m_isDoingBarrage = false;
			this.IsCurrentlyActive = false;
			yield break;
		}

		// Token: 0x060071B7 RID: 29111 RVA: 0x002C47B4 File Offset: 0x002C29B4
		protected List<Vector2> AcquireBarrageTargets(Vector2 startPoint, Vector2 direction)
		{
			List<Vector2> list = new List<Vector2>();
			float num = -this.barrageRadius / 2f;
			float z = BraveMathCollege.Atan2Degrees(direction);
			Quaternion rotation = Quaternion.Euler(0f, 0f, z);
			while (num < this.attackLength)
			{
				float t = Mathf.Clamp01(num / this.attackLength);
				float num2 = Mathf.Lerp(this.initialWidth, this.finalWidth, t);
				float x = Mathf.Clamp(num, 0f, this.attackLength);
				for (int i = 0; i < this.BarrageColumns; i++)
				{
					float num3 = Mathf.Lerp(-num2, num2, ((float)i + 1f) / ((float)this.BarrageColumns + 1f));
					float num4 = UnityEngine.Random.Range(-num2 / (4f * (float)this.BarrageColumns), num2 / (4f * (float)this.BarrageColumns));
					Vector2 v = new Vector2(x, num3 + num4);
					Vector2 b = (rotation * v).XY();
					list.Add(startPoint + b);
				}
				num += this.barrageRadius;
			}
			return list;
		}

		// Token: 0x060071B8 RID: 29112 RVA: 0x002C48D5 File Offset: 0x002C2AD5
		protected override void OnDestroy()
		{
			if (this.m_extantReticleQuad)
			{
				UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
			}
			base.OnDestroy();
		}

		// Token: 0x04007314 RID: 29460
		public float initialWidth;

		// Token: 0x04007315 RID: 29461
		public float finalWidth;

		// Token: 0x04007316 RID: 29462
		public float startDistance;

		// Token: 0x04007317 RID: 29463
		public float attackLength;

		// Token: 0x04007318 RID: 29464
		public GameObject reticleQuad;

		// Token: 0x04007319 RID: 29465
		//public bool doesGoop;

		// Token: 0x0400731A RID: 29466
		//public GoopDefinition goopDefinition;

		// Token: 0x0400731B RID: 29467
		public bool doesBarrage;

		// Token: 0x0400731C RID: 29468
		public int BarrageColumns;

		// Token: 0x0400731D RID: 29469
		public GameObject barrageVFX;

		// Token: 0x0400731E RID: 29470
		public ExplosionData barrageExplosionData;

		// Token: 0x0400731F RID: 29471
		public float barrageRadius;

		// Token: 0x04007320 RID: 29472
		public float delayBetweenStrikes;

		// Token: 0x04007321 RID: 29473
		public bool SkipTargetingStep;

		// Token: 0x04007322 RID: 29474
		public string AudioEvent;

		// Token: 0x04007323 RID: 29475
		private PlayerController m_currentUser;

		// Token: 0x04007324 RID: 29476
		private tk2dSlicedSprite m_extantReticleQuad;


		// Token: 0x04007326 RID: 29478
		private bool m_isDoingBarrage;

	}

}
