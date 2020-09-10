using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class SnareGoop : GoopDefinition
	{
		public static void Init()
		{
			goopDefinition = new GoopDefinition();

			goopDefinition.CanBeIgnited = false;
			goopDefinition.damagesEnemies = false;
			goopDefinition.damagesPlayers = false;
			goopDefinition.baseColor32 = new Color32(237, 93, 239, 200);
			goopDefinition.CanBeFrozen = false;
			

			goopDefinition.goopTexture = ResourceExtractor.GetTextureFromResource("LostItems/NuclearThrone/NuclearThroneSprite/water_standard_base_001.png");

			goopDefinition.SpeedModifierEffect = item.SpeedModifierEffect;
			goopDefinition.AppliesSpeedModifier = true;
			goopDefinition.AppliesSpeedModifierContinuously = true;
		}
		static BulletStatusEffectItem item = new BulletStatusEffectItem();
		public static GoopDefinition goopDefinition;
	}
}
