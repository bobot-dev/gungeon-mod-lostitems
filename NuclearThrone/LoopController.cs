using Dungeonator;
using System;
using System.Collections.Generic;
using UnityEngine;
using ItemAPI;

namespace LostItems.NuclearThrone
{
    class LoopController
    {
		public void Init()
		{
			victem.OnEnteredCombat += this.OnNewRoom;
			victem.OnNewFloorLoaded += this.OnNewFloor;
		}

		public void OnNewFloor(PlayerController player)
		{
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				GameManager.Instance.LoadCustomLevel("tt_castle");
				percentageHealthReduction = (0.1f * loopCount) + 1;
				AIActor.HealthModifier *= percentageHealthReduction;
				loopCount += 1;
			}
		}

	
        int loopCount = 1;
        float percentageHealthReduction = 0;

		public void OnNewRoom()
		{
			if (victem.HasPickupID(ETGMod.Databases.Items["Nuclear Talisman"].PickupObjectId))
			{
				List<AIActor> activeEnemies = victem.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
				bool flag = activeEnemies != null && victem.CurrentRoom.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.SPECIAL;
				if (flag)
				{
					int count = activeEnemies.Count;
					for (int i = 0; i < count; i++)
					{
						bool flag2 = activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && !activeEnemies[i].IsTransmogrified && activeEnemies[i].EnemyGuid != "22fc2c2c45fb47cf9fb5f7b043a70122";
						if (flag2)
						{
							AIActor aiactor = tools.SummonAtRandomPosition(activeEnemies[i].EnemyGuid, victem);
							aiactor.CanDropCurrency = true;
							aiactor.AssignedCurrencyToDrop = activeEnemies[i].AssignedCurrencyToDrop;
							aiactor.HandleReinforcementFallIntoRoom(0f);
						}
					}
				}
			}

		}

		public PlayerController victem;

	}

}
