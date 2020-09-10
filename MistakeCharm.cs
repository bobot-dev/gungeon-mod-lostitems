using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace LostItems
{
	public class MistakeCharm : PassiveItem
	{

		public static void Init()
		{
			//The name of the item
			string itemName = "Mistake Charm";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/sprites/mistakecharm";

			//Create new GameObject
			GameObject obj = new GameObject();

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<MistakeCharm>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = ":D";
			string longDesc = "This powerful item gives the one who holds it the power of the mistake. \n\nit seems to be stuck to your hand...";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

			//Adds the actual passive effect to the item
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DodgeRollDistanceMultiplier, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DodgeRollSpeedMultiplier, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DodgeRollDamage, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);

			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.CanBeDropped = false;
	
		}


		public override void Pickup(PlayerController player)
		{
			//player.OnKilledEnemy += this.OnKill;
			base.Pickup(player);
			this.EvaluateStats();
		}
		protected override void Update()
		{
			/*
			bool flag4 = base.Owner;
			bool flag5 = flag4;
			if (flag5)
			{
				this.EvaluateStats();
			}
			*/
			PlayerController lastOwner = this.Owner;
			bool flag = lastOwner && lastOwner.HasActiveItem(this.PickupObjectId);
			if (flag)
			{
				bool flag2 = lastOwner.HasPickupID(529) && !this.hasSynergy;
				if (flag2)
				{
					this.hasSynergy = true;
					this.needRestat = true;

				}
				else
				{
					bool flag3 = !lastOwner.HasPickupID(529) && this.needRestat;
					if (flag3)
					{
						this.needRestat = false;
						this.hasSynergy = false;
						lastOwner.stats.RecalculateStats(lastOwner, true, false);
					}
				}
			}
		}
		private void EvaluateStats()
		{
			if (PlayerStats.StatType.KnockbackMultiplier >= 0)
			{
				this.AddStat(PlayerStats.StatType.KnockbackMultiplier, -2, StatModifier.ModifyMethod.MULTIPLICATIVE);
				base.Owner.stats.RecalculateStats(base.Owner, true, false);
			}
		}
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.amount = amount;
			statModifier.statToBoost = statType;
			statModifier.modifyType = method;
			foreach (StatModifier statModifier2 in this.passiveStatModifiers)
			{
				bool flag = statModifier2.statToBoost == statType;
				bool flag2 = flag;
				if (flag2)
				{
					return;
				}
			}
			bool flag3 = this.passiveStatModifiers == null;
			bool flag4 = flag3;
			if (flag4)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
				return;
			}
			this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
			{
				statModifier
			}).ToArray<StatModifier>();
		}

		private bool hasSynergy;
		private bool needRestat;
	}
}
