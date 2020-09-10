using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using LostItems.NuclearThrone.Character_Ablilities;
//demonetization
namespace LostItems
{
    class MutaionExplosive : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Boiling Veins";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Boiling_veins-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionExplosive>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "";
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
            if (weaponTypes.ExplosiveWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {

                EnableVFX(Owner);
            }
            else
            {
                DisableVFX(Owner);
            }

            this.player = player;


        }
        /*

        private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
        {
            explosiveModifier.explosionData = obj.GetComponent<ExplosiveModifier>().explosionData;
            if (explosiveModifier.explosionData != null)
            {

                explosiveModifier.explosionData.damageToPlayer = 10f;

                explosiveModifier.explosionData.doDamage = true;

                explosiveModifier.explosionData.ignoreList.Remove(obj.Owner.specRigidbody);


            }
        }
        */


        public void OnGunChanged(Gun old, Gun current, bool newGun)
        {
            if (weaponTypes.ExplosiveWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {
                EnableVFX(Owner);
            }
            else
            {
                DisableVFX(Owner);
            }
        }

        


        protected override void Update()
        {

            if (player.healthHaver.GetCurrentHealth() <= 2)
            {
                this.GiveFireImmunity();
            } else 
            { 
                
                RemoveFireImmunity(); 
            }
            base.Update();
        }
        private void GiveFireImmunity()
        {
            bool flag = this.fireImmune;
            if (flag)
            {
                base.Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
                this.fireImmune = false;
            }
            this.fireImmune = true;
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            base.Owner.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
        }

        private void RemoveFireImmunity()
        {
            base.Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            this.fireImmune = false;
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

        PlayerController player;

        public bool fireImmune;

        private DamageTypeModifier m_fireImmunity;

        WeaponTypes weaponTypes = new WeaponTypes();

    }
}
