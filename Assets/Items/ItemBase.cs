#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
	public enum ItemState {
		Picked,
		Thrown,
		Dead
	}

	public class ItemBase : MonoBehaviour {
		//movement variables
		[SerializeField] private float maxGravity = 20f;

		[Header("X Additives")]
		[SerializeField] private float XPreAdditive;
		[Min(0f)]
		[SerializeField] private float XMultiplier = 1f;
		[SerializeField] private float XPostAdditive;
		[Header("Y Additives")]
		[SerializeField] private float YPreAdditive;
		[Min(0f)]
		[SerializeField] private float YMultiplier = 1f;
		[SerializeField] private float YPostAdditive;
		[Header("Additive Velocity Multiplier")]
		[Min(0f)]
		[SerializeField] private float Multiplier = 1f;

		//components
		protected Rigidbody2D rb;
		protected BoxCollider2D bx;

		//state
		protected ItemState currentState = ItemState.Picked;

		protected virtual void Awake() {
			//get components
			rb = GetComponent<Rigidbody2D>();
			rb.simulated = false;
			bx = GetComponent<BoxCollider2D>();
			bx.enabled = false;
		}

		protected virtual void FixedUpdate() {
			//make sure the item doesnt fall too fast 
			if (rb.velocity.y < -maxGravity) {
				rb.velocity = new Vector2(rb.velocity.x, -maxGravity);
			}
		}

		public virtual void Throw(Vector2 direction, Vector2 additive) {
			direction.x += XPreAdditive;
			direction.x *= XMultiplier;
			direction.x += XPostAdditive;

			direction.y += YPreAdditive;
			direction.y *= YMultiplier;
			direction.y += YPostAdditive;

			additive *= Multiplier;

			bx.enabled = true;
			rb.simulated = true;
			rb.velocity = direction + additive;
			currentState = ItemState.Thrown;
		}

		public virtual void Pick() {
			currentState = ItemState.Picked;
			rb.simulated = false;
			bx.enabled = false;
		}

		protected virtual void OnBecameInvisible() {
			Destroy(gameObject);
		}
	}
}
