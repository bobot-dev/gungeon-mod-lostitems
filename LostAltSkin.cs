using System;
using System.Collections.Generic;
using GungeonAPI;
using UnityEngine;

namespace LostItems
{
	// Token: 0x0200002D RID: 45
	public static class LostAltSkin
	{


		// Token: 0x0600015C RID: 348 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "AltSkinForLost",
				modID = "bot_nt",
				text =  "Please Select a Character",
				spritePath = "LostItems/sprites/junk_heart.png",
				//room = RoomFactory.BuildFromResource("FrostAndGunfireItems/Resources/rooms/LostAltSkinRoom.room").room,
				acceptText = "Next Character",
				declineText = "This one's fine",
				OnAccept = new Action<PlayerController, GameObject>(LostAltSkin.Accept),
				OnDecline = null,
				CanUse = new Func<PlayerController, GameObject, bool>(LostAltSkin.CanUse),
				offset = new Vector3(189.3f, 17.7f, 18.9f),
				talkPointOffset = new Vector3(0.75f, 1.5f, 0f),
				isToggle = false,
				isBreachShrine = true
			};
			shrineFactory.Build();
		}


		// Token: 0x0600015D RID: 349 RVA: 0x0000D194 File Offset: 0x0000B394
		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return player.characterIdentity == PlayableCharacters.Guide;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public static void Accept(PlayerController player, GameObject shrine)
		{

			BuildLibrary(player);
		
		}

		private static void BuildLibrary(PlayerController player)
		{
			List<string> spriteNames = spriteNamesToReplace;
			List<Texture2D> sprites = new List<Texture2D>();
			foreach (string name in spriteNames)
			{
				sprites.Add(ResourceExtractor.GetTextureFromResource("LostItems/sprites/PlayerSprites" + name + ".png"));
			}

			tk2dSpriteAnimation library = UnityEngine.Object.Instantiate((PickupObjectDatabase.GetById(163) as BulletArmorItem).knightLibrary);
			UnityEngine.Object.DontDestroyOnLoad(library);
			var orig = library.clips[0].frames[0].spriteCollection;
			var copyCollection = UnityEngine.Object.Instantiate(orig);

			UnityEngine.Object.DontDestroyOnLoad(copyCollection);

			RuntimeAtlasPage page = new RuntimeAtlasPage();
			for (int i = 0; i < sprites.Count; i++)
			{
				var tex = sprites[i];
				var def = copyCollection.GetSpriteDefinition(tex.name);
				if (def != null)
				{
					def.ReplaceTexture(tex);
				}
			}
			page.Apply();

			foreach (var clip in library.clips)
			{
				for (int i = 0; i < clip.frames.Length; i++)
				{
					clip.frames[i].spriteCollection = copyCollection;
				}
			}
			//replacementLibrary = library;
			player.OverrideAnimationLibrary = library;


		}

		static List<string> spriteNamesToReplace;
	}
}
