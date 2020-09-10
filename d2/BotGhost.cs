using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace LostItems.d2
{
    public class BotGhost : PassiveItem
    {
        private GameObject m_extantOrbital;
        private object OrbitalPrefab;
        private object OrbitalFollowerPrefab;

        public static void Init()
        {
            //The name of the item
            string itemName = "Ghost";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Laser_brain-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BotGhost>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Little Light";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }

        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
        }

        private void CreateOrbital(PlayerController owner)
        {
            GameObject extantOrbital = UnityEngine.Object.Instantiate<GameObject>((!(this.OrbitalPrefab != null)) ? this.OrbitalFollowerPrefab.gameObject : this.OrbitalPrefab.gameObject, owner.transform.position, Quaternion.identity);
            this.m_extantOrbital = extantOrbital;
            if (this.OrbitalPrefab != null)
            {
                this.m_extantOrbital.GetComponent<PlayerOrbital>().Initialize(owner);
            }
            else if (this.OrbitalFollowerPrefab != null)
            {
                this.m_extantOrbital.GetComponent<PlayerOrbitalFollower>().Initialize(owner);
            }
        }

    }
}
