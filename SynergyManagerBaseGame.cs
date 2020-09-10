using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using HutongGames.PlayMaker.Actions;

namespace LostItems
{
	internal class CustomSynergiesBaseGame
	{
        public static void Init()
        {
                // Token: 0x02000008 RID: 8
            List<string> mandatoryConsoleIDs = new List<string>
            {
                "full_metal_jacket",
                "elder_blank"

            };
            CustomSynergies.Add("Elder Vest", mandatoryConsoleIDs, null, true);

            List<string> mandatoryConsoleIDs2 = new List<string>
            {
                "ser_manuels_revolver",
                "betrayers_shield"

            };
            CustomSynergies.Add("United Again", mandatoryConsoleIDs2, null, true);
            AdvancedDualWieldSynergyProcessor dualWield = new AdvancedDualWieldSynergyProcessor();
            dualWield.SynergyNameToCheck = "United Again";
        }
	}
}
