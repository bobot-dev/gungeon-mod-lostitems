using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
//demonetization
namespace LostItems
{
    class MutaionSpirit : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Strong Spirit";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Strong_Spirit-export";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MutaionSpirit>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Some kills regenerate HP";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
            BuildPrefab();
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            //player.healthHaver.OnPreDeath += this.HandlePreDeath;
            // this.healingAmount = 1f;
            hasHalo = true;
            HealthHaver healthHaver = player.healthHaver;
            GameManager.Instance.OnNewLevelFullyLoaded += this.HaloCheck;
           
            healthHaver.ModifyDamage += this.ModifyIncomingDamage;

            prefab.GetComponent<tk2dBaseSprite>().SetSprite(MutaionSpirit.spriteIds[1]);
        }

        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            bool flag2 = (double)source.GetCurrentHealth() <= 0.5 && source.Armor <= 0f;
            if (flag2)
            {
                if (hasHalo == true)
                {
                    args.ModifiedDamage = 0f;
                    base.Owner.healthHaver.ApplyHealing(0.5f);
                    //  Owner.healthHaver.ApplyHealing(this.healingAmount);
                    AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", base.gameObject);
                    hasHalo = false;
                }
            }



        }
        static GameObject prefab;
        public static void BuildPrefab()
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource("LostItems/NuclearThrone/NuclearThroneSprite/halo_vfx_001", null, true);
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            GameObject gameObject2 = new GameObject("Halo");
            tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);
            MutaionSpirit.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("LostItems/NuclearThrone/NuclearThroneSprite/halo_vfx_001", tk2dSprite.Collection));
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            MutaionSpirit.spriteIds.Add(tk2dSprite.spriteId);
            gameObject2.SetActive(false);
            tk2dSprite.SetSprite(MutaionSpirit.spriteIds[0]);
            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            //MutaionSpirit.boomprefab = gameObject2;
            prefab = gameObject2;
        }

        private void ModBoom(Projectile obj)
        {
            //GameObject boomprefab1 = UnityEngine.Object.Instantiate<GameObject>(MutaionSpirit.boomprefab, obj.specRigidbody.UnitCenter, Quaternion.identity);
            //boomprefab1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(obj.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.MiddleCenter);
            //GameManager.Instance.StartCoroutine(this.HandleAttack(boomprefab1));
        }

        private IEnumerator HandleAttack(GameObject prefab)
        {
            yield return new WaitForSeconds(0.1f);
            prefab.GetComponent<tk2dBaseSprite>().SetSprite(MutaionSpirit.spriteIds[1]);
            Destroy(prefab.gameObject);
        }


        public static List<int> spriteIds = new List<int>();

        protected override void Update()
        {
            
            base.Update();
            //GameObject original;
            //original = MutaionSpirit.itemVFXPrefab;
       
            //tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, Owner.transform).GetComponent<tk2dSprite>();
            //component.name = MutaionSpirit.vfxName;
            //component.PlaceAtPositionByAnchor(Owner.sprite.WorldTopCenter + this.offset, tk2dBaseSprite.Anchor.LowerCenter);
            //component.scale = Vector3.zero;
        }

        /*public static void BuildPrefab()
        {
           
            MutaionSpirit.itemVFXPrefab = SpriteBuilder.SpriteFromResource(MutaionSpirit.itemVFX, null, false);
           
            MutaionSpirit.itemVFXPrefab.name = MutaionSpirit.vfxName;
          
            UnityEngine.Object.DontDestroyOnLoad(MutaionSpirit.itemVFXPrefab);
            FakePrefab.MarkAsFakePrefab(MutaionSpirit.itemVFXPrefab);
            MutaionSpirit.itemVFXPrefab.SetActive(false);


            /*
             MutaionSpirit.haloVFXPrefab = SpriteBuilder.SpriteFromResource(MutaionSpirit.haloVFX, null, false);

            MutaionSpirit.haloVFXPrefab.name = MutaionSpirit.vfxName;

            UnityEngine.Object.DontDestroyOnLoad(MutaionSpirit.haloVFXPrefab);
            FakePrefab.MarkAsFakePrefab(MutaionSpirit.haloVFXPrefab);
            MutaionSpirit.haloVFXPrefab.SetActive(false);*/


       // }

        public void HaloCheck()
        {
            if (hasHalo == false && Owner.healthHaver.GetCurrentHealthPercentage() >= 1)
            {
                hasHalo = true;
                prefab.GetComponent<tk2dBaseSprite>().SetSprite(MutaionSpirit.spriteIds[1]);
            }
        }



       // private static string gunVFX = "FrostAndGunfireItems/Resources/vfx_stuff/gun_vfx_001";


        private static string itemVFX = "LostItems/NuclearThrone/NuclearThroneSprite/halo_vfx_001";


        private static string vfxName = "HaloVFX";

        private static GameObject itemVFXPrefab;

        bool hasHalo;
        public float healingAmount;

      //  private static string vfxName = "HaloVFX";
       // private static string haloVFX = "LostItems/NuclearThrone/NuclearThroneSprite/halo_vfx_001";
        private Vector2 offset = new Vector2(0f, 0.25f);
        private static GameObject haloVFXPrefab;
    }
}
