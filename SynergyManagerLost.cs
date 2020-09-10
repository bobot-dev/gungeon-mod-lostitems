using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LostItems
{
	internal class CustomSynergiesLost
	{
		// Token: 0x02000008 RID: 8
		public class LostRobesSmokeBombs : AdvancedSynergyEntry
		{
			// Token: 0x0600000B RID: 11 RVA: 0x0000276C File Offset: 0x0000096C
			public LostRobesSmokeBombs()
			{
				this.NameKey = "Assasine";
				this.MandatoryItemIDs = new List<int>
			{
				ETGMod.Databases.Items["Cloak of the Lost"].PickupObjectId,462
			};
				this.IgnoreLichEyeBullets = true;
				this.statModifiers = new List<StatModifier>(0);
				this.bonusSynergies = new List<CustomSynergyType>();
			}
		}
	}
}
