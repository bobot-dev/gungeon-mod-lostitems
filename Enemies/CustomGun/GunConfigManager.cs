using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LostItems
{
	public class GunConfigManager : MonoBehaviour
	{
		public void Init()
		{
			try
			{
				this.CreateOrLoadConfiguration();
				// this.Build();
				GunConfigManager.Instance = this;
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

		public void CreateOrLoadConfiguration()
		{
			bool flag = !File.Exists(GunConfigManager.SaveFilePath);
			if (flag)
			{
				ETGModConsole.Log("GunConfigManager (Gun): Unable to find existing config, making a new one!", false);
				Directory.CreateDirectory(GunConfigManager.ConfigDirectory);
				File.Create(GunConfigManager.SaveFilePath).Close();
				this.UpdateConfiguration();
			}
			else
			{
				string text = File.ReadAllText(GunConfigManager.SaveFilePath);
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					this.configuration = JsonUtility.FromJson<GunConfigManager.Configuration>(text);
				}
				else
				{
					this.UpdateConfiguration();
				}
			}
		}

		public void UpdateConfiguration()
		{
			bool flag = !File.Exists(GunConfigManager.SaveFilePath);
			if (flag)
			{
				ETGModConsole.Log("GunConfigManager (Gun): Unable to find existing config, making a new one!", false);
				Directory.CreateDirectory(GunConfigManager.ConfigDirectory);
				File.Create(GunConfigManager.SaveFilePath).Close();
			}
			File.WriteAllText(GunConfigManager.SaveFilePath, JsonUtility.ToJson(this.configuration, true));
		}

		public GunConfigManager.Configuration configuration = new GunConfigManager.Configuration
		{
			ProjectileToCopy = 221,
			AmmoCost = 1,
			GunName = "custom gun",
			MaxAmmo = 250,
			ClipSize = 5,
			CassingCostPerShot = 0,
			Damage = 5,
			//ShootStyle = ProjectileModule.ShootStyle.SemiAutomatic,
			ReloadTime = 1,
			FireRate = 0.3f,
			InfiniteAmmo = false,
			Spread = 25,
			Quality = PickupObject.ItemQuality.SPECIAL
	};

		public struct Configuration
		{
			//public int MuzzleFlash;
			public int ProjectileToCopy;
			public int AmmoCost;
			public string GunName;
			public PickupObject.ItemQuality Quality;
			public int MaxAmmo;
			public int ClipSize;
			public int CassingCostPerShot;
			public float Damage;
			//public ProjectileModule.ShootStyle ShootStyle;
			public float ReloadTime;
			public float FireRate;
			public bool InfiniteAmmo;
			public float Spread;
		}





		public static GunConfigManager Instance;

		public static string ConfigDirectory = Path.Combine(ETGMod.ResourcesDirectory, "gunconfig");

		// Token: 0x0400000F RID: 15
		public static string SaveFilePath = Path.Combine(GunConfigManager.ConfigDirectory, "GunConfigManager.json");

	}
}
