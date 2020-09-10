using ItemAPI;
using UnityEngine;
using GungeonAPI;
using Dungeonator;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using System.IO;

namespace LostItems
{
	public class Ping : PlayerItem
	{

		public static void Init()
		{
			//The name of the item
			string itemName = "Ping";
			string resourceName = "LostItems/sprites/ping";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<Ping>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "@everyone";
			string longDesc = "PING";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			item.consumable = false;
			item.quality = ItemQuality.EXCLUDED;
		}

		public override void Pickup(PlayerController player)
		{

			base.Pickup(player);
		}

		protected override void DoEffect(PlayerController user)
		{
			Begine();
		}

		public void Begine()
		{
				this.CreateOrLoadConfiguration();
				// this.Build();
				Ping.Instance = this;

		}

		public static void Log(string text, string color = "FFFFFF")
		{
			ETGModConsole.Log($"<color={color}>{text}</color>", false);
		}

		private void CreateOrLoadConfiguration()
		{
			bool flag = !File.Exists(Ping.SaveFilePath);
			if (flag)
			{
				ETGModConsole.Log("Ping: Unable to find existing config, making a new one!", false);
				Directory.CreateDirectory(Ping.ConfigDirectory);
				File.Create(Ping.SaveFilePath).Close();
				this.UpdateConfiguration();
			}
			else
			{
				string text = File.ReadAllText(Ping.SaveFilePath);
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					this.configuration = JsonUtility.FromJson<Ping.Configuration>(text);
				}
				else
				{
					this.UpdateConfiguration();
				}
			}
		}

		public void UpdateConfiguration()
		{
			bool flag = !File.Exists(Ping.SaveFilePath);
			if (flag)
			{
				ETGModConsole.Log("Ping: Unable to find existing config, making a new one!", false);
				Directory.CreateDirectory(Ping.ConfigDirectory);
				File.Create(Ping.SaveFilePath).Close();
			}
			File.WriteAllText(Ping.SaveFilePath, JsonUtility.ToJson(this.configuration, true));
		}

		public Ping.Configuration configuration = new Ping.Configuration
		{
			ShouldPing = false
		};

		public struct Configuration
		{
			public bool ShouldPing;

		}

		public static Ping Instance;

		public static string ConfigDirectory = Path.Combine(ETGMod.ResourcesDirectory, "ping");

		// Token: 0x0400000F RID: 15
		public static string SaveFilePath = Path.Combine(Ping.ConfigDirectory, "Ping.json");
	}
}
