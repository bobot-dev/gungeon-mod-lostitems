using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using HutongGames.PlayMaker.Actions;
using LostItems.NuclearThrone;
using UnityEngine;

namespace LostItems
{
	// Token, 0x02000009 RID, 9
	public static class NuclearShrine
	{
		
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{

				name = "Nuclear Shrine",
				modID = "bot_nt",
				//text = "Might not be smart to stick around.",
				spritePath = "LostItems/NuclearThrone/NuclearThroneSprite/NPC/Idle/ntNPC_idle_001.png",
				shadowSpritePath = "LostItems/NuclearThrone/NuclearThroneSprite/NPC/Idle/ntNPC_idle_001.png",
				acceptText = "Bring it on!",
				declineText = "No thanks.",
				OnAccept = new Action<PlayerController, GameObject>(NuclearShrine.Accept),
				OnDecline = null,
				CanUse = new Func<PlayerController, GameObject, bool>(NuclearShrine.CanUse),
				//offset = new Vector3(43.8f, 42.4f, 42.9f),
				offset = new Vector3(188.8f, 18.3f, 18.9f),
				talkPointOffset = new Vector3(0.75f, 1.5f, 0f),
				isToggle = false,
				isBreachShrine = true,
				interactableComponent = typeof(NuclearInteractable)
			};
			GameObject gameObject = shrineFactory.Build();
			gameObject.AddAnimation("idle", "LostItems/NuclearThrone/NuclearThroneSprite/NPC/Idle", 12, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			gameObject.AddAnimation("talk", "LostItems/NuclearThrone/NuclearThroneSprite/NPC/Talk/", 12, NPCBuilder.AnimationType.Talk, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			gameObject.AddAnimation("talk_start", "LostItems/NuclearThrone/NuclearThroneSprite/NPC/StartTalk/", 12, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			gameObject.AddAnimation("do_effect", "LostItems/NuclearThrone/NuclearThroneSprite/NPC/DoEffect/", 12, NPCBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			NuclearInteractable component = gameObject.GetComponent<NuclearInteractable>();
			component.conversation = new List<string>
			{
				"Hello ",
				//$"Hello <color={LostItemsMod.TEXT_COLOR_NT}>wip</color>",
				"I will be the npc that enables the upcoming NUCLEAR THRONE MODE",
				//$"I will be the npc that enables the upcoming <color={LostItemsMod.TEXT_COLOR_NT}>NUCLEAR THRONE MODE</color>",
				

				"But thats not finished"
			};
			//gameObject.SetActive(false);
		}

		private static bool CanUse(PlayerController player, GameObject npc)
		{
			//return false;
			PlayableCharacters characterIdentity = player.characterIdentity;
			bool flag = characterIdentity == PlayableCharacters.Robot;
			if (flag)
			{
				return false;
			} else
			{
				return player != NuclearShrine.storedPlayer;
			}

		}

		public static void Accept(PlayerController player, GameObject npc)
		{
			
			//Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.Rainbow_Chest, npc. + Vector2.down, npc.sprite.WorldCenter.GetAbsoluteRoom(), true);
			npc.GetComponent<tk2dSpriteAnimator>().PlayForDuration("doEffect", -2f, "idle", false);
			//	string header = "LEVEL 1";
			//	string text = "MAY THE GUN GODZ BE WITH YOU";
			//	Notify(header, text);
			HandleStats(player);
			NuclearShrine.HandleLoadout(player);
			//LootEngine.SpawnItem(ETGMod.Databases.Items["Nuclear Talisman"].gameObject, new Vector3(189.8f, 18.3f, 18.9f), Vector2.zero, 1f, false, true, false);
			NuclearShrine.storedPlayer = player;
			LoopController loopController = new LoopController();
			loopController.victem = player;
			loopController.Init();

		}
		public static void HandleLoadout(PlayerController player)
		{

			//PassiveItem weightedRandomItem = 
			//player.startingGunIds.Add(weightedRandomItem.PickupObjectId);
			StripPlayer(player);
			
			
			LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items["Nuclear Talisman"].gameObject, player, true);
			Gun gun = PickupObjectDatabase.GetById(80) as Gun;
			Gun gunGold = PickupObjectDatabase.GetById(652) as Gun;
			Gun gunCop = PickupObjectDatabase.GetById(58) as Gun;
			Gun gunSword = PickupObjectDatabase.GetById(574) as Gun;
			if (Char <= 0)
			{
				Char = UnityEngine.Random.Range(1, 13);
			}

			switch(Char)
			{
				case 1://fish that can roll
					player.inventory.AddGunToInventory(gun, true);
					player.stats.SetBaseStatValue(PlayerStats.StatType.DodgeRollDistanceMultiplier, 1.2f, player);
					player.stats.SetBaseStatValue(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.5f, player);
					//player.stats.SetBaseStatValue(PlayerStats.StatType.DodgeRollDamage, 3f, player);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "fish";
					break;

				case 2://shiny rock
					player.inventory.AddGunToInventory(gun, true);
					player.stats.SetBaseStatValue(PlayerStats.StatType.Health, 5, player);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);

					header = "crystal";

					break;

				case 3://lots o' eyes
					player.inventory.AddGunToInventory(gun, true);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "eyes";
					break;

				case 4://dead
					player.inventory.AddGunToInventory(gun, true);
					player.stats.SetBaseStatValue(PlayerStats.StatType.Health, 1, player);
					LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items["Corps Exploion"].gameObject, player, true);
					header = "melting";
					break;

				case 5://plant
					player.inventory.AddGunToInventory(gun, true);
					player.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, 8, player);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "plant";
					break;

				case 6://gun god
					player.inventory.AddGunToInventory(gunGold, true);
					player.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, 1.5f, player);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "yv";
					break;

