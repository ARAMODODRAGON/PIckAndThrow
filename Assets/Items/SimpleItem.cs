using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {

	public class SimpleItem : ItemBase {
		[Header("Sprites")]
		public List<Sprite> sprites = new List<Sprite>();

		protected override void Awake() {
			base.Awake();
			GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
		}
	}
}
