using System;

namespace LostItems
{
	// Token: 0x0200000F RID: 15
	public static class CustomTrackedStatsManager
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00006D85 File Offset: 0x00004F85
		public static void SetCustomStat(CustomTrackedStats stat, float value)
		{
			GameStatsManager.Instance.SetStat(CustomTrackedStatsManager.GetTrackedStatForCustomTrackedStat(stat), value);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00006D9C File Offset: 0x00004F9C
		public static float GetCustomStat(CustomTrackedStats stat)
		{
			return GameStatsManager.Instance.GetPlayerStatValue(CustomTrackedStatsManager.GetTrackedStatForCustomTrackedStat(stat));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00006DBE File Offset: 0x00004FBE
		public static void RegisterStatChange(CustomTrackedStats stat, float value)
		{
			GameStatsManager.Instance.RegisterStatChange(CustomTrackedStatsManager.GetTrackedStatForCustomTrackedStat(stat), value);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00006DD4 File Offset: 0x00004FD4
		public static TrackedStats GetTrackedStatForCustomTrackedStat(CustomTrackedStats stat)
		{
			return (TrackedStats)stat;
		}
	}
}
