using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using UnityEngine;

//charmed bow
//derringer
//ser manual's revolver done
//bubble blaster - range done
//Mr. Accretion Jr. - fire rate max ammo
//Dueling Pistol
//Shellegun
//Grey Mauser - max ammo
//AC-15 armor on pick up maybe
//Deck4rd clip size - max ammo

namespace LostItems
{
    class GunBuffs : MonoBehaviour
    {
        public static void Init()
        {
			Gun grassChopper = PickupObjectDatabase.GetById(180) as Gun;


			grassChopper.SetBaseMaxAmmo(120);
			grassChopper.GainAmmo(120);
			grassChopper.DefaultModule.cooldownTime = 0.5f;
			grassChopper.DefaultModule.numberOfShotsInClip = 5;
			//gun.reloadTime = 0.8f;
			//gun.DefaultModule.cooldownTime = 0.025f;
			Projectile projectileGC = grassChopper.DefaultModule.projectiles[0];
			//projectile.FireApplyChance = 100f;
			//projectile.ignoreDamageCaps = true;
			projectileGC.baseData.damage *= 1.5f;
			//projectile.baseData.force /= 3f;
			//PierceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
			//orAddComponent.penetratesBreakables = true;
			//orAddComponent.penetration = 999;
			//orAddComponent.preventPenetrationOfActors = false;
			//orAddComponent.BeastModeLevel = PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE;

			Gun witchPistol = PickupObjectDatabase.GetById(145) as Gun;
			witchPistol.DefaultModule.numberOfShotsInClip += 1;

			Projectile projectileWP = witchPistol.DefaultModule.projectiles[0];
			projectileWP.baseData.damage = 8;
			projectileWP.ChanceToTransmogrify *= 1.2f;

			Gun SunlightJavline = PickupObjectDatabase.GetById(748) as Gun;
			SunlightJavline.DefaultModule.numberOfShotsInClip = 1;
			SunlightJavline.reloadTime = 0.5f;


			Gun Screecher = PickupObjectDatabase.GetById(3) as Gun;


			Projectile projectileSC = Screecher.DefaultModule.projectiles[0];
			projectileSC.baseData.damage *= 1.5f;

			//Gun TurboGun = PickupObjectDatabase.GetById(577) as Gun;

			Gun SerManualsRevolver = PickupObjectDatabase.GetById(183) as Gun;

			SerManualsRevolver.SetBaseMaxAmmo(450);
			SerManualsRevolver.GainAmmo(450);
			SerManualsRevolver.PlaceItemInAmmonomiconAfterItemById(380);
			SerManualsRevolver.DefaultModule.numberOfShotsInClip += 4;
			SerManualsRevolver.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			SerManualsRevolver.quality = PickupObject.ItemQuality.B;
			//SerManualsRevolver.sy

			SerManualsRevolver.DefaultModule.projectiles[0] = (PickupObjectDatabase.GetById(380) as Gun).DefaultModule.projectiles[0];
			//SerManualsRevolver.DefaultModule.projectiles[0].baseData.damage = 8;

			//Projectile projectileSMR = SerManualsRevolver.DefaultModule.projectiles[0];
			//projectileSMR.baseData.damage = 6;

			Gun BubbleBlaster = PickupObjectDatabase.GetById(599) as Gun;
			BubbleBlaster.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			BubbleBlaster.DefaultModule.burstShotCount = 9;
			BubbleBlaster.DefaultModule.burstCooldownTime = BubbleBlaster.DefaultModule.cooldownTime/2;
			BubbleBlaster.DefaultModule.numberOfShotsInClip = 9;

			Projectile projectileBB = BubbleBlaster.DefaultModule.projectiles[0];

			Gun Casey = PickupObjectDatabase.GetById(541) as Gun;
			Projectile projectileBat = Casey.DefaultModule.projectiles[0];
			projectileBat.AppliesKnockbackToPlayer = true;
			projectileBat.PlayerKnockbackForce = 100;
			//projectileBat.baseData.force = -10;
			projectileBat.knockbackDoer.knockbackMultiplier = -10;
			//projectileBB.baseData.life
			//projectileBB.baseData.AccelerationCurve *= 2;




		}
    }
}
