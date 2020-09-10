﻿using Brave.BulletScript;
using EnemyAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LostItems
{

    public class AmmocondaHoming : OverrideBehavior
    {
        public override string OverrideAIActorGUID => "1"; // Replace the GUID with whatever enemy you want to modify. This GUID is for the bullet kin.
                                                                                          // You can find a full list of GUIDs at https://github.com/ModTheGungeon/ETGMod/blob/master/Assembly-CSharp.Base.mm/Content/gungeon_id_map/enemies.txt
        public override void DoOverride()
        {
            // In this method, you can do whatever you want with the enemy using the fields "actor", "healthHaver", "behaviorSpec", and "bulletBank".

            actor.MovementSpeed *= 2; // Doubles the enemy movement speed

            healthHaver.SetHealthMaximum(healthHaver.GetMaxHealth() * 0.5f); // Halves the enemy health

            // The BehaviorSpeculator is responsible for almost everything an enemy does, from shooting a gun to teleporting.
            // Tip: To debug an enemy's BehaviorSpeculator, you can uncomment the line below. This will print all the behavior information to the console.
            //Tools.DebugInformation(behaviorSpec);

            // For this first change, we're just going to increase the lead amount of the bullet kin's ShootGunBehavior so its shots fire like veteran kin.
            ShootGunBehavior shootGunBehavior = behaviorSpec.AttackBehaviors[0] as ShootGunBehavior; // Get the ShootGunBehavior, at index 0 of the AttackBehaviors list
            shootGunBehavior.LeadAmount = 0.62f;

            // Next, we're going to change another few things on the ShootGunBehavior so that it has a custom BulletScript.
            shootGunBehavior.WeaponType = WeaponType.BulletScript; // Makes it so the bullet kin will shoot our bullet script instead of his own gun shot.
            shootGunBehavior.BulletScript = new CustomBulletScriptSelector(typeof(TestBulletScript)); // Sets the bullet kin's bullet script to our custom bullet script.
        }

        public class TestBulletScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
        {
            protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
            {
                base.Fire(new Direction(-40f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
                base.Fire(new Direction(40f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.

                yield return Wait(20); // Wait for 20 frames

                base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null); // Shoot a bullet -20 degrees from the enemy aim angle, with a bullet speed of 9.
                base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null); // Shoot a bullet 20 degrees from the enemy aim angle, with a bullet speed of 9.

                yield break;
            }
        }
    }
}