				case 7://drugs
					player.inventory.AddGunToInventory(gun, true);
					player.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, 1.8f, player);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "steroids";
					break;

				case 8://robot
					player.inventory.AddGunToInventory(gun, true);
					LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items["Eat Weapon"].gameObject, player, true);
					header = "robot";
					break;

				case 9://bird
					player.inventory.AddGunToInventory(gunSword, true);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "chicken";
					break;

				case 10://minion spam
					player.inventory.AddGunToInventory(gun, true);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "rebel";
					break;

				case 11://green guy number 67394-113
					player.inventory.AddGunToInventory(gun, true);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "horror";
					break;

				default://cop
					player.inventory.AddGunToInventory(gunCop, true);
					//LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items[""].gameObject, player, true);
					header = "rogue";
					break;
			}
			Notify(header, text);

		}

		public static void HandleStats(PlayerController player)
		{
			player.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalBlanksPerFloor, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalGunCapacity, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalItemCapacity, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotPiercing, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.AmmoCapacityMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.ChargeAmountMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.Coolness, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.Curse, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.Damage, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.DamageToBosses, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.DodgeRollDamage, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.DodgeRollDistanceMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.ExtremeShadowBulletChance, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.GlobalPriceMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.Health, 4, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.KnockbackMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.MoneyMultiplierFromEnemies, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, 7, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.PlayerBulletScale, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.RangeMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.ShadowBulletChance, 0, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.TarnisherClipCapacityMultiplier, 1, player);
			player.stats.SetBaseStatValue(PlayerStats.StatType.ThrownGunDamage, 1, player);

			player.healthHaver.FullHeal();
		}

			public static void StripPlayer(PlayerController player)
		{
			List<int> startingPassiveItemIds = player.startingPassiveItemIds;
			foreach (int pickupId in startingPassiveItemIds)
			{
				player.RemovePassiveItem(pickupId);
			}
			player.passiveItems = new List<PassiveItem>();
			player.startingPassiveItemIds = new List<int>();
			player.RemoveAllPassiveItems();
			bool flag = player.inventory != null;
			if (flag)
			{
				player.inventory.DestroyAllGuns();
			}
			player.startingGunIds = new List<int>();
			player.startingAlternateGunIds = new List<int>();
			player.activeItems.Clear();
			player.startingActiveItemIds = new List<int>();
		}

		private static void Notify(string header, string text)
		{
			tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
			int spriteIdByName = encounterIconCollection.GetSpriteIdByName("LostItems/NuclearThrone/NuclearThroneSprite/Nuclear Talisman2");
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, encounterIconCollection, spriteIdByName, UINotificationController.NotificationColor.PURPLE, false, false);
		}
		// Token, 0x0600004F RID, 79 RVA, 0x00004810 File Offset, 0x00002A10
		private static PlayerController storedPlayer;
		public static int Char = -1;

		public static string header = "test";
		public static string text = "Character";

	}
}
	


