using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using GungeonAPI;
using UnityEngine;


namespace LostItems
{
	// Token: 0x0200002D RID: 45
	public static class MoreBreachShrine
	{


		// Token: 0x0600015C RID: 348 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "Breach Shrine",
				modID = "bot",
				text =  "Please Select a Character",
				spritePath = "LostItems/NuclearThrone/NuclearThroneSprite/char_shrine_temp.png",
				
				acceptText = "Go to hang out",
				declineText = "No thanks",
				OnAccept = new Action<PlayerController, GameObject>(MoreBreachShrine.Accept),
				OnDecline = null,
				CanUse = new Func<PlayerController, GameObject, bool>(MoreBreachShrine.CanUse),
				offset = new Vector3(188.3f, 19.7f, 18.9f),
				talkPointOffset = new Vector3(0.75f, 1.5f, 0f),
				isToggle = false,
				isBreachShrine = true
			};
			shrineFactory.Build();
		}


		// Token: 0x0600015D RID: 349 RVA: 0x0000D194 File Offset: 0x0000B394
		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return true;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public static void Accept(PlayerController user, GameObject shrine)
		{
			//BreachRoomController roomController = new BreachRoomController();
			//roomController.StartTeleport(user);
			//GameManager.Instance.LoadCustomLevel("tt_tutorial");
			roomMagic(user);
			//GameManager.Instance.OnNewLevelFullyLoaded += OnNewFloor;

			
		}
 

		public static void roomMagic(PlayerController user)
		{
			
			Dungeon dungeon = GameManager.Instance.Dungeon;
			//dungeon.
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
			RoomHandler creepyRoom = tools.AddCustomRuntimeRoom(room, false, false, true, null, DungeonData.LightGenerationStyle.STANDARD);
			//PrototypeDungeonRoom creepyRoom =
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();

			//creepyRoom.PrecludeTilemapDrawing = true;
			//creepyRoom.OverrideTilemap = component;

			//tk2dTileMap component = (tk2dTileMap)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			//tk2dTileMap component = GameObject.Find("TileMap").GetComponent<tk2dTileMap>();
			//creepyRoom.OverrideTilemap = (tk2dTileMap)BraveResources.Load("Global Prefabs/ENV_Tileset_Gungeon", ".prefab");
			//creepyRoom.OverrideTilemap = component;
			creepyRoom.DrawPrecludedCeilingTiles = true;
			creepyRoom.PrecludeTilemapDrawing = true;
			TeleportToRoom(user, creepyRoom);

			user.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);
			bool flag2 = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER;
			if (flag2)
			{
				GameManager.Instance.GetOtherPlayer(user).ReuniteWithOtherPlayer(user, false);
			}
			//static float CharNum = 0;
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

		private static bool m_IsTeleporting;
		static PrototypeDungeonRoom room = RoomFactory.BuildFromResource("LostItems/rooms/breachHangout.room").room;
		private static Vector2 WarpTarget;
	}
}
