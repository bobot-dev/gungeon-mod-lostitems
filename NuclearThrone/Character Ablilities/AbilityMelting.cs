using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;


namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityMelting : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Corps Exploion";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Melting_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityMelting>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "abiliy of melting";
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


			CorpseExplodeActiveItem corpsExploion = PickupObjectDatabase.GetById(403).GetComponent<CorpseExplodeActiveItem>();
			CorpseExplosionData = corpsExploion.CorpseExplosionData as ExplosionData;
			//this.m_bulletBank = base.GetComponent<AIBulletBank>();
			CorpseExplosionData.doDamage = true;
			//CorpseExplosionData.forceUseThisRadius = ;
			CorpseExplosionData.damageRadius = 3.5f;
			//CorpseExplosionData.damageToPlayer = 0f;
			CorpseExplosionData.damageToPlayer = 0f;
			CorpseExplosionData.damage = 15f;
			CorpseExplosionData.breakSecretWalls = false;
			CorpseExplosionData.secretWallsRadius = 0;
			CorpseExplosionData.doDestroyProjectiles = true;
			CorpseExplosionData.doForce = true;
			CorpseExplosionData.pushRadius = 5f;
			CorpseExplosionData.force = 100f;
			CorpseExplosionData.debrisForce = 50f;
			CorpseExplosionData.explosionDelay = 0.1f;
			//CorpseExplosionData.effect = 
			CorpseExplosionData.doScreenShake = false;
			//CorpseExplosionData.ss = false;
			CorpseExplosionData.doStickyFriction = true;
			CorpseExplosionData.doExplosionRing = true;
			CorpseExplosionData.isFreezeExplosion = false;
			CorpseExplosionData.freezeRadius = 0;
			//CorpseExplosionData.freezeEffect = 0;
			CorpseExplosionData.playDefaultSFX = true;
			CorpseExplosionData.IsChandelierExplosion = false;

		}

	// Token: 0x0600717F RID: 29055 RVA: 0x002C2DE4 File Offset: 0x002C0FE4


	// Token: 0x06007180 RID: 29056 RVA: 0x002C2E7C File Offset: 0x002C107C
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_dead_again_01", base.gameObject);
		//bool flag = false;
		for (int i = 0; i < StaticReferenceManager.AllCorpses.Count; i++)
		{
			GameObject gameObject = StaticReferenceManager.AllCorpses[i];
			if (gameObject && gameObject.GetComponent<tk2dBaseSprite>() && gameObject.transform.position.GetAbsoluteRoom() == user.CurrentRoom)
			{
				Vector2 worldCenter = gameObject.GetComponent<tk2dBaseSprite>().WorldCenter;
				Exploder.Explode(worldCenter, CorpseExplosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
				//Exploder.DoDefaultExplosion(worldCenter, Vector2.zero, null, true, CoreDamageTypes.None, false);

				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
		}
	}

	// Token: 0x06007181 RID: 29057 RVA: 0x002C2FD4 File Offset: 0x002C11D

	// Token: 0x06007182 RID: 29058 RVA: 0x002C30BC File Offset: 0x002C12BC

	// Token: 0x04007135 RID: 28981
	//public ScreenShakeSettings ScreenShake;

	// Token: 0x04007136 RID: 28982
	public ExplosionData CorpseExplosionData;
	

	}

}
