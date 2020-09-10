using ItemAPI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LostItems
{
	public static class MagicSpriteStuff
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00013060 File Offset: 0x00011260
		public static void Init()
		{

            //newItem.texture = ResourceExtractor.GetTextureFromResource("LostItems/sprites/Shadow_Relic_001.png");

            //adds an element to the clips array
            Array.Resize(ref GameUIRoot.Instance.ammoControllers[0].ammoTypes, GameUIRoot.Instance.ammoControllers[0].ammoTypes.Length + 1);
            int last_memeber = GameUIRoot.Instance.ammoControllers[0].ammoTypes.Length - 1;
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber] = new GameUIAmmoType();
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoType = new GameUIAmmoType.AmmoType();
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarBG = new dfTiledSprite();
            //the sprites,  bg is empty clip and fg is full clip
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarFG = new dfTiledSprite();
            //has to be custom (duh)
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            //the name by which the gun defaulte module refers to this whole thing
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].customAmmoType = "ExampleUIClip";
            //names of your sprites, doesnt really matter
            GameObject ExampleBG = new GameObject("Shadow_Relic_001");
            GameObject ExampleFG = new GameObject("Shadow_Relic_001");
            //neeeded so proper tranform will exist and unity wont complain
            ExampleBG.AddComponent<dfTiledSprite>(GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarBG);
            ExampleFG.AddComponent<dfTiledSprite>(GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarFG);
            //absolutly no idea why thats actually needed but it is
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarBG = ExampleBG.GetComponent<dfTiledSprite>();
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarFG = ExampleFG.GetComponent<dfTiledSprite>();
            //name of the sprite in the dfAtlas, thats how it determines sprites. you can either use existing ones or edit the atlas if you want
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarBG.SpriteName = "testeru tex";
            GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber].ammoBarFG.SpriteName = "gatling_gull_muscles";
        }
       
    }
}
