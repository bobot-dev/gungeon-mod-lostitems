using System;
using GungeonAPI;
using UnityEngine;

namespace LostItems
{
	// Token: 0x0200002D RID: 45
	public static class CharacterShrine
	{


		// Token: 0x0600015C RID: 348 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "Character Shrine Shrine",
				modID = "bot_nt",
				text =  "Please Select a Character",
				spritePath = "LostItems/NuclearThrone/NuclearThroneSprite/char_shrine_temp.png",
				//room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/CharacterShrineRoom.room").room,
				acceptText = "Next Character",
				declineText = "This one's fine",
				OnAccept = new Action<PlayerController, GameObject>(CharacterShrine.Accept),
				OnDecline = null,
				CanUse = new Func<PlayerController, GameObject, bool>(CharacterShrine.CanUse),
				offset = new Vector3(188.3f, 16.7f, 18.9f),
				talkPointOffset = new Vector3(0.75f, 1.5f, 0f),
				isToggle = false,
				isBreachShrine = true
			};
			shrineFactory.Build();
		}


		// Token: 0x0600015D RID: 349 RVA: 0x0000D194 File Offset: 0x0000B394
		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return !player.HasPickupID((ETGMod.Databases.Items["Nuclear Talisman"].PickupObjectId));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public static void Accept(PlayerController player, GameObject shrine)
		{
			//Char = "Your current character is " + NuclearShrine.Char;
			NuclearShrine.Char += 1;
			Popup();

			AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
		}
		static string Char = "Your current character is " + NuclearShrine.Char;
		//static float CharNum = 0;

		static void Popup()
		{
			switch (NuclearShrine.Char)
			{
				case 0:

					header = "random";
					break;

				case 1://fish that can roll

					header = "fish";
					break;

				case 2://shiny rock


					header = "crystal";

					break;

				case 3://lots o' eyes

					header = "eyes";
					break;

				case 4://dead

					header = "melting";
					break;

				case 5://plant

					header = "plant";
					break;

				case 6://gun god

					header = "yv";
					break;

				case 7://drugs

					header = "steroids";
					break;

				case 8://robot

					header = "robot";
					break;

				case 9://bird

					header = "chicken";
					break;

				case 10://minion spam

					header = "rebel";
					break;

				case 11://green guy number 67394-113

					header = "horror";
					break;

				default://cop
					header = "rogue";
					NuclearShrine.Char = 0;
					break;
			}

			Notify(header, text);
		}

		private static void Notify(string header, string text)
		{
			tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
			int spriteIdByName = encounterIconCollection.GetSpriteIdByName("LostItems/NuclearThrone/NuclearThroneSprite/Nuclear Talisman2");
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, encounterIconCollection, spriteIdByName, UINotificationController.NotificationColor.PURPLE, true, false);
		}
		static string header = "test";
		static string text = "Character";
	}
}
