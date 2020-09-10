using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
//demonetization
namespace LostItems
{
    class MutaionBolt : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Bolt Marrow";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Arrow_in_the_knee";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionBolt>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Homing bolts";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            homingRadius = 5f;
            homingAngularVelocity = 360f;
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.GunChanged += this.OnGunChanged;
        }

        private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
        {
            if (weaponTypes.BoltWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {
                HomingModifier homingModifier = obj.gameObject.GetComponent<HomingModifier>();
                if (homingModifier == null)
                {
                    homingModifier = obj.gameObject.AddComponent<HomingModifier>();
                    homingModifier.HomingRadius = 0f;
                    homingModifier.AngularVelocity = 0f;
                }
                float num = 1f;
                homingModifier.HomingRadius += this.homingRadius * num;
                homingModifier.AngularVelocity += this.homingAngularVelocity * num;
                
            }
        }
        public void OnGunChanged(Gun old, Gun current, bool newGun)
        {
            
            var item = this;
            if (weaponTypes.BoltWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {
                
                EnableVFX(Owner);
                if (piercing == false)
                {
                    float curPiercing = Owner.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalShotPiercing);
                    float newPiercing = curPiercing + 10f;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotPiercing, newPiercing, Owner);
                    piercingBuff = newPiercing - curPiercing;
                    piercing = true;
                }
            }
            else
            {
                
                DisableVFX(Owner);
                if (piercing == true)
                {
                    if (piercingBuff <= 0) return;
                    float curPiercing = Owner.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalShotPiercing);
                    float newPiercing = curPiercing - piercingBuff;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotPiercing, newPiercing, Owner);
                    piercingBuff = -1;
                    piercing = false;
                }
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


        public float homingRadius;
        public float homingAngularVelocity;
        bool piercing = false;
        float piercingBuff = -1;
    }
}
