using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace LostItems
{
    class RetoStarterOld : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Gungeonpop";

            //Refers to an embedded png in the project. Make sure to embed your sprites! Google it.
            string resourceName = "LostItems/sprites/retostarter";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<RetoStarterOld>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Banger Supreme!";
            string longDesc = "Temporarily increases stats and grants flight while active. (due to health and safty conserns flight is currently unavalable) \n\n This album made by the famous Gungeon band 'Bulletkin 65' includes several certified Banger Supremes, such as 'I'm Reloading' and 'My Gunsoul' is said to grant immense power upon playing it.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "yt-reto");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 300);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float damageBuff = -1;
        float kbBuff = -1;
        float accuracyBuff = -1;
        float speedBuff = -1;
        float fireRateBuff = -1;
        float reloadBuff = -1;
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_WPN_guitar_shot_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        //Doubles the damage, makes the next shot kill the player, and stores the amount we buffed the player for later
        private void StartEffect(PlayerController user)
        {
            float curDamage = user.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
            float newDamage = curDamage + 0.5f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, user);
            damageBuff = newDamage - curDamage;

            float curKB = user.stats.GetBaseStatValue(PlayerStats.StatType.KnockbackMultiplier);
            float newKB = curKB + 1f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.KnockbackMultiplier, newKB, user);
            kbBuff = newKB - curKB;

            float curAccuracy = user.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy);
            float newAccuracy = curAccuracy + 0.25f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, newAccuracy, user);
            accuracyBuff = newAccuracy - curAccuracy;

            float curSpeed = user.stats.GetBaseStatValue(PlayerStats.StatType.MovementSpeed);
            float newSpeed = curSpeed + 2f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, newSpeed, user);
            speedBuff = newSpeed - curSpeed;

            float curFireRate = user.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float newFireRate = curFireRate + 0.25f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, user);
            fireRateBuff = newFireRate - curFireRate;

            float curReload = user.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
            float newReload = curReload - 0.25f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload, user);
            reloadBuff = newReload - curReload;

            //user.SetIsFlying(true, "wings", true, false);

            EnableVFX(user);
        }

        private void EnableVFX(PlayerController user)
        {
            //Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            //outlineMaterial.SetColor("_OverrideColor", new Color(76f, 252f, 252f));
            //Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            //outlineMaterial.SetColor("_OverrideColor", new Color(0.3f, 0.988f, 0.988f));
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0.337f, 0.855f, 0.961f));
        }

        private void DisableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }


        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
            float curDamage = user.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
            float curKB = user.stats.GetBaseStatValue(PlayerStats.StatType.KnockbackMultiplier);
            float curAccuracy = user.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy);
            float curSpeed = user.stats.GetBaseStatValue(PlayerStats.StatType.MovementSpeed);
            float curFireRate = user.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float curReload = user.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);

            float newDamage = curDamage - damageBuff;
            float newKB = curKB - kbBuff;
            float newAccuery = curAccuracy - accuracyBuff;
            float newSpeed = curSpeed - speedBuff;
            float newFireRate = curFireRate - fireRateBuff;
            float newReload = curReload - reloadBuff;

            user.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, user);
            user.stats.SetBaseStatValue(PlayerStats.StatType.KnockbackMultiplier, newKB, user);
            user.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, newAccuery, user);
            user.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, newSpeed, user);
            user.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, user);
            user.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload, user);
            damageBuff = -1;
            kbBuff = -1;
            accuracyBuff = -1;
            speedBuff = -1;
            fireRateBuff = -1;
            reloadBuff = -1;
            AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);

            //user.SetIsFlying(false, "wings", true, false);

            DisableVFX(user);
        }
        
        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
