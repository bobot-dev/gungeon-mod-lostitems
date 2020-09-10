using System;
using System.Reflection;
using System.Collections.Generic;
using GungeonAPI;
using ItemAPI;
using MonoMod.RuntimeDetour;
using LostItems.NuclearThrone.Character_Ablilities;
using System.IO;
using LostItems.NuclearThrone;
using LostItems.NuclearThrone.Weapons.Bullet;
using EnemyAPI;
using LostItems.d2;

namespace LostItems
{
    public class LostItemsMod : ETGModule
    {
        public static readonly string MOD_NAME = "Lost Item Pack";
        public static readonly string VERSION = "2.2.1";
        public static readonly string TEXT_COLOR = "#6e0696";
        public static readonly string TEXT_COLOR_GOOD = "#00ff3c";
        public static readonly string TEXT_COLOR_BAD = "#eb1313";
        public static readonly string TEXT_COLOR_NT = "#1ee000";
        public override void Init()
        {

        }

        public override void Start()
        {
            try
            {
                GungeonAP.Init();

                ETGModConsole.Commands.AddUnit("botflow", delegate (string[] args)
                {
                    DungeonHandler.debugFlow = !DungeonHandler.debugFlow;
                    string str = DungeonHandler.debugFlow ? "enabled" : "disabled";
                    string color = DungeonHandler.debugFlow ? "00FF00" : "FF0000";
                    LostItemsMod.Log("Debug flow " + str, color);
                });
            }
            catch (Exception e)
            {
                LostItemsMod.Log("Command Broke", TEXT_COLOR_BAD);
                LostItemsMod.Log(string.Format(e + ""), TEXT_COLOR_BAD);
            }

            try
            {
                //GungeonAP.Init();
                //NuclearShrine.Add();
                FakePrefabHooks.Init();
                ItemBuilder.Init();
                ShrineFactory.Init();
                GungeonAPI.Tools.Init();

                EnemyAPI.Tools.Init();
                EnemyAPI.Hooks.Init();

                //CustomSynergiesBaseGame.Init();

                hooks.Init();

                //MoreBreachShrine.Add();
                //BreachRoomShrine.Add();

                // ConfigManager sta = new ConfigManager();
                // GunConfigManager gcf = new GunConfigManager();

                //sta.Init();
                //gcf.Init();

                //CustomGun.Add();

                //GunBuffs.Init();

                // MagicSpriteStuff.Init();

                //Ping.Init();

                NuclearTalisman.Init();//potato

                BotGhost.Init();//potato

                LostGun.Add();//1 change sounds

                //SaiGun.Add();//2 book sprite

                LostOrb.Init();//3 sprite is boring

                LostRobe.Init();//4

                RoboticMuncher.Init();//5 

                JunkHeart.Init();//6 i think this is done

                LootBox.Init();//7 

                CNerfGun.Add();//8 pretty much everything

                //MopController.Add();//9 fix charge animation, add water projectile

                CoopTest.Init();//10 pretty much everything

                Bond.Init();//11 rework or remove

                Bob.Init();//12 this ones just a joke

                Children.Init();//13 maybe add some nice vfx

                BlessedOrb.Init();//14 

                BanGun.Add();//15 disintergration effect, new projectile sprite?

                RetoStarter.Init();//16 add flight and knockback, lessen the light it gives off

                RetoStarterOld.Init();//17 add flight and knockback, lessen the light it gives off

                Test.Init();//18 add new items when i get to making them

                MistakeCharm.Init();//19 i think this one is fine

                Revenge.Init();//21

                //CNerfGun2.Add();//22

                //ETGModConsole.Log("up to pet works");

                BabyGoodMistake.Init();//20 make the synergy work

                //Demonitization.Init(); //23

               // syntest.Add(); //24 

                //syntest2.Add(); //25 

                CosmicSludge.Init(); // 26

                //StarterGun.Add(); // 27

                ScatedyCat.Init(); // 28

                Shine.Init(); //29

                Kunia.Add();//30

                BalencePatch.Init();//31

                ChlorophyteRounds.Init();//32

                LostGunAlt.Add();//33

                Apache.Add();

                //TestGun.Add();//

                TestActive.Init();//35
                                  //RageRifle.Add();//34
                TestPassive.Init();//36

                Gundertale.Add();

                MyItem.Init();

                RuinousEffigy.Add();

                Log($"Begining to load Nuclear Throne related stuff...", TEXT_COLOR_NT);
                //2.1



                // NuclearTalisman.Init();

                MutaionBolt.Init();//1

                MutaionBullet.Init();//2``

                MutaionEnergy.Init();//3``

                MutaionShell.Init();//4``

                MutaionMelee.Init();//5``

                MutaionStress.Init();//6

                MutaionTriggerFingers.Init();//7

                MutaionTeeth.Init();//8

                MutaionFace.Init();//9

                MutaionSkin.Init();//10

                MutaionEyes.Init();//11

                MutaionMuscle.Init();//12

                MutaionFeet.Init();//13

                MutaionPaw.Init();//14

                MutaionWish.Init();//15

                MutaionEuphoria.Init();//16

                MutaionWrists.Init();//17

                MutaionBlood.Init();//18

                MutaionStomach.Init();//19

                MutaionLucky.Init();//20

                MutaionExplosive.Init();//21

                MutaionGuts.Init();//22

                MutaionMind.Init();//23

                MutaionSpirit.Init();//24

                MutaionWait.Init();//25

                //Tools.Print<string>("Did Start()", "00FF00", false);
                //NuclearShrine.Add();

                SnareGoop.Init();

                AbilityMelting.Init();//1

                AbilityRobot.Init();//2

                AbilitySteroids.Init();//3

                AbilityRogue.Init();//4

                AbilityRebel.Init();//5

                AbilityCrystal.Init();//6

                AbilityPlant.Init();//7

                AbilityYV.Init();//8

                AbilityEyes.Init(); //9

                AbilityChicken.Init();

                // RebelGun.Add();//1

                CharacterShrine.Add();//2


                Revolver.Add();

                AssaultRifle.Add();

                SmartGun.Add();

                //Katana.Init();


                Directory.CreateDirectory("NTConfig");
                bool flag = !File.Exists("NTsConfig/protochest.json");
                if (flag)
                {
                    File.WriteAllText("NTConfig/protochest.json", "1");
                }
               // bool flag2 = !LostItemsMod.AvailableNailModes.Contains(File.ReadAllText("NTConfig/nailmode.json"));
               // if (flag2)
                //{
                   // File.WriteAllText("NTConfig/protochest.json", "normal");
               // }
                LostItemsMod.ProtoChestContent = File.ReadAllText("NTConfig/protochest.json");

                ProtoChest.Add();

                ShrineFactory.PlaceBreachShrines();


                ETGModConsole.Commands.AddGroup("nt", delegate (string[] args)
                {
                    ETGModConsole.Log("shoop has my family", false);
                });

                ETGModConsole.Commands.GetGroup("nt").AddUnit("level", delegate (string[] args)
                {
                    header = "test";
                    text = "test";

                    Notify(header, text);
                });

                ETGModConsole.Commands.GetGroup("nt").AddUnit("character", delegate (string[] args)
                {
                    header = NuclearShrine.header;
                    text = "Character";

                    Notify(header, text);
                });

                ETGModConsole.Commands.GetGroup("nt").AddUnit("protochest", delegate (string[] args)
                {
                    LostItemsMod.Log("Current stored gun has the id " + ProtoChest.StoredGunID, LostItemsMod.TEXT_COLOR_NT);
                });



                LostItemsMod.Log(LostItemsMod.MOD_NAME + " v" + LostItemsMod.VERSION + " started successfully.", LostItemsMod.TEXT_COLOR);
                LostItemsMod.Log("It worked", LostItemsMod.TEXT_COLOR_GOOD);
            }
            catch (Exception arg)
            {
                LostItemsMod.Log(string.Format(LostItemsMod.MOD_NAME + " v" + LostItemsMod.VERSION + " Failed to load ", LostItemsMod.MOD_NAME, LostItemsMod.VERSION, arg), LostItemsMod.TEXT_COLOR_BAD);
                LostItemsMod.Log("it did not work", LostItemsMod.TEXT_COLOR_BAD);
            }
        }
        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>", false);
        }

