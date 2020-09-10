using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
//demonetization
namespace LostItems
{
    class MutaionShell : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Shotgun Shoulders";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Shotgun_fingers-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionShell>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Shells bounce further";
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

            BounceProjModifier bounceProj = new BounceProjModifier();
            bounceProj.OnBounce += Bounce;
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.GunChanged += this.OnGunChanged;

            var item = this;
            if (ShellWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {

                EnableVFX(Owner);
                if (bounce == false)
                {
                    float curSpeed = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ProjectileSpeed);
                    float newSpeed = curSpeed + 3f;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, newSpeed, Owner);
                    speedBuff = newSpeed - curSpeed;

                    float curBounce = Owner.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces);
                    float newBounce = curBounce + 5f;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces, newBounce, Owner);
                    bounceBuff = newBounce - curBounce;
                    bounce = true;
                }
            }
            else
            {

                DisableVFX(Owner);
                if (bounce == true)
                {
                    if (bounceBuff <= 0) return;
                    float curSpeed = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ProjectileSpeed);
                    float newSpeed = curSpeed - speedBuff;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, newSpeed, Owner);
                    speedBuff = -1;

                    //if (bounceBuff <= 0) return;
                    float curBounce = Owner.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces);
                    float newBounce = curBounce - bounceBuff;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces, newBounce, Owner);
                    bounceBuff = -1;
                    bounce = false;
                }
            }
        }




        public void OnGunChanged(Gun old, Gun current, bool newGun)
        {

            var item = this;
            if (ShellWeapons.Contains(Owner.CurrentGun.PickupObjectId))
            {

                EnableVFX(Owner);
                if (bounce == false)
                {
                    float curSpeed = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ProjectileSpeed);
                    float newSpeed = curSpeed + 5f;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, newSpeed, Owner);
                    speedBuff = newSpeed - curSpeed;

                    float curBounce = Owner.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces);
                    float newBounce = curBounce + 5f;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces, newBounce, Owner);
                    bounceBuff = newBounce - curBounce;
                    bounce = true;
                }
            }
            else
            {

                DisableVFX(Owner);
                if (bounce == true)
                {
                    //if (speedBuff <= 0) return;
                    float curSpeed = Owner.stats.GetBaseStatValue(PlayerStats.StatType.ProjectileSpeed);
                    float newSpeed = curSpeed - speedBuff;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.ProjectileSpeed, newSpeed, Owner);
                    speedBuff = -1;

                    //if (bounceBuff <= 0) return;
                    float curBounce = Owner.stats.GetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces);
                    float newBounce = curBounce - bounceBuff;
                    Owner.stats.SetBaseStatValue(PlayerStats.StatType.AdditionalShotBounces, newBounce, Owner);
                    bounceBuff = -1;
                    bounce = false;
                }
            }
        }

        private void Bounce()
        {
            ETGModConsole.Log("bounce", false);
            projectile.Speed *= 10;
        }


        private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
        {
            Projectile projectile = obj;
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

        List<float> ShellWeapons = new List<float>
        {
            512,
            675,
            157,
            550,
            51,
            601,
            93,
            202,
            650,
            1,
            406,
            82,
            175,
            365,
            143,
            379,
            347,
            231,
            122,
            329,
            346,
            445,
            152,
            340,
            154,
            480
        };
        bool bounce = false;
        float bounceBuff = -1;
        float speedBuff = -1;
    }
}
