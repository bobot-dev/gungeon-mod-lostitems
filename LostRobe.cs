using UnityEngine;
using ItemAPI;
using System;

namespace LostItems
{
    public class LostRobe : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Cloak of the Lost"; 

            //Refers to an embedded png in the project. Make sure to embed your sprites! Google it
            string resourceName = "LostItems/sprites/cloak"; 

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<LostRobe>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "The shadows give you power";
            string longDesc = "This cloak gives strenght to the wearing it power when they become one with darkness. \n\n A cloak once belonging to a strange treveler.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.PlaceItemInAmmonomiconAfterItemById(414);
        }

        public override void Pickup(PlayerController player)
        {
         //   player.CustomDodgeRollEffect/
            //   player.startingAlternateGunIds.Add(LostGun.gunId);
            player.startingAlternateGunIds.Add(LostGunAlt.gunId);
            player.startingGunIds.Add(LostGun.gunId);
            //player.
            player.finalFightGunIds.Clear();
            player.finalFightGunIds.Add(LostGun.gunId);           
            player.finalFightGunIds.Add(145);


            //tk2dSpriteAnimation library = spriteAnimator.Library;
           
            BulletArmorItem bulletArmor = PickupObjectDatabase.GetById(160).GetComponent<BulletArmorItem>();
            player.AlternateCostumeLibrary = bulletArmor.knightLibrary;



            base.Pickup(player);
          
        }

        float damageBuff = -1;
        bool stealthBoost = false;

        private void EnableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(84f, 6f, 107f));
            
        }

        private void DisableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));

        }

        protected override void Update()
        {
            

            bool flag = base.Owner;
            if (flag)
            {
                bool flag2 = base.Owner.IsStealthed;
                if (flag2 && stealthBoost == false)
                {
                    stealthBoost = true;
                    float curDamage = base.Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                    float newDamage = curDamage + 1f;
                    base.Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, base.Owner);
                    damageBuff = newDamage - curDamage;
                    EnableVFX(base.Owner);


                }
                else
                {
                    if (!flag2 && stealthBoost == true)
                    {
                        stealthBoost = false;
                        float curDamage = base.Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                        float newDamage = curDamage - damageBuff;
                        base.Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, base.Owner);
                        //UnityEngine.Object.Destroy(this.woop);
                        //Owner.healthHaver.gameObject.AddComponent(this.woop);
                        //Owner.healthHaver.gameObject.AddComponent(this.woop);
                        DisableVFX(base.Owner);
                    }
                }
            }
        }
    }
}
