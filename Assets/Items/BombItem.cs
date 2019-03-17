#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
	public class BombItem : ItemBase {
		[Header("Timer")]
		[SerializeField] private float TimeToExplosion;

		private float timer = 0f;

		protected override void FixedUpdate() {
			base.FixedUpdate();
			timer += Time.fixedDeltaTime;
			if (timer >= TimeToExplosion) {
				Destroy(gameObject);
			}
		}
	}
}
