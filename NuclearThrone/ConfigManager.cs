using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LostItems.NuclearThrone
{
	class ConfigManager : MonoBehaviour
	{
		public void Init()
		{
			try
			{
				this.CreateOrLoadConfiguration();
				// this.Build();
				ConfigManager.Instance = this;
				//this.UpdateAppearance();
			}
			catch (Exception e)
			{
				Log("bad thing happened coz " + e, "FF0000");
			}
		}

		public static void Log(string text, string color = "FFFFFF")
		{
			ETGModConsole.Log($"<color={color}>{text}</color>", false);
		}

		private void CreateOrLoadConfiguration()
		{
			bool flag = !File.Exists(ConfigManager.SaveFilePath);
			if (flag)
			{
				ETGModConsole.Log("ConfigManager (NT): Unable to find existing config, making a new one!", false);
				Directory.CreateDirectory(ConfigManager.ConfigDirectory);
				File.Create(ConfigManager.SaveFilePath).Close();
				this.UpdateConfiguration();
			}
			else
			{
				string text = File.ReadAllText(ConfigManager.SaveFilePath);
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					this.configuration = JsonUtility.FromJson<ConfigManager.Configuration>(text);
				}
				else
				{
					this.UpdateConfiguration();
				}
			}
		}

		public void UpdateConfiguration()
		{
			bool flag = !File.Exists(ConfigManager.SaveFilePath);
			if (flag)
			{
				ETGModConsole.Log("ConfigManager: Unable to find existing config, making a new one!", false);
				Directory.CreateDirectory(ConfigManager.ConfigDirectory);
				File.Create(ConfigManager.SaveFilePath).Close();
			}
			File.WriteAllText(ConfigManager.SaveFilePath, JsonUtility.ToJson(this.configuration, true));
		}

		public ConfigManager.Configuration configuration = new ConfigManager.Configuration
		{
			ProtoChest = -1,

			FishgGold = -1,
			CrystalGold= -1,
			EyesGold = -1,
			MeltingGold = -1,
			PlantGold = -1,
			YvGold = -1,
			SteroidsGold = -1,
			RobotGold = -1,
			ChickenGold = -1,
			RebelGold = -1,
			HorrorGold = -1,
			RogueGold = -1
		};

		public struct Configuration
		{
			public int ProtoChest;

			public int FishgGold;
			public int CrystalGold;
			public int EyesGold;
			public int MeltingGold;
			public int PlantGold;
			public int YvGold;
			public int SteroidsGold;
			public int RobotGold;
			public int ChickenGold;
			public int RebelGold;
			public int HorrorGold;
			public int RogueGold;

		}

		public static ConfigManager Instance;

		public static string ConfigDirectory = Path.Combine(ETGMod.ResourcesDirectory, "ntconfig");

		// Token: 0x0400000F RID: 15
		public static string SaveFilePath = Path.Combine(ConfigManager.ConfigDirectory, "ConfigManager.json");

	}
}
