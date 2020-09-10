using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LostItems
{
    class Hunts : MonsterHuntQuest
    {
        public void Init()
        {
            ValidTargetMonsterGuids.Add("ec6b674e0acd4553b47ee94493d66422");
            QuestIntroString = "test quest";
            TargetStringKey = "1";
            NumberKillsRequired = 5;

        }
    }
}
