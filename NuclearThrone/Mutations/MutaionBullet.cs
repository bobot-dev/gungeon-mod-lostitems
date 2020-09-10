using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using LostItems.NuclearThrone.Weapons;
//demonetization
namespace LostItems
{
    class MutaionBullet : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Rycycle Gland";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Vomit-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionBullet>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Most hit bullets become ammo";
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

            //ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
            //explosiveModifier.explosionData = (PickupObjectDatabase.GetById(player.CurrentGun.PickupObjectId) as Gun).DefaultModule.projectiles[0].GetComponent<ExplosiveModifier>().explosionData;


            var item = this;
            if (player.CurrentGun.encounterTrackable.journalData.GetNotificationPanelDescription() == NTWeaponHelper.BulletDesc)
            {

                EnableVFX(Owner);
            }
            else
            {
                DisableVFX(Owner);
            }

            this.player = player;


        }



        public void OnGunChanged(Gun old, Gun current, bool newGun)
        {
            if (player.CurrentGun.encounterTrackable.journalData.GetNotificationPanelDescription() == NTWeaponHelper.BulletDesc)
            {
                EnableVFX(Owner);
                GunExt.SetName(player.CurrentGun, "yes");
            }
            else
            {
                DisableVFX(Owner);
                GunExt.SetName(player.CurrentGun, "no");
            }
        }

        private void EnableVFX(PlayerController Owner)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 1f, 0f));
        }

        private void DisableVFX(PlayerController Owner)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }


        protected override void Update()
        {


            base.Update();
        }

        PlayerController player;

        WeaponTypes weaponTypes = new WeaponTypes();
    }
}
