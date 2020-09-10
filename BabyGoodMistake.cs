using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;

namespace LostItems
{
	public class BabyGoodMistake : CompanionItem
	{
		public static GameObject prefab;
		private static readonly string guid = "send_help987654321"; //give your companion some unique guid
		float damageBuff = -1;

		public static void Init()
		{
			string itemName = "Ministake";
			string resourceName = "LostItems/sprites/mistakecharm";

			GameObject obj = new GameObject();
			var item = obj.AddComponent<BabyGoodMistake>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "wip";
			string longDesc = "These strange creatures will apear if they find your skills in combat exaptable, all though they will weaken you in exchange for there deffence.";

			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = PickupObject.ItemQuality.C;
			item.CompanionGuid = guid; //this will be used by the item later to pull your companion from the enemy database
			item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
			//item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 5f);
			item.AddPassiveStatModifier(PlayerStats.StatType.Damage, -0.05f, StatModifier.ModifyMethod.ADDITIVE);
			item.CompanionGuid = BabyGoodMistake.guid;
			item.CanBeDropped = false;
			BuildPrefab();
			item.PlaceItemInAmmonomiconAfterItemById(664);

		}

		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			//prefab = CompanionBuilder.BuildPrefab("mistake", guid, "LostItems/sprites/Pets/mistake/Idle_front/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
			prefab = CompanionBuilder.BuildPrefab("mistake", guid, "LostItems/sprites/Pets/mistake/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
		
		 //Add a companion component to the prefab (could be a custom class)
		 var companion = prefab.AddComponent<CompanionController>();
			companion.aiActor.MovementSpeed = 5f;

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			//prefab.AddAnimation("idle_right", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_right", "LostItems/sprites/Pets/mistake/idle_front", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			//prefab.AddAnimation("idle_left", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_left", "LostItems/sprites/Pets/mistake/idle_front", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			//prefab.AddAnimation("run_right", "ItemAPI/Resources/BigSlime/MoveRight", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_right", "LostItems/sprites/Pets/mistake/idle_front", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
			//prefab.AddAnimation("run_left", "ItemAPI/Resources/BigSlime/MoveLeft", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("run_left", "LostItems/sprites/Pets/mistake/idle_front", fps: 7, AnimationType.Move, DirectionType.TwoWayHorizontal);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			var bs = prefab.GetComponent<BehaviorSpeculator>();
			bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });
			companion.CanInterceptBullets = true;
			//companion.CanInterceptBullets = true;
			companion.aiActor.healthHaver.PreventAllDamage = true;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = false;
			companion.aiActor.healthHaver.ForceSetCurrentHealth(1f);
			companion.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
			companion.aiActor.specRigidbody.PixelColliders.Clear();
			companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				//CollisionLayer = CollisionLayer.PlayerCollider,
				CollisionLayer = CollisionLayer.EnemyBulletBlocker,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 16,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});
			/*
			companion.aiAnimator.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.BulletBlocker,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 16,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});
			*/
		}
		protected override void Update()
		{
			base.Update();
		}
		public override void Pickup(PlayerController player)
		{
			player.OnKilledEnemy += this.OnKill;
			base.Pickup(player);
		}
		private void OnKill(PlayerController player)
		{
			killCount = UnityEngine.Random.Range(1, 6);
			if (killCount >= 5)
			{
				this.CreateNewCompanion(base.Owner);
				//killCount = 0;
				//base.Owner.healthHaver.NextShotKills = true;

			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00010334 File Offset: 0x0000E534
		private void CreateNewCompanion(PlayerController player)
		{


			AkSoundEngine.PostEvent("Play_OBJ_smallchest_spawn_01", base.gameObject);
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

					float curDamage = base.Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
					float newDamage = curDamage - 0.05f;
					base.Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, base.Owner);
					damageBuff = newDamage - curDamage;

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
					orAddComponent.aiAnimator.PlayUntilFinished("spawn", false, null, -1f, false);
				}
			}
		}
		float killCount = 0;
		public int MaxNumberOfCompanions = 10;
		private List<CompanionController> companionsSpawned = new List<CompanionController>();
	}
}







