using System;
using System.IO;
using GungeonAPI;
using LostItems.NuclearThrone;
using UnityEngine;

namespace LostItems
{
	// Token: 0x0200002D RID: 45
	public static class ProtoChest
	{


		// Token: 0x0600015C RID: 348 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "Proto Chest",
				modID = "bot_nt",
				text = "Store A weapon",
				spritePath = "LostItems/NuclearThrone/NuclearThroneSprite/chest/proto_chest.png",

				room = RoomFactory.BuildFromResource("LostItems/NuclearThrone/NuclearThroneRooms/proto_chest_room.room").room,
				acceptText = "Store Current Gun",
				declineText = "Close Chest",
				OnAccept = new Action<PlayerController, GameObject>(ProtoChest.Accept),
				OnDecline = null,
				CanUse = new Func<PlayerController, GameObject, bool>(ProtoChest.CanUse),
				offset = new Vector3(-1f, 0f, 0),
				//offset = new Vector3(189.3f, 13.7f, 18.9f),
				talkPointOffset = new Vector3(0f, 3f, 0f),
				isToggle = false,
				isBreachShrine = false
			};
			shrineFactory.Build();
		}

		public static bool CanUse(PlayerController player, GameObject shrine)
		{

			if (player.CurrentGun.CanBeDropped == false | player.CurrentGun.PickupObjectId == StoredGunID | player.CurrentGun.InfiniteAmmo == true)
			{
				return false;
			}
			else
			{
				return true;
			}

		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public static void Accept(PlayerController player, GameObject shrine)
		{
			//if (StoredGunID != 0)
			//{
			//StoredGunID = Convert.ToInt32(File.ReadAllText("NTConfig/protochest.json"));
			StoredGunID = ConfigManager.Instance.configuration.ProtoChest;

			

			//Char = "Your current character is " + NuclearShrine.Char;
			Gun gun = PickupObjectDatabase.GetById(StoredGunID) as Gun;

			StoredGunID = player.CurrentGun.PickupObjectId;

			//File.WriteAllText("NTConfig/protochest.json", StoredGunID + "");
			ConfigManager.Instance.configuration.ProtoChest = StoredGunID;


			player.inventory.DestroyCurrentGun();

			player.inventory.AddGunToInventory(gun, true);

			ConfigManager.Instance.UpdateConfiguration();

			AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
			//}
		}
		//
		public static int StoredGunID;
	}
}
