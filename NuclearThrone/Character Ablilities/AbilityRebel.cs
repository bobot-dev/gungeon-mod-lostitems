using System.Collections.Generic;
using UnityEngine;
using ItemAPI;
using System.Linq;
using Dungeonator;
using System;

namespace LostItems.NuclearThrone.Character_Ablilities
{
	class AbilityRebel : ActiveSummonItem
	{
		//AIShooter aiShooter;


		PlayerController player;
		//Call this method from the Start() method of your ETGModule extension
		//public static GameObject allyPrefab;
		//private static readonly string guid = "Ally69420hahaha"; //give your companion some unique guid
		//float damageBuff = -1;
		//public string allyPrefab;
		public static void Init()
		{
			//The name of the item
			string itemName = "Spawn Ally";

			//Refers to an embedded png in the project. Make sure to embed your resources! Google it
			string resourceName = "LostItems/NuclearThrone/NuclearThroneSprite/CharacterIcons/Rebel_icon1-export";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<AbilityRebel>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "abiliy of rebel";
			string longDesc = "wip";

			//Adds the item to the gungeon item list, the ammonomicon, the loot ally, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot_nt");

			//Adds the actual passive effect to the item
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.SPECIAL;
			item.CanBeDropped = false;


			item.CompanionGuid = AbilityRebel.guid;
			AbilityRebel.BuildPrefab();
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			spawnDamage = 0.5f;
			player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));

			BlinkPassiveItem Scarf = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();
			this.ScarfPrefab =  Scarf.ScarfPrefab;

			//ETGModConsole.Log( + "", false);
			Shader.Find("_OverrideColor");
			Color ScarfColor = new Color(0.23529411764f, 0.30196078431f, 0.92549019607f);
			ScarfPrefab.ScarfMaterial.SetColor("_OverrideColor", ScarfColor);
			//ScarfPrefab.ScarfMaterial.SetColor("_Color", Color.black);
			ScarfPrefab.ScarfLength /= 2;
			//ScarfPrefab.ScarfMaterial.GetColor
			//ScarfPrefab.ScarfMaterial.mainTexture;
			//this.m_scarf = Scarf.m_scarf;

