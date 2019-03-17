using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
	public class BlockItem : SimpleItem {
		public override void Throw(Vector2 direction, Vector2 additive) {
			if (direction.x > 0f) {
				transform.position = new Vector3(transform.position.x + 1f, transform.position.y);
			} else {
				transform.position = new Vector3(transform.position.x - 1f, transform.position.y);
			}

			base.Throw(direction, additive);
		}

		protected override void OnBecameInvisible() { /* Do Nothing */ }
	}
}
