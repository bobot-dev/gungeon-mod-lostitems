using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using GungeonAPI;
using UnityEngine;
using Pathfinding;

namespace LostItems
{
	class BreachRoomController
    {
		public void StartTeleport(PlayerController user)
		{
			RoomHandler targetRoom = user.transform.position.GetAbsoluteRoom();
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleSeamlessTransitionToCombatRoom(targetRoom));
		}

		public static void roomMagic(PlayerController user)
		{
			//tk2dTileMap tk2dTileMap = null;
			user.CurrentRoom.CompletelyPreventLeaving = true;
			List<RoomHandler> rooms = GameManager.Instance.Dungeon.data.rooms;
			foreach (RoomHandler roomHandler in rooms)
			{
				roomHandler.visibility = RoomHandler.VisibilityStatus.OBSCURED;
				//roomHandler = null;
			}
			//List<RoomHandler>.Enumerator enumerator = default(List<RoomHandler>.Enumerator);
			WarpTarget = user.CenterPosition;
			RoomHandler currentRoom = user.CurrentRoom;
			user.ForceStopDodgeRoll();
			//this.DoTentacleVFX(user);
			IntVector2? targetCenter = new IntVector2?(user.CenterPosition.ToIntVector2(VectorConversions.Floor));
			Vector2 startingpoint = user.CenterPosition;
			//RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(new IntVector2(30, 30), (GameObject)BraveResources.Load(room));
			RoomHandler creepyRoom = tools.AddCustomRuntimeRoom(room, false, false, false, null, DungeonData.LightGenerationStyle.STANDARD);
			TeleportToRoom(user, creepyRoom);

			user.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);
			bool flag2 = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER;
			if (flag2)
			{
				GameManager.Instance.GetOtherPlayer(user).ReuniteWithOtherPlayer(user, false);
			}

		}
		public static void TeleportToRoom(PlayerController targetPlayer, RoomHandler targetRoom)
		{
			m_IsTeleporting = true;
			IntVector2? randomAvailableCell = targetRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, null);
			if (randomAvailableCell == null)
			{
				m_IsTeleporting = false;
				return;
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(targetPlayer);
				if (otherPlayer)
				{
					TeleportToRoom(otherPlayer, targetRoom);
				}
			}
			targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
			GameManager.Instance.StartCoroutine(HandleTeleportToRoom(targetPlayer, randomAvailableCell.Value.ToCenterVector2()));
			targetPlayer.specRigidbody.Velocity = Vector2.zero;
			targetPlayer.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			targetRoom.EnsureUpstreamLocksUnlocked();
		}

		private static IEnumerator HandleTeleportToRoom(PlayerController targetPlayer, Vector2 targetPoint)
		{
			//if (targetPlayer.transform.position.GetAbsoluteRoom() != null)
			//{
			//	this.StunEnemiesForTeleport(targetPlayer.transform.position.GetAbsoluteRoom(), 1f);
			//}
			targetPlayer.healthHaver.IsVulnerable = false;
			CameraController cameraController = GameManager.Instance.MainCameraController;
			Vector2 offsetVector = cameraController.transform.position - targetPlayer.transform.position;
			offsetVector -= cameraController.GetAimContribution();
			Minimap.Instance.ToggleMinimap(false, false);
			cameraController.SetManualControl(true, false);
			cameraController.OverridePosition = cameraController.transform.position;
			targetPlayer.CurrentInputState = PlayerInputState.NoInput;

			yield return new WaitForSeconds(1f);
			targetPlayer.ToggleRenderer(false, "arbitrary teleporter");
			targetPlayer.ToggleGunRenderers(false, "arbitrary teleporter");
			targetPlayer.ToggleHandRenderers(false, "arbitrary teleporter");
			yield return new WaitForSeconds(1f);
			Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
			yield return new WaitForSeconds(0.15f);
			targetPlayer.transform.position = targetPoint;
			targetPlayer.specRigidbody.Reinitialize();
			targetPlayer.specRigidbody.RecheckTriggers = true;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
			}
			else
			{
				cameraController.OverridePosition = (targetPoint + offsetVector).ToVector3ZUp(0f);
			}
			targetPlayer.WarpFollowersToPlayer(false);
			targetPlayer.WarpCompanionsToPlayer(false);

			Pixelator.Instance.MarkOcclusionDirty();
			Pixelator.Instance.FadeToBlack(0.15f, true, 0f);
			yield return null;

			cameraController.SetManualControl(false, true);

			targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
			yield return new WaitForSeconds(1.7f);
			targetPlayer.ToggleRenderer(true, "arbitrary teleporter");
			targetPlayer.ToggleGunRenderers(true, "arbitrary teleporter");
			targetPlayer.ToggleHandRenderers(true, "arbitrary teleporter");
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(targetPlayer.specRigidbody, null, false);
			targetPlayer.CurrentInputState = PlayerInputState.AllInput;
			targetPlayer.healthHaver.IsVulnerable = true;
			m_IsTeleporting = false;
			yield break;
		}

		private static void ExpandRoom(RoomHandler r)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", GameManager.Instance.gameObject);
			tk2dTileMap tk2dTileMap = null;
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			for (int i = -5; i < r.area.dimensions.x + 5; i++)
			{
				for (int j = -5; j < r.area.dimensions.y + 5; j++)
				{
					IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
					CellData cellData = (!dungeon.data.CheckInBoundsAndValid(intVector)) ? null : dungeon.data[intVector];
					if (cellData != null && cellData.type == CellType.WALL && cellData.HasTypeNeighbor(dungeon.data, CellType.FLOOR))
					{
						hashSet.Add(cellData.position);
					}
				}
			}
			foreach (IntVector2 key in hashSet)
			{
				CellData cellData2 = dungeon.data[key];
				cellData2.breakable = true;
				cellData2.occlusionData.overrideOcclusion = true;
				cellData2.occlusionData.cellOcclusionDirty = true;
				tk2dTileMap = dungeon.DestroyWallAtPosition(key.x, key.y, true);
				//if (UnityEngine.Random.value < 0.25f)
				//{
				//	this.VFXDustPoof.SpawnAtPosition(key.ToCenterVector3((float)key.y), 0f, null, null, null, null, false, null, null, false);
				//}
				r.Cells.Add(cellData2.position);
				r.CellsWithoutExits.Add(cellData2.position);
				r.RawCells.Add(cellData2.position);
			}
			Pixelator.Instance.MarkOcclusionDirty();
			Pixelator.Instance.ProcessOcclusionChange(r.Epicenter, 1f, r, false);
			if (tk2dTileMap)
			{
				dungeon.RebuildTilemap(tk2dTileMap);
			}
		}

		protected IEnumerator HandleSeamlessTransitionToCombatRoom(RoomHandler sourceRoom)
		{
			Dungeon d = GameManager.Instance.Dungeon;
			//sourceChest.majorBreakable.TemporarilyInvulnerable = true;
			//sourceRoom.DeregisterInteractable(sourceChest);
			int tmapExpansion = 13;
			RoomHandler newRoom = d.RuntimeDuplicateChunk(sourceRoom.area.basePosition, sourceRoom.area.dimensions, tmapExpansion, sourceRoom, true);
			newRoom.CompletelyPreventLeaving = true;
			List<Transform> movedObjects = new List<Transform>();

			//if (sourceChest.specRigidbody)
			//{
			//	PathBlocker.BlockRigidbody(sourceChest.specRigidbody, false);
			//}

			//GameObject spawnedVFX = SpawnManager.SpawnVFX(this.DrillVFXPrefab, sourceChest.transform.position + chestOffset, Quaternion.identity);
			//tk2dBaseSprite spawnedSprite = spawnedVFX.GetComponent<tk2dBaseSprite>();
			//spawnedSprite.HeightOffGround = 1f;
			//spawnedSprite.UpdateZDepth();
			Vector2 oldPlayerPosition = GameManager.Instance.BestActivePlayer.transform.position.XY();
			Vector2 playerOffset = oldPlayerPosition - sourceRoom.area.basePosition.ToVector2();
			Vector2 newPlayerPosition = newRoom.area.basePosition.ToVector2() + playerOffset;
			Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
			Pathfinder.Instance.InitializeRegion(d.data, newRoom.area.basePosition, newRoom.area.dimensions);
			GameManager.Instance.BestActivePlayer.WarpToPoint(newPlayerPosition, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
			}
			yield return null;
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				GameManager.Instance.AllPlayers[j].WarpFollowersToPlayer(false);
				GameManager.Instance.AllPlayers[j].WarpCompanionsToPlayer(false);
			}
			yield return d.StartCoroutine(this.HandleCombatRoomExpansion(sourceRoom, newRoom));
			//this.DisappearDrillPoof.SpawnAtPosition(spawnedSprite.WorldBottomLeft + new Vector2(-0.0625f, 0.25f), 0f, null, null, null, new float?(3f), false, null, null, false);
			//UnityEngine.Object.Destroy(spawnedVFX.gameObject);
			//sourceChest.ForceUnlock();
			//AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
			//AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", GameManager.Instance.gameObject);
			bool goodToGo = false;
			while (!goodToGo)
			{
				goodToGo = true;
				for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
				{
					//float num = Vector2.Distance(sourceChest.specRigidbody.UnitCenter, GameManager.Instance.AllPlayers[k].CenterPosition);
					//if (num > 3f)
					//{
					//	goodToGo = false;
					//}
				}
				yield return null;
			}
			GameManager.Instance.MainCameraController.SetManualControl(true, true);
			GameManager.Instance.MainCameraController.OverridePosition = GameManager.Instance.BestActivePlayer.CenterPosition;
			for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
			{
				GameManager.Instance.AllPlayers[l].SetInputOverride("shrinkage");
			}
			//yield return d.StartCoroutine(HandleCombatRoomShrinking(newRoom));
			for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
			{
				GameManager.Instance.AllPlayers[m].ClearInputOverride("shrinkage");
			}
			Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
			AkSoundEngine.PostEvent("Play_OBJ_paydaydrill_end_01", GameManager.Instance.gameObject);
			GameManager.Instance.MainCameraController.SetManualControl(false, false);
			//GameManager.Instance.BestActivePlayer.WarpToPoint(oldPlayerPosition, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
			}

			//this.m_inEffect = false;
			yield break;
		}

		private IEnumerator HandleCombatRoomShrinking(RoomHandler targetRoom)
		{
			float elapsed = 5.5f;
			int numExpansionsDone = 6;
			while (elapsed > 0f)
			{
				elapsed -= BraveTime.DeltaTime * 9f;
				while (elapsed < (float)numExpansionsDone && numExpansionsDone > 0)
				{
					numExpansionsDone--;
					this.ShrinkRoom(targetRoom);
				}
				yield return null;
			}
			yield break;
		}

		private void ShrinkRoom(RoomHandler r)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", GameManager.Instance.gameObject);
			tk2dTileMap tk2dTileMap = null;
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			for (int i = -5; i < r.area.dimensions.x + 5; i++)
			{
				for (int j = -5; j < r.area.dimensions.y + 5; j++)
				{
					IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
					CellData cellData = (!dungeon.data.CheckInBoundsAndValid(intVector)) ? null : dungeon.data[intVector];
					if (cellData != null && cellData.type != CellType.WALL && cellData.HasTypeNeighbor(dungeon.data, CellType.WALL))
					{
						hashSet.Add(cellData.position);
					}
				}
			}
			foreach (IntVector2 key in hashSet)
			{
				CellData cellData2 = dungeon.data[key];
				cellData2.breakable = true;
				cellData2.occlusionData.overrideOcclusion = true;
				cellData2.occlusionData.cellOcclusionDirty = true;
				tk2dTileMap = dungeon.ConstructWallAtPosition(key.x, key.y, true);
				r.Cells.Remove(cellData2.position);
				r.CellsWithoutExits.Remove(cellData2.position);
				r.RawCells.Remove(cellData2.position);
			}
			Pixelator.Instance.MarkOcclusionDirty();
			Pixelator.Instance.ProcessOcclusionChange(r.Epicenter, 1f, r, false);
			if (tk2dTileMap)
			{
				dungeon.RebuildTilemap(tk2dTileMap);
			}
		}
		private IEnumerator HandleCombatRoomExpansion(RoomHandler sourceRoom, RoomHandler targetRoom)
		{
			//yield return new WaitForSeconds(this.DelayPreExpansion);
			float duration = 5.5f;
			float elapsed = 0f;
			int numExpansionsDone = 0;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime * 9f;
				while (elapsed > (float)numExpansionsDone)
				{
					numExpansionsDone++;
					ExpandRoom(targetRoom);
					AkSoundEngine.PostEvent("Play_OBJ_rock_break_01", GameManager.Instance.gameObject);
				}
				yield return null;
			}
			Dungeon d = GameManager.Instance.Dungeon;
			Pathfinder.Instance.InitializeRegion(d.data, targetRoom.area.basePosition + new IntVector2(-5, -5), targetRoom.area.dimensions + new IntVector2(10, 10));
			//yield return new WaitForSeconds(this.DelayPostExpansionPreEnemies);
			//yield return this.HandleCombatWaves(d, targetRoom, sourceChest);
			yield break;
		}

		

		//System.Action<
		private static bool m_IsTeleporting;
		static PrototypeDungeonRoom room = RoomFactory.BuildFromResource("LostItems/rooms/breachHangout.room").room;
		private static Vector2 WarpTarget;
	}
}
