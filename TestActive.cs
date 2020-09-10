using ItemAPI;
using UnityEngine;
using GungeonAPI;
using Dungeonator;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using System.Linq;

namespace LostItems
{
	public class TestActive : PlayerItem
	{

		public static void Init()
		{
			//The name of the item
			string itemName = "Test active Item";
			string resourceName = "LostItems/sprites/wip";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<TestActive>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "testing item";
			string longDesc = "this item is purly for testing";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			item.consumable = false;
			item.quality = ItemQuality.EXCLUDED;
		}
		private Dictionary<string, StringTableManager.StringCollection> leadMaidan;
		public override void Pickup(PlayerController player)
		{

			ETGModConsole.Log("" + StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].Count());
			ETGModConsole.Log("" + StringTableManager.GetSynergyString("#DESTINY_TWO"), false);
			ETGModConsole.Log("" + StringTableManager.GetSynergyString("#DESTINY_ONE"), false);
			//Dictionary<string, StringTableManager.StringCollection> leadMaidan = new Dictionary<string, StringTableManager.StringCollection>();
			StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].AddString("Lead maidens are singlehandedly ruining this game. There's no reason an extremely common random enemy should be tons harder than everything else so far including the floor 2 boss even though Lead Maidens started showing up before that fight. It makes every moment not spent fighting a Lead Maiden pointless because whether or not I win depends 99% on whether or not that bullshit miniboss appears. I've never seen one nonboss singlehandedly ruin a game before but this one is doing it extremely efficiently.Beating one Lead Maiden is harder than beating Rabi - Ribi's True Boss Rush. It's fucking unforgivable to have the game change so radically every time that enemy shows up.It kills me in like one fucking hit & it has far more health than everything else I've fought before despite also being bigger & faster than everything else too. Whomever came up with that enemy needs to go jack off to medieval torture porn & get it out of their system.Jesus Christ! I really wish the creators of this game would've just decided whether to make an amazing game or the worst game ever & stuck with it.", 100000000);
			StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_BEGRUDGING"].AddString("Lead maidens are singlehandedly ruining this game. There's no reason an extremely common random enemy should be tons harder than everything else so far including the floor 2 boss even though Lead Maidens started showing up before that fight. It makes every moment not spent fighting a Lead Maiden pointless because whether or not I win depends 99% on whether or not that bullshit miniboss appears. I've never seen one nonboss singlehandedly ruin a game before but this one is doing it extremely efficiently.Beating one Lead Maiden is harder than beating Rabi - Ribi's True Boss Rush. It's fucking unforgivable to have the game change so radically every time that enemy shows up.It kills me in like one fucking hit & it has far more health than everything else I've fought before despite also being bigger & faster than everything else too. Whomever came up with that enemy needs to go jack off to medieval torture porn & get it out of their system.Jesus Christ! I really wish the creators of this game would've just decided whether to make an amazing game or the worst game ever & stuck with it.", 100000000);
			StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_FRIENDS"].AddString("Lead maidens are singlehandedly ruining this game. There's no reason an extremely common random enemy should be tons harder than everything else so far including the floor 2 boss even though Lead Maidens started showing up before that fight. It makes every moment not spent fighting a Lead Maiden pointless because whether or not I win depends 99% on whether or not that bullshit miniboss appears. I've never seen one nonboss singlehandedly ruin a game before but this one is doing it extremely efficiently.Beating one Lead Maiden is harder than beating Rabi - Ribi's True Boss Rush. It's fucking unforgivable to have the game change so radically every time that enemy shows up.It kills me in like one fucking hit & it has far more health than everything else I've fought before despite also being bigger & faster than everything else too. Whomever came up with that enemy needs to go jack off to medieval torture porn & get it out of their system.Jesus Christ! I really wish the creators of this game would've just decided whether to make an amazing game or the worst game ever & stuck with it.", 100000000);

			//StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_BEGRUDGING"].AddString("Wow you have less brain cells than a titan main!", 100000);
			//StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].AddString("Wow you have less brain cells than a titan main!", 100000);
			StringTableManager.CoreTable["#INSULT_NAME"].AddString("titan main", 100000);
			StringTableManager.CoreTable["#INSULT_NAME"].AddString("gunslinger main", 100000);
			// = leadMaidan;


