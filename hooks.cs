using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using UnityEngine;
//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

namespace LostItems
{
	class hooks
	{

		public static void Init()
		{
			try
			{
				Hook UseAltWeaponShrine = new Hook(
					typeof(FoyerAlternateGunShrineController).GetMethod("DoEffect", BindingFlags.Instance | BindingFlags.Public),
					typeof(hooks).GetMethod("UseAltWeaponShrine")
					);
				/*
				Hook ExplodeHook = new Hook(
					typeof(Exploder).GetMethod("Explode", BindingFlags.Static | BindingFlags.Public),
					typeof(hooks).GetMethod("ExplodeHook")
					);*/
				Hook ChangeCostumeHook = new Hook(
					typeof(FoyerCharacterSelectFlag).GetMethod("ChangeToAlternateCostume", BindingFlags.Instance | BindingFlags.Public),
					typeof(hooks).GetMethod("ChangeCostumeHook")
					);

				Hook ExplosiveHook = new Hook(
					typeof(ExplosiveModifier).GetMethod("Explode", BindingFlags.Instance | BindingFlags.Public),
					typeof(hooks).GetMethod("ExplosiveHook")

				);
				Hook DamageHook = new Hook(
					typeof(PlayerController).GetMethod("OnDidDamage", BindingFlags.Instance | BindingFlags.Public),
					typeof(hooks).GetMethod("DamageHook")
				
				);



			}
			catch (NullReferenceException arg)
			{
				Log(string.Format("oh no thats not good (hooks broke): ", arg), "#eb1313");
				//LostItemsMod.Log(string.Format("D:", ), "#eb1313");
			}
		}

		public static void DamageHook(Action<PlayerController, float, bool, HealthHaver> orig, PlayerController self, float damageDone, bool fatal, HealthHaver target)
		{
			
			orig(self, damageDone, fatal, target);

			

			if (target.aiActor.EnemyGuid == "ffdc8680bdaa487f8f31995539f74265")
			{
				target.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
			}
		}


		public static void Log(string text, string color = "FFFFFF")
		{
			ETGModConsole.Log($"<color={color}>{text}</color>", false);
		}

		public static void ExplosiveHook(Action<ExplosiveModifier, Vector2, bool, CollisionData> orig, ExplosiveModifier self, Vector2 sourceNormal, bool ignoreDamageCaps = false, CollisionData cd = null)
		{
			
			if (self.projectile && self.projectile.Owner)
			{
				if (self.projectile.Owner is PlayerController)
				{
					for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
					{
						PlayerController playerController = GameManager.Instance.AllPlayers[i];
						if (playerController && playerController.specRigidbody)
						{
							if (playerController.HasPickupID(ETGMod.Databases.Items["Nuclear Talisman"].PickupObjectId)) 
							{
								self.explosionData.ignoreList.Remove(playerController.specRigidbody);
								self.explosionData.damageToPlayer = 4;
								if (playerController.HasPickupID(ETGMod.Databases.Items["Boiling Veins"].PickupObjectId) & playerController.healthHaver.GetCurrentHealth() <= 2)
								{
									self.explosionData.ignoreList.Add(playerController.specRigidbody);
								}
							}
							else
							{
								self.explosionData.ignoreList.Add(playerController.specRigidbody);
							}
						} 
					}
				}
				else
				{
					self.explosionData.ignoreList.Add(self.projectile.Owner.specRigidbody);
				}
			}
			Vector3 vector = (cd == null) ? self.specRigidbody.UnitCenter.ToVector3ZUp(0f) : cd.Contact.ToVector3ZUp(0f);
			if (self.doExplosion)
			{
				CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
				if (self.explosionData.doDamage && self.explosionData.damageRadius < 10f && self.projectile)
				{
					if (self.projectile.AppliesFreeze)
					{
						coreDamageTypes |= CoreDamageTypes.Ice;
					}
					if (self.projectile.AppliesFire)
					{
						coreDamageTypes |= CoreDamageTypes.Fire;
					}
					if (self.projectile.AppliesPoison)
					{
						coreDamageTypes |= CoreDamageTypes.Poison;
					}
					if (self.projectile.statusEffectsToApply != null)
					{
						for (int j = 0; j < self.projectile.statusEffectsToApply.Count; j++)
						{
							GameActorEffect gameActorEffect = self.projectile.statusEffectsToApply[j];
							if (gameActorEffect is GameActorFreezeEffect)
							{
								coreDamageTypes |= CoreDamageTypes.Ice;
							}
							else if (gameActorEffect is GameActorFireEffect)
							{
								coreDamageTypes |= CoreDamageTypes.Fire;
							}
							else if (gameActorEffect is GameActorHealthEffect)
							{
								coreDamageTypes |= CoreDamageTypes.Poison;
							}
						}
					}
				}
				Exploder.Explode(vector, self.explosionData, sourceNormal, null, self.IgnoreQueues, coreDamageTypes, ignoreDamageCaps);
			}
			if (self.doDistortionWave)
			{
				Exploder.DoDistortionWave(vector, self.distortionIntensity, self.distortionRadius, self.maxDistortionRadius, self.distortionDuration);
			}
		}

		public static void ChangeCostumeHook(Action<FoyerCharacterSelectFlag> orig, FoyerCharacterSelectFlag foyerCharacterSelect)
		{
			ETGModConsole.Log("Costume Changed", false);
			orig(foyerCharacterSelect);
		}

		public static void UseAltWeaponShrine(Action<FoyerAlternateGunShrineController, PlayerController> orig, FoyerAlternateGunShrineController shrine, PlayerController interactor)
		{
			
			//PlayerController player = ExplosiveModifier.gun.CurrentOwner as PlayerController;
			if (interactor.HasGun(LostGun.gunId))
			{
				interactor.inventory.RemoveGunFromInventory(PickupObjectDatabase.GetById(LostGun.gunId) as Gun);
			}
			//if (interactor.HasGun(LostGunAlt.gunId))
			//{
			//	interactor.inventory.RemoveGunFromInventory(PickupObjectDatabase.GetById(LostGunAlt.gunId) as Gun);
			//}
			orig(shrine, interactor);

			//do stuff :D
		}

		MetaShopTier metaShopTier = new MetaShopTier
		{
			itemId1 = 467,
			itemId2 = 349,
			itemId3 = 762,
			overrideTierCost = 500,
			overrideItem1Cost = 69,
			overrideItem2Cost = 420,
			overrideItem3Cost = 42069
	};

	}

}
