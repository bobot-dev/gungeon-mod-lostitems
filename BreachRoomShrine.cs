using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using GungeonAPI;
using UnityEngine;


namespace LostItems
{
	// Token: 0x0200002D RID: 45
	public static class BreachRoomShrine
	{


		// Token: 0x0600015C RID: 348 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "Test Breach Shrine",
				modID = "bot",
				text =  "hi",
				spritePath = "LostItems/sprites/hmm.png",
				room = RoomFactory.BuildFromResource("LostItems/rooms/breachHangout.room").room,
				acceptText = "hi",
				declineText = "bye",
				OnAccept = new Action<PlayerController, GameObject>(BreachRoomShrine.Accept),
				OnDecline = null,
				CanUse = new Func<PlayerController, GameObject, bool>(BreachRoomShrine.CanUse),
				offset = new Vector3(0,0,0),
				talkPointOffset = new Vector3(0.75f, 1.5f, 0f),
				isToggle = false,
				isBreachShrine = true
			};
			shrineFactory.Build();
		}


		// Token: 0x0600015D RID: 349 RVA: 0x0000D194 File Offset: 0x0000B394
		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return false;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public static void Accept(PlayerController user, GameObject shrine)
		{


			
		}
 

		
	}
}