        public override void Exit()
        {
        }

        private static void Notify(string header, string text)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName("LostItems/NuclearThrone/NuclearThroneSprite/Nuclear Talisman2");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, encounterIconCollection, spriteIdByName, UINotificationController.NotificationColor.GOLD, false, true);
        }

        static string header = "-";
        static string text = "-";
        public static string ProtoChestContent = "1";
        

        //public static void Log(string text, string color = "FFFFFF")
        //{
        //    ETGModConsole.Log($"<color={color}>{text}</color>");
        //}

        //Log($"Nuclear Throne related stuff loaded succsesfully.", "#48fd08");
        /*
        List<string> mandatoryConsoleIDs = new List<string>
        {
            "bot:mistake_charm",
            "bot:ministake"

        };
        CustomSynergies.Add("Two negetives can make a positive", mandatoryConsoleIDs, null, true);
        //CustomSynergies.Add("Two negetives can make a positive", mandatoryConsoleIDs, null, true);

        /*List<string> mandatoryConsoleIDs2 = new List<string>
        {
            "clone",
            "gun_soul"

        };
        CustomSynergies.Add("DROP GUN SOUL!", mandatoryConsoleIDs2, null, true);

        List<string> mandatoryConsoleIDs3 = new List<string>
        {
            "full_metal_jacket",
            "elder_blank"

        };
        CustomSynergies.Add("Elder Vest", mandatoryConsoleIDs3, null, true);

        Hook ShellBounceShellBounce2 = new Hook(
        typeof(BounceProjModifier).GetMethod("Bounce", BindingFlags.Instance | BindingFlags.Public),
        typeof(LostItemsMod).GetMethod("ShellBounce2")
        );
        */
        //Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);

    }


    // Token: 0x060000C9 RID: 201 RVA: 0x00009F1C File Offset: 0x0000811C


}

