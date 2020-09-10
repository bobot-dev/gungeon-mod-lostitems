using System;

namespace LostItems
{
	// Token: 0x0200000C RID: 12
	public static class CustomGungeonFlagsManager
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00006AEC File Offset: 0x00004CEC
		public static void SetCustomDungeonFlag(CustomDungeonFlags flag, bool value)
		{
			GameStatsManager.Instance.SetFlag(CustomGungeonFlagsManager.GetGungeonFlagForCustomDungeonFlag(flag), value);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00006B04 File Offset: 0x00004D04
		public static bool GetCustomDungeonFlag(CustomDungeonFlags flag)
		{
			return GameStatsManager.Instance.GetFlag(CustomGungeonFlagsManager.GetGungeonFlagForCustomDungeonFlag(flag));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00006B28 File Offset: 0x00004D28
		public static GungeonFlags GetGungeonFlagForCustomDungeonFlag(CustomDungeonFlags flag)
		{
			return (GungeonFlags)flag;
		}
	}
}
