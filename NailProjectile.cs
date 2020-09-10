using System;
using UnityEngine;

namespace LostItems
{
	// Token: 0x02000009 RID: 9
	internal class NailProjectile : MonoBehaviour
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00003B40 File Offset: 0x00001D40
		public void Start()
		{
			this.projectile = base.GetComponent<Projectile>();
			Projectile projectile = this.projectile;
			projectile.baseData.damage = 10f;
			projectile.baseData.speed = 20f;
			projectile.baseData.range = 2.1474836E+09f;
			projectile.sprite.spriteId = projectile.sprite.GetSpriteIdByName("grubberfly_strike_001");
			projectile.baseData.force = 0f;
		}

		// Token: 0x04000009 RID: 9
		private Projectile projectile;
	}
}
