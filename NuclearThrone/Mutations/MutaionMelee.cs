using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionMelee : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Long Arms";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Long_Arms2-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionMelee>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "More melee range";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            //player.PostProcessProjectile += this.PostProcessProjectile;
            player.GunChanged += this.OnGunChanged;

            var item = this;
            if (weaponTypes.MeleeWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {

                EnableVFX(Owner);
               
            }
            else
            {

                DisableVFX(Owner);
            }
        }



        public void OnGunChanged(Gun old, Gun current, bool newGun)
        {

            var item = this;
            if (weaponTypes.MeleeWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {

                EnableVFX(Owner);

            }
            else
            {

                DisableVFX(Owner);
            }
        }
        private void EnableVFX(PlayerController Owner)
        {
            //Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            //outlineMaterial.SetColor("_OverrideColor", new Color(76f, 252f, 252f));
            //Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            //outlineMaterial.SetColor("_OverrideColor", new Color(0.3f, 0.988f, 0.988f));
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 1f, 0f));
        }

        private void DisableVFX(PlayerController Owner)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }

        WeaponTypes weaponTypes = new WeaponTypes();

    }
}