			ETGModConsole.Log("" + StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].Count());

			//ETGModConsole.Log(StringTableManager.EvaluateReplacementToken("%INSULT"));


			//ungeon dungeon = GameManager.Instance.Dungeon;

			//RationItem ration = PickupObjectDatabase.GetById(104).GetComponent<RationItem>();
			//this.healVFX = ration.healVFX;
			//healingAmount = 1;
			//BulletArmorItem bulletArmor = PickupObjectDatabase.GetById(160).GetComponent<BulletArmorItem>();
			//transformSprites = player.AlternateCostumeLibrary;

			base.Pickup(player);
			//player.CurrentGun.OnPostFired += this.OnFired;

			//player.CurrentGun.OnReloadPressed += this.OnReload;

		}

		public void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration)
		{
			TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, stringKey, string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}

		protected override void DoEffect(PlayerController user)
		{

			this.DoAmbientTalk(user.transform, new Vector3(1,2,0), "git gud", 4f);

			ETGModConsole.Log(StringTableManager.EvaluateReplacementToken("%INSULT"));
			ETGModConsole.Log(StringTableManager.GetString("#MASKGUN_ROOMCLEAR_ENMITY"));
			ETGModConsole.Log(StringTableManager.GetString("#MASKGUN_ROOMCLEAR_BEGRUDGING"));


			return;
			//Dictionary<string, StringTableManager.StringCollection> diolog = StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_BEGRUDGING"];
			Dictionary<string, StringTableManager.StringCollection> dialog = new Dictionary<string, StringTableManager.StringCollection>();

			List<string> textList = new List<string>();

			textList = dialog.Select(kvp => kvp.Key).ToList();

			textList = dialog.Keys.ToList();

			int t = 1;

			foreach (string text in textList)
			{
				
				ETGModConsole.Log(text);
				ETGModConsole.Log("" + t);
				t++;
			}


			
			//GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_DEATHS, 1000000f);
			GameStatsManager.Instance.SetStat(TrackedStats.NUMBER_DEATHS, 1000000f);
			Shader shader = Shader.Find("Brave/Internal/GlitchEevee");
			//user.SetOverrideShader(ShaderCache.Acquire("Brave/Internal/GlitchEevee"));
			Camera.main.RenderWithShader(shader, "10");
			Camera.current.RenderWithShader(shader, "10");
			GameManager manager = GameManager.Instance;

			ETGModConsole.Log(user.name, false);
			ETGModConsole.Log(user.ActorName, false);

			GameOptions options = new GameOptions();
			//options.CurrentLanguage = StringTableManager.GungeonSupportedLanguages.RUBEL_TEST;

			//MoreBreachShrine.roomMagic(user);
			Dungeon dungeon = GameManager.Instance.Dungeon;
			if (component = null)
			{
				component = dungeon.data.tilemap;
			}
			
			
			Vector2 pos = user.sprite.WorldCenter;
			dungeon.BossMasteryTokenItemId = Apache.gunId;
			//dungeon.OverrideAmbientColor = Color.black;
			//dungeon.OverrideAmbientLight = true;
			//dungeon.data.tilemap = component;
			dungeon.DungeonFloorLevelTextOverride = "bob";


			//user.ForceMetalGearMenu = true;
			AkSoundEngine.PostEvent("Play_WPN_earthwormgun_shot_01", base.gameObject);
			//SpriteOutlineManager.RemoveOutlineFromSprite(user.sprite, false);
			//zSpriteOutlineManager.AddOutlineToSprite(user.sprite, Color.red);

			GameManager.PVP_ENABLED = true;

			BulletArmorItem bulletArmor = PickupObjectDatabase.GetById(160).GetComponent<BulletArmorItem>();
			transformSprites = bulletArmor.knightLibrary;

			user.SwapToAlternateCostume();
			//Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.Rainbow_Chest, npc.sprite.WorldCenter + Vector2.down, npc.sprite.WorldCenter.GetAbsoluteRoom(), true);
			//MoreBreachShrine.roomMagic(user);
			//user.OverrideAnimationLibrary = bulletArmor.knightLibrary;
			//Start();
		}



		// Token: 0x06006A61 RID: 27233 RVA: 0x0028DB29 File Offset: 0x0028BD29
		public override void Update()
		{
			//SpriteOutlineManager.AddSingleOutlineToSprite(LastOwner.sprite, new IntVector2(0, 0), Color.red);
			if (LastOwner.CurrentGun.ClipShotsRemaining != lastClip)
			{
				lastClip = LastOwner.CurrentGun.ClipShotsRemaining;
				ETGModConsole.Log("you have " + lastClip + " shots left out of " + LastOwner.CurrentGun.ClipCapacity);
			}
		}


		int lastClip;

		public tk2dSpriteAnimation transformSprites;

		public float healingAmount;

		public GameObject healVFX;
		tk2dTileMap component;
	}
}
