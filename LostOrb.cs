using UnityEngine;
using ItemAPI;

namespace LostItems
{
    class LostOrb : PlayerItem
    {
        /*
        private static int[] spriteIDs;
        int charge = 0;
        private static readonly string[] spritePaths = new string[]
        {
            "LostItems/sprites/Shadow_Relic_001",
            "LostItems/sprites/Shadow_Relic_002",
            "LostItems/sprites/Shadow_Relic_003",
            "LostItems/sprites/Shadow_Relic_004",
            "LostItems/sprites/Shadow_Relic_005"
        };
        */
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Shadow Relic";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "LostItems/sprites/Shadow_Relic_001";
            //LostOrb.spriteIDs = new int[LostOrb.spritePaths.Length];
            //string resourceName = LostOrb.spritePaths[4];

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<LostOrb>();
            /*
            LostOrb.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(LostOrb.spritePaths[0], item.sprite.Collection);
            LostOrb.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(LostOrb.spritePaths[1], item.sprite.Collection);
            LostOrb.spriteIDs[2] = SpriteBuilder.AddSpriteToCollection(LostOrb.spritePaths[2], item.sprite.Collection);
            LostOrb.spriteIDs[3] = SpriteBuilder.AddSpriteToCollection(LostOrb.spritePaths[3], item.sprite.Collection);
            LostOrb.spriteIDs[4] = item.sprite.spriteId;
            */
            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Become one with darkness";
            string longDesc = "This relic allows its owner to become a shadow for a short time. \n\n A relic once belonging to a strange treveler.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

			//Set the cooldown type and duration of the cooldown
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.SPECIAL;
            item.CanBeDropped = false;
            item.PreventStartingOwnerFromDropping = true;
            item.PlaceItemInAmmonomiconAfterItemById(462);

        }
        public override void Update()
        {
            base.Update();
            //bool pickedUp = base.PickedUp;
            //if (pickedUp)
            //{
            //    base.sprite.SetSprite(LostOrb.spriteIDs[this.charge]);
            //}
        }
        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float duration = 3f;
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
			StartEffect(user);
			StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }
            private void StartEffect(PlayerController user)
        {
            ConsumableStealthItem smokebomb = PickupObjectDatabase.GetById(462).GetComponent<ConsumableStealthItem>();
            poofVfx = smokebomb.poofVfx.gameObject;
            //user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Jam_Status") as GameObject, Vector3.zero, true, false, false);
            user.ChangeSpecialShaderFlag(1, 1f);
            user.SetIsStealthed(true, "orb");
            user.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
        }


        private void EndEffect(PlayerController user)
        {
            //charge += 1;
            user.SetIsStealthed(false, "orb");
            user.ChangeSpecialShaderFlag(1, 0f);
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
            user.PlayEffectOnActor(this.poofVfx, Vector3.zero, false, true, false);
        }
        public override bool CanBeUsed(PlayerController user)
		{
			return base.CanBeUsed(user);
		}

        public GameObject poofVfx;

    }
}