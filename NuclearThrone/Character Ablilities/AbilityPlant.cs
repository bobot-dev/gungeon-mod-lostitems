using System;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityPlant : PlayerItem
	{
		//Call this method from the Start() method of your ETGModule extension
		public static void Init()
		{
			//The name of the item
			string itemName = "Snare";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Plant_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityPlant>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "ability of plant";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.5f);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.CanBeDropped = false;
		}
		public override void Pickup(PlayerController owner)
		{
			PlayerController player = owner;


			Gun Seed = PickupObjectDatabase.GetById(197) as Gun;
			sourceProjectile = Seed.DefaultModule.projectiles[0];




			//this.startDistance = 1f;
			//this.attackLength = 10f;
			//this.barrageRadius = 1.5f;

			
			base.Pickup(player);
		}

		private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(SnareGoop.goopDefinition).TimedAddGoopCircle(arg2.sprite.WorldBottomCenter, 3f, 1f, false);
		}

		private void HandleDestruction(Projectile sourceProjectile)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(SnareGoop.goopDefinition).TimedAddGoopCircle(sourceProjectile.LastPosition, 3f, 1f, false);
		}

		protected override void DoEffect(PlayerController player)
		{

			//StartEffect(player);
			//StartCoroutine(ItemBuilder.HandleDuration(this, duration, player, EndEffect));
			//Vector3 vector = player.unadjustedAimPoint - player.LockedApproximateSpriteCenter;
			//Vector3 vector2 = player.specRigidbody.UnitCenter;
			GameObject gameObject = SpawnManager.SpawnProjectile(sourceProjectile.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();

			bool flag = component != null;
			if (flag)
			{
				component.Owner = player;
				component.Shooter = player.specRigidbody;
				component.DefaultTintColor = new Color(0f, 0.45882352941f, 0.02745098039f);
				component.HasDefaultTint = true;
				component.baseData.damage = 0;

				component.OnDestruction += this.HandleDestruction;
				
			}
		}

		private void StartEffect(PlayerController user)
		{
			Vector2 vector = user.CenterPosition;
			Vector2 normalized = (user.unadjustedAimPoint.XY() - vector).normalized;
			vector += normalized * this.startDistance;
			this.HandleEngoopening(vector, normalized);

		}

		protected void HandleEngoopening(Vector2 startPoint, Vector2 direction)
		{
			float duration = 1f;
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(SnareGoop.goopDefinition);
			goopManagerForGoopType.TimedAddGoopLine(startPoint, startPoint + direction * this.attackLength, this.barrageRadius, duration);
		}

		private void EndEffect(PlayerController user)
		{

		}

		public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}

		// Token: 0x0400007E RID: 126
		private float duration = 3f;

		public float attackLength;
		public float barrageRadius;
		public float startDistance;


		Projectile sourceProjectile;
	}

}