			if (this.ScarfPrefab)
			{
				this.m_scarf = UnityEngine.Object.Instantiate<GameObject>(this.ScarfPrefab.gameObject).GetComponent<ScarfAttachmentDoer>();
				this.m_scarf.Initialize(player);
			}
		}

		        

		public ScarfAttachmentDoer ScarfPrefab;
		private ScarfAttachmentDoer m_scarf;

		protected override void DoEffect(PlayerController user)
		{
			if (user.healthHaver.GetCurrentHealth() >= 1)
			{
				user.healthHaver.ApplyDamage(spawnDamage, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, true, null, true);
				CreateNewCompanion(user);
			}

		}

		public override bool CanBeUsed(PlayerController user)
		{
			if (user.healthHaver.GetCurrentHealth() >= 1)
			{
				return true;
			} 
			else
			{
				return false;
			}
			
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00015670 File Offset: 0x00013870
		private void CreateNewCompanion(PlayerController player)
		{
			//AkSoundEngine.PostEvent("Play_OBJ_smallchest_spawn_01", base.gameObject);
			bool flag = this.companionsSpawned.Count + 1 == this.MaxNumberOfCompanions;
			bool flag2 = flag;
			bool flag3 = !flag2;
			if (flag3)
			{
				bool flag4 = this.companionsSpawned.Count >= this.MaxNumberOfCompanions;
				bool flag5 = !flag4;
				bool flag6 = flag5;
				if (flag6)
				{
					AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
					Vector3 vector = player.transform.position;
					bool flag7 = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
					bool flag8 = flag7;
					bool flag9 = flag8;
					if (flag9)
					{
						vector += new Vector3(1.125f, -0.3125f, 0f);
					}
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
					CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
					this.companionsSpawned.Add(orAddComponent);
					orAddComponent.Initialize(player);
					bool flag10 = orAddComponent.specRigidbody;
					bool flag11 = flag10;
					bool flag12 = flag11;
					if (flag12)
					{
						PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
					}
					orAddComponent.healthHaver.OnDeath += this.OnDeath;
					orAddComponent.aiAnimator.PlayUntilFinished("spawn", false, null, -1f, false);
				}
			}
		}
		float spawnDamage;
		// Token: 0x06000236 RID: 566 RVA: 0x000157D4 File Offset: 0x000139D4


		// Token: 0x06000237 RID: 567 RVA: 0x00015936 File Offset: 0x00013B36
		private void OnDeath(Vector2 v)
		{
			this.removeDeadCompanions();
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00015940 File Offset: 0x00013B40
		private void removeDeadCompanions()
		{
			for (int i = this.companionsSpawned.Count - 1; i >= 0; i--)
			{
				bool flag = !this.companionsSpawned[i];
				bool flag2 = flag;
				bool flag3 = flag2;
				if (flag3)
				{
					this.companionsSpawned.RemoveAt(i);
				}
				else
				{
					bool flag4 = this.companionsSpawned[i].healthHaver && this.companionsSpawned[i].healthHaver.IsDead;
					bool flag5 = flag4;
					bool flag6 = flag5;
					if (flag6)
					{
						UnityEngine.Object.Destroy(this.companionsSpawned[i].gameObject);
						this.companionsSpawned.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00015A0C File Offset: 0x00013C0C
		private void DestroyCompanions()
		{
			for (int i = this.companionsSpawned.Count - 1; i >= 0; i--)
			{
				bool flag = this.companionsSpawned[i];
				bool flag2 = flag;
				bool flag3 = flag2;
				if (flag3)
				{
					UnityEngine.Object.Destroy(this.companionsSpawned[i].gameObject);
				}
				this.companionsSpawned.RemoveAt(i);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00015A7C File Offset: 0x00013C7C
		private void HandleNewFloor(PlayerController player)
		{
			int count = this.companionsSpawned.Count;
			this.DestroyCompanions();
			for (int i = 0; i < count; i++)
			{
				this.CreateNewCompanion(player);
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00015AB8 File Offset: 0x00013CB8
		//public override DebrisObject Drop(PlayerController player)
		//{
		//	player.OnAllyFlipCompleted = (Action<FlippableCover>)Delegate.Remove(player.OnAllyFlipCompleted, new Action<FlippableCover>(this.HandleFlip));
		//	player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		///	this.DestroyCompanions();
		//	return base.Drop(player);
		//}

		// Token: 0x0600023C RID: 572 RVA: 0x00015B1C File Offset: 0x00013D1C
		public static void BuildPrefab()
		{
			bool flag = AbilityRebel.AllyPrefab != null || CompanionBuilder.companionDictionary.ContainsKey(AbilityRebel.guid);
			bool flag2 = flag;
			bool flag3 = !flag2;
			if (flag3)
			{


				AbilityRebel.AllyPrefab = CompanionBuilder.BuildPrefab("Ally Boi", AbilityRebel.guid, AbilityRebel.spritePaths[0], new IntVector2(3, 2), new IntVector2(8, 9));
				AbilityRebel.AllyBehavior allyBehavior = AbilityRebel.AllyPrefab.AddComponent<AbilityRebel.AllyBehavior>();
				allyBehavior.aiAnimator.facingType = AIAnimator.FacingType.Movement;
				allyBehavior.aiAnimator.faceSouthWhenStopped = false;
				allyBehavior.aiAnimator.faceTargetWhenStopped = true;

				AIAnimator aiAnimator = allyBehavior.aiAnimator;
				//AIBulletBank aiBulletBlank = aiBulletBlank.
				//allyBehavior.aiShooter.equippedGunId = 79;

				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "run",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "die",
						anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.Single,
							Prefix = "die",
							AnimNames = new string[1],
							Flipped = new DirectionalAnimation.FlipType[1]
						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left"
					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};

				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle_right",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};

				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "attack",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack",
						anim = anim
					}
				};
				AIAnimator.NamedDirectionalAnimation namedDirectionalAnimation = new AIAnimator.NamedDirectionalAnimation();
				namedDirectionalAnimation.name = "spawn";
				namedDirectionalAnimation.anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "spawn",
					AnimNames = new string[]
					{
						"spawn"
					},
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				bool flag4 = AbilityRebel.AllyBoiCollection == null;
				bool flag5 = flag4;
				if (flag5)
				{
					AbilityRebel.AllyBoiCollection = SpriteBuilder.ConstructCollection(AbilityRebel.AllyPrefab, "Ally_Boi_Collection");
					UnityEngine.Object.DontDestroyOnLoad(AbilityRebel.AllyBoiCollection);
					for (int i = 0; i < AbilityRebel.spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(AbilityRebel.spritePaths[i], AbilityRebel.AllyBoiCollection);
					}
					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						43
					}, "attack", tk2dSpriteAnimationClip.WrapMode.Once).fps = 20f;
					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						37,
						38,
						39,
						40,
						41,
						42
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 12f;

					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						37,
						38,
						39,
						40,
						41,
						42
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 12f;

					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						31,
						32,
						33,
						34,
						35,
						36
					}, "run", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 12f;

					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						31,
						32,
						33,
						34,
						35,
						36
					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 12f;

					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						25,
						26,
						27,
						28,
						29,
						30
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(allyBehavior.spriteAnimator, AbilityRebel.AllyBoiCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4,
						5,
						6,
						7,
						8,
						9,
						10,
						11,
						12,
						13,
						14,
						15,
						16,
						17,
						18,
						19,
						20,
						21,
						22,
						23,
						24
					}, "spawn", tk2dSpriteAnimationClip.WrapMode.Once).fps = 24f;
				}
				allyBehavior.aiActor.MovementSpeed = 10f;
				allyBehavior.aiActor.HasShadow = false;
				allyBehavior.aiActor.CanTargetPlayers = false;
				BehaviorSpeculator behaviorSpeculator = allyBehavior.behaviorSpeculator;
				behaviorSpeculator.AttackBehaviors.Add(new AbilityRebel.AllyAttackBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new AbilityRebel.ApproachEnemiesBehavior());
				behaviorSpeculator.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
				{
					IdleAnimations = new string[]
					{
						"idle"
					}
				});
				UnityEngine.Object.DontDestroyOnLoad(AbilityRebel.AllyPrefab);
				FakePrefab.MarkAsFakePrefab(AbilityRebel.AllyPrefab);
				AbilityRebel.AllyPrefab.SetActive(false);
				allyBehavior.CanInterceptBullets = true;
				allyBehavior.aiActor.healthHaver.PreventAllDamage = false;
				allyBehavior.aiActor.CollisionDamage = 0f;
				allyBehavior.aiActor.specRigidbody.CollideWithOthers = true;
				allyBehavior.aiActor.specRigidbody.CollideWithTileMap = false;
				allyBehavior.aiActor.healthHaver.ForceSetCurrentHealth(12f);
				allyBehavior.aiActor.healthHaver.SetHealthMaximum(12f, null, false);
				allyBehavior.aiActor.specRigidbody.PixelColliders.Clear();
				allyBehavior.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.PlayerHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 16,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});;
				/*
				allyBehavior.aiAnimator.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.PlayerHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 1,
					ManualHeight = 1,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});*/
			}
		}

		// Token: 0x040000CB RID: 203
		public static GameObject AllyPrefab;

		// Token: 0x040000CC RID: 204
		public static readonly string guid = "allyboi";

		// Token: 0x040000CD RID: 205
		public int MaxNumberOfCompanions = 100;

		// Token: 0x040000CE RID: 206
		private List<CompanionController> companionsSpawned = new List<CompanionController>();

		// Token: 0x040000CF RID: 207
		private static string[] spritePaths = new string[]
		{
			//"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_001",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_001",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_002",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_003",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_004",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_005",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_006",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_007",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_007",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_008",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_009",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_010",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_011",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_012",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_013",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_014",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_015",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_016",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_017",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_018",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_019",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_020",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_021",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_022",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_023",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyAppear_024",

			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyDead_001",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyDead_002",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyDead_003",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyDead_004",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyDead_005",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyDead_006",

			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyWalk_001",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyWalk_002",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyWalk_003",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyWalk_004",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyWalk_005",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyWalk_006",

			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_001",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_002",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_003",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_004",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_005",
			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_006",

			"LostItems/NuclearThrone/NuclearThroneSprite/Ally/sprAllyIdle_006"
		};

		// Token: 0x040000D0 RID: 208
		private static tk2dSpriteCollectionData AllyBoiCollection;

		// Token: 0x020000AC RID: 172
		public class AllyBehavior : CompanionController
		{
			// Token: 0x0600039C RID: 924 RVA: 0x0001E312 File Offset: 0x0001C512
			private void Start()
			{
				base.spriteAnimator.Play("idle");
				this.Owner = this.m_owner;
			}

			// Token: 0x0400022E RID: 558
			public PlayerController Owner;
		}

		// Token: 0x020000AD RID: 173
		public class AllyAttackBehavior : AttackBehaviorBase
		{
			// Token: 0x0600039E RID: 926 RVA: 0x000196A6 File Offset: 0x000178A6
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x0600039F RID: 927 RVA: 0x0001E332 File Offset: 0x0001C532
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<AbilityRebel.AllyBehavior>().Owner;
			}

			// Token: 0x060003A0 RID: 928 RVA: 0x0001E358 File Offset: 0x0001C558
			public override BehaviorResult Update()
			{
				bool flag = this.attackTimer > 0f && this.isAttacking;
				bool flag2 = flag;
				if (flag2)
				{
					base.DecrementTimer(ref this.attackTimer, false);
				}
				else
				{
					bool flag3 = this.attackCooldownTimer > 0f && !this.isAttacking;
					bool flag4 = flag3;
					if (flag4)
					{
						base.DecrementTimer(ref this.attackCooldownTimer, false);
					}
				}
				bool flag5 = this.IsReady();
				bool flag6 = (!flag5 || this.attackCooldownTimer > 0f || this.attackTimer == 0f || this.m_aiActor.TargetRigidbody == null) && this.isAttacking;
				bool flag7 = flag6;
				BehaviorResult result;
				if (flag7)
				{
					this.StopAttacking();
					result = BehaviorResult.Continue;
				}
				else
				{
					bool flag8 = flag5 && this.attackCooldownTimer == 0f && !this.isAttacking;
					bool flag9 = flag8;
					if (flag9)
					{
						this.attackTimer = this.attackDuration;
						this.m_aiAnimator.PlayUntilFinished(this.attackAnimation, false, null, -1f, false);
						AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", this.m_aiActor.gameObject);
						this.isAttacking = true;
					}
					bool flag10 = this.attackTimer > 0f && flag5;
					bool flag11 = flag10;
					if (flag11)
					{
						this.Attack();
						result = BehaviorResult.SkipAllRemainingBehaviors;
					}
					else
					{
						result = BehaviorResult.Continue;
					}
				}
				return result;
			}

			// Token: 0x060003A1 RID: 929 RVA: 0x0001E4C1 File Offset: 0x0001C6C1
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			// Token: 0x060003A2 RID: 930 RVA: 0x0001E4E4 File Offset: 0x0001C6E4
			public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
			{
				AIActor aiactor = null;
				nearestDistance = float.MaxValue;
				bool flag = activeEnemies == null;
				bool flag2 = flag;
				bool flag3 = flag2;
				bool flag4 = flag3;
				AIActor result;
				if (flag4)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i];
						bool flag5 = aiactor2.healthHaver && aiactor2.healthHaver.IsVulnerable;
						bool flag6 = flag5;
						bool flag7 = flag6;
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !aiactor2.healthHaver.IsDead;
							bool flag10 = flag9;
							bool flag11 = flag10;
							bool flag12 = flag11;
							if (flag12)
							{
								bool flag13 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
								bool flag14 = flag13;
								bool flag15 = flag14;
								bool flag16 = flag15;
								if (flag16)
								{
									float num = Vector2.Distance(position, aiactor2.CenterPosition);
									bool flag17 = num < nearestDistance;
									bool flag18 = flag17;
									bool flag19 = flag18;
									bool flag20 = flag19;
									if (flag20)
									{
										nearestDistance = num;
										aiactor = aiactor2;
									}
								}
							}
						}
					}
					result = aiactor;
				}
				return result;
			}

			// Token: 0x060003A3 RID: 931 RVA: 0x0001E604 File Offset: 0x0001C804
			private void Attack()
			{
				bool flag = this.Owner == null;
				bool flag2 = flag;
				if (flag2)
				{
					this.Owner = this.m_aiActor.GetComponent<AbilityRebel.AllyBehavior>().Owner;
				}
				float num = -1f;
				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag3 = activeEnemies == null | activeEnemies.Count <= 0;
				bool flag4 = !flag3;
				if (flag4)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag5 = nearestEnemy && num < 10f;
					bool flag6 = flag5;
					if (flag6)
					{
						bool flag7 = this.IsInRange(nearestEnemy);
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							bool flag10 = flag9;
							if (flag10)
							{
								Vector2 worldBottomLeft = this.m_aiActor.specRigidbody.sprite.WorldBottomLeft;
								Vector2 unitCenter = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter - worldBottomLeft).normalized);
								Projectile projectile = ((Gun)ETGMod.Databases.Items["ak-47"]).DefaultModule.projectiles[0];
								GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldBottomLeft, Quaternion.Euler(0.5f, 0f, z), true);
								Projectile component = gameObject.GetComponent<Projectile>();
								bool flag11 = component != null;
								bool flag12 = flag11;
								bool flag13 = flag12;
								if (flag13)
								{	
									component.baseData.damage = 5f;
									component.baseData.force = 1f;
									component.collidesWithPlayer = false;
								}
							}
						}
					}
				}
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x0001E830 File Offset: 0x0001CA30
			public override float GetMaxRange()
			{
				return 17f;
			}

			// Token: 0x060003A5 RID: 933 RVA: 0x0001E848 File Offset: 0x0001CA48
			public override float GetMinReadyRange()
			{
				return 7f;
			}

			// Token: 0x060003A6 RID: 934 RVA: 0x0001E860 File Offset: 0x0001CA60
			public override bool IsReady()
			{
				AIActor aiActor = this.m_aiActor;
				bool flag = aiActor == null;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
					Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
					flag2 = (vector == null);
				}
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x060003A7 RID: 935 RVA: 0x0001E900 File Offset: 0x0001CB00
			public bool IsInRange(AIActor enemy)
			{
				bool flag = enemy == null;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					SpeculativeRigidbody specRigidbody = enemy.specRigidbody;
					Vector2? vector = (specRigidbody != null) ? new Vector2?(specRigidbody.UnitCenter) : null;
					flag2 = (vector == null);
				}
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, enemy.specRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x0400022F RID: 559
			public string attackAnimation = "attack";

			// Token: 0x04000230 RID: 560
			private bool isAttacking;

			// Token: 0x04000231 RID: 561
			private float attackCooldown = 1.2f;

			// Token: 0x04000232 RID: 562
			private float attackDuration = 0.01f;

			// Token: 0x04000233 RID: 563
			private float attackTimer;

			// Token: 0x04000234 RID: 564
			private float attackCooldownTimer;

			// Token: 0x04000235 RID: 565
			private PlayerController Owner;

			// Token: 0x04000236 RID: 566
			private List<AIActor> roomEnemies = new List<AIActor>();
		}

		// Token: 0x020000AE RID: 174
		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			// Token: 0x060003A9 RID: 937 RVA: 0x00019D3B File Offset: 0x00017F3B
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			// Token: 0x060003AA RID: 938 RVA: 0x0001E9C3 File Offset: 0x0001CBC3
			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			// Token: 0x060003AB RID: 939 RVA: 0x0001E9DC File Offset: 0x0001CBDC
			public override BehaviorResult Update()
			{
				SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
				bool flag = this.repathTimer > 0f;
				bool flag2 = flag;
				BehaviorResult result;
				if (flag2)
				{
					result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
				}
				else
				{
					bool flag3 = overrideTarget == null;
					bool flag4 = flag3;
					if (flag4)
					{
						this.PickNewTarget();
						result = BehaviorResult.Continue;
					}
					else
					{
						this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
						bool flag5 = overrideTarget != null && !this.isInRange;
						bool flag6 = flag5;
						if (flag6)
						{
							this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
							this.repathTimer = this.PathInterval;
							result = BehaviorResult.SkipRemainingClassBehaviors;
						}
						else
						{
							bool flag7 = overrideTarget != null && this.repathTimer >= 0f;
							bool flag8 = flag7;
							if (flag8)
							{
								this.m_aiActor.ClearPath();
								this.repathTimer = -1f;
							}
							result = BehaviorResult.Continue;
						}
					}
				}
				return result;
			}

			// Token: 0x060003AC RID: 940 RVA: 0x0001EB14 File Offset: 0x0001CD14
			private void PickNewTarget()
			{
				bool flag = this.m_aiActor == null;
				bool flag2 = !flag;
				if (flag2)
				{
					bool flag3 = this.Owner == null;
					bool flag4 = flag3;
					if (flag4)
					{
						this.Owner = this.m_aiActor.GetComponent<AbilityRebel.AllyBehavior>().Owner;
					}
					this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						bool flag5 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc";
						bool flag6 = flag5;
						if (flag6)
						{
							this.roomEnemies.Remove(aiactor);
						}
					}
					bool flag7 = this.roomEnemies.Count == 0;
					bool flag8 = flag7;
					if (flag8)
					{
						this.m_aiActor.OverrideTarget = null;
					}
					else
					{
						AIActor aiActor = this.m_aiActor;
						AIActor aiactor2 = this.roomEnemies[UnityEngine.Random.Range(0, this.roomEnemies.Count)];
						aiActor.OverrideTarget = ((aiactor2 != null) ? aiactor2.specRigidbody : null);
					}
				}
			}

			// Token: 0x04000237 RID: 567
			public float PathInterval = 0.25f;

			// Token: 0x04000238 RID: 568
			public float DesiredDistance = 3f;

			// Token: 0x04000239 RID: 569
			private float repathTimer;

			// Token: 0x0400023A RID: 570
			private List<AIActor> roomEnemies = new List<AIActor>();

			// Token: 0x0400023B RID: 571
			private bool isInRange;

			// Token: 0x0400023C RID: 572
			private PlayerController Owner;
		}
	}
}