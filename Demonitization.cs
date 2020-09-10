using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class Demonitization : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Demonetization";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/sprites/demonetization";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<Demonitization>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "wip";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        public override void Pickup(PlayerController player)
        {
            player.OnKilledEnemy += this.OnKill;
            base.Pickup(player);
        }

        private void OnKill(PlayerController player)
        {
            //EnableShader(this.Owner);

        }

        private void ProcessGunShader(Gun g)
        {
            MeshRenderer component = g.GetComponent<MeshRenderer>();
            if (!component)
            {
                return;
            }
            Material[] sharedMaterials = component.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i].shader == this.m_glintShader)
                {
                    return;
                }
            }
            Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
            Material material = new Material(this.m_glintShader);
 
            //material.SetColor("_OverrideColor", new Color(252f, 204f, 45f));
            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;
        }
        /*
        protected void EnableShader(PlayerController user)
        {
            if (!user)
            {
                return;
            }
            Material[] array = user.SetOverrideShader(ShaderCache.Acquire("Brave/PlayerShaderEevee"));
            for (int i = 0; i < array.Length; i++)
            {
                if (!(array[i] == null))
                {
                    array[i].SetFloat("_AllColorsToggle", 1f);
                }
            }
        }*/
       // public tk2dSpriteAnimation RatAnimationLibrary;
        //private PlayerController m_lastPlayer;
        private Shader m_glintShader;
    }
}
