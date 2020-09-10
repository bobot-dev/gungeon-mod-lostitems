using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MultiplayerBasicExample;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;

namespace LostItems
{
    public class NuclearTalisman : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Nuclear Talisman";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/Nuclear Talisman2";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<NuclearTalisman>();
            

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "The Throne";
            string longDesc = "wip";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 0.4f, StatModifier.ModifyMethod.ADDITIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.CanBeDropped = false;
        }
        public override void Pickup(PlayerController player)
        {



            player.inventory.maxGuns.Equals(2);
            player.MAX_GUNS_HELD = 2;
            player.inventory.maxGuns = 2;

            Hook hook2 = new Hook(typeof(Chest).GetMethod("orig_Open", BindingFlags.Instance | BindingFlags.NonPublic), typeof(NuclearTalisman).GetMethod("OnOpen"));
            if (this.m_pickedUp)
            {
                return;
            }
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                GameManager.Instance.AllPlayers[i].carriedConsumables.InfiniteKeys = true;
            }

            player.OnRoomClearEvent += this.HandleRoomCleared;

            base.Pickup(player);














        }

        protected override void Update()
        {
            PlayerController player = Owner as PlayerController;
            if (player.inventory.AllGuns.Count <= 2)
            {
               this.currentGun = player.CurrentGun;
                //currentGun.HasEverBeenAcquiredByPlayer = true;
                //player.inventory.RemoveGunFromInventory(currentGun);
                //currentGun.DropGun(0.5f);
            } else if (player.inventory.AllGuns.Count >= 3)
            {
                //Gun currentGun = player.CurrentGun;
                this.currentGun.HasEverBeenAcquiredByPlayer = true;
                player.inventory.RemoveGunFromInventory(this.currentGun);
                this.currentGun.DropGun(0.5f);
            }

            PlayerItem item = LootEngine.GetItemOfTypeAndQuality<PlayerItem>(ItemQuality.D, GameManager.Instance.RewardManager.ItemsLootTable, true); 
            base.Update();
            bool flag = base.Owner;
            if (flag)
            {
                this.HandleRads(base.Owner);
            }

        }

        // Token: 0x060000CC RID: 204 RVA: 0x0000B2A4 File Offset: 0x000094A4
        public static void OnOpen(Action<Chest, PlayerController> orig, Chest self, PlayerController player)
        {
            if (player.HasPickupID(ETGMod.Databases.Items["Nuclear Talisman"].PickupObjectId))
            {
                int i = 0;
                List<int> list4 = new List<int>()
                {
                    74
                };
                List<PickupObject> list3 = self.PredictContents(player);
                foreach (PickupObject pickupObject2 in list3)
                {

                    if (GameManager.Instance.RewardManager.ItemsLootTable.RawContains(pickupObject2.gameObject))
                    {

                        //i++;
                        self.contents = null;
                        self.forceContentIds = list4;
                        //self.forceContentIds.Add(74);

                        // self.contents.Add(PickupObjectDatabase.GetById(74));
                       // ETGModConsole.Log("nope not today");
                    }
                    //.PickupObjectId = () ;
                }
                
            }
            orig(self, player);

        }




            public void HandleRads(PlayerController player)
        {
            if (player.carriedConsumables.Currency > 0)
            {
                player.carriedConsumables.Currency -= 1;
                Rads += 1;

                if (Rads == 1)
                {
                    string RadsMsg = "You currently have " + Rads + " Rad";
                    ETGModConsole.Log(RadsMsg);

                } else
                {
                    string RadsMsg = "You currently have " + Rads + " Rads";
                    ETGModConsole.Log(RadsMsg);
                }

                if (Rads >= RadsToLevelUp * Level && Level < 10)
                {
                    
                    string header = "LEVEL UP";
                    if (Level == 10)
                    {
                        string text = "level ULTRA";
                        this.Notify(header, text);
                    }
                    else
                    {
                        string text = "level " + Level;
                        this.Notify(header, text);
                    }

                    needsToSpawnMuts = true;

                    Rads -= RadsToLevelUp * Level;
                    Level += 1;

                }
            }
        }

        private void HandleRoomCleared(PlayerController obj)
        {

            if (needsToSpawnMuts == true)
            {
                ChooseMutaions();
            }

        }


        private void Notify(string header, string text)
        {
            //tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            //tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            //tk2dBaseSprite notificationObjectSprite = 
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName("LostItems/NuclearThrone/NuclearThroneSprite/Nuclear Talisman2");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, encounterIconCollection, spriteIdByName, UINotificationController.NotificationColor.PURPLE, false, false);

            //GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.PURPLE, true, false);
        }

        private void ChooseMutaions ()
        {
            MutationNumber = UnityEngine.Random.Range(1, 29);
            switch (MutationNumber)
            {
                case 1:
                    MutToSpawn = "Boiling Veins";
                    break;
                case 2:
                    MutToSpawn = "Rycycle Gland";
                    break;
                case 3:
                    MutToSpawn = "Shotgun Shoulders";
                    break;
                case 4:
                    MutToSpawn = "Laser Brain";
                    break;
                case 5:
                    MutToSpawn = "Homing bolts";
                    break;
                case 6:
                    MutToSpawn = "Impact Wrists";
                    break;
                case 7:
                    MutToSpawn = "";
                    break;
                case 8:
                    MutToSpawn = "Trigger Fingers";
                    break;
                case 9:
                    MutToSpawn = "Sharp Teeth";
                    break;
                case 10:
                    MutToSpawn = "Stress";
                    break;
                case 11:
                    MutToSpawn = "Second Stomach";
                    break;
                case 12:
                    MutToSpawn = "Strong Spirit";
                    break;
                case 13:
                    MutToSpawn = "Rhino Skin";
                    break;
                case 14:
                    MutToSpawn = "Rabbit Paw";
                    break;
                case 15:
                    MutToSpawn = "Back Muscle";
                    break;
                case 16:
                    MutToSpawn = "Open Mind";
                    break;
                case 17:
                    MutToSpawn = "Long Arms";
                    break;
                case 18:
                    MutToSpawn = "Lucky Shot";
                    break;
                case 19:
                    MutToSpawn = "Gamma Guts";
                    break;
                case 20:
                    MutToSpawn = "Extra Feet";
                    break;
                case 21:
                    MutToSpawn = "Scarier Face";
                    break;
                case 22:
                    MutToSpawn = "Eagle Eyes";
                    break;
                case 23:
                    MutToSpawn = "Euphoria";
                    break;
                case 24:
                    MutToSpawn = "";
                    break;
                case 25:
                    MutToSpawn = "";
                    break;
                default:
                    MutToSpawn = "Last Wish";
                    break;

            }
            SpawnMutaions(MutToSpawn);

            
        }


        private void SpawnMutaions(string mut)
        {
            if (!Owner.HasPickupID(ETGMod.Databases.Items[mut].PickupObjectId))
            {
                LootEngine.SpawnItem(ETGMod.Databases.Items[mut].gameObject, Owner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                needsToSpawnMuts = false;
            } 
            else
            {
                ChooseMutaions();
            }

           
        }

        float MutationNumber = 0;
        float Rads = 0;
        float RadsToLevelUp = 30;
        float Level = 1;
        private Chest nearbyChest;
        private List<Chest> encounteredChests = new List<Chest>();
        private PlayerController player;
        string MutToSpawn;
        Gun currentGun;
        public bool needsToSpawnMuts = false;
    }
}
