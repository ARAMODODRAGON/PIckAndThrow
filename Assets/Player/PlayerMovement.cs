#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
	/* Movement */
	public partial class PlayerController {
		//ground variables
		[Header("Ground Movement")]
		[SerializeField] private float maxFallSpeed;
		[SerializeField] private float fallAcceleration;
		[SerializeField] private float fastFallAccelration;
		[SerializeField] private float jumpVelocity;
		[SerializeField] private float maxWalkSpeed;
		[SerializeField] private float maxRunSpeed;
		[SerializeField] private float acceleration;
		[SerializeField] private float reverseAcceleration;
		[SerializeField] private float decceleration;

		//velocity vector
		private Vector2 velocity = Vector2.zero;

		private void HandleGroundMovement() {
			//get the resultant velocity and fixeddeltatime
			velocity = rb.velocity;
			float fdelta = Time.fixedDeltaTime;

			//first handle x movement
			if (!aim.ButtonHeld) {
				if (velocity.x > 0f && axis.x > 0f) { //moving in same direction
					velocity.x += acceleration * fdelta;
				} else if (velocity.x < 0f && axis.x < 0f) { //moving in same direction
					velocity.x -= acceleration * fdelta;
				} else if (axis.x == 0f) { //not moving
					if (velocity.x > 0f) {
						velocity.x -= decceleration * fdelta;
					} else if (velocity.x < 0f) {
						velocity.x += decceleration * fdelta;
					}
					//make sure that the player stops
					if (Mathf.Abs(velocity.x) < 0.4f) velocity.x = 0f;
				} else if (axis.x > 0f) {
					velocity.x += reverseAcceleration * fdelta;
				} else if (axis.x < 0f) {
					velocity.x -= reverseAcceleration * fdelta;
				}
			} else {
				if (velocity.x < 0f) {
					velocity.x += reverseAcceleration * fdelta;
				} else if (velocity.x > 0f) {
					velocity.x -= reverseAcceleration * fdelta;
				}
				//make sure that the player stops
				if (Mathf.Abs(velocity.x) < 0.4f) velocity.x = 0f;
			}

			//cap the x velocity
			if (velocity.x > maxWalkSpeed) {
				velocity.x = maxWalkSpeed;
			} else if (velocity.x < -maxWalkSpeed) {
				velocity.x = -maxWalkSpeed;
			}
			//if (false) {
			//	if (velocity.x > maxRunSpeed) {
			//		velocity.x = maxRunSpeed;
			//	} else if (velocity.x < -maxRunSpeed) {
			//		velocity.x = -maxRunSpeed;
			//	}
			//} else {

			//}

			//next y movement
			if (!cc.IsGrounded) {
				if (jump.ButtonHeld) {
					if (velocity.y > -maxFallSpeed) {
						velocity.y -= fallAcceleration * fdelta;
					} else if (velocity.y < -maxFallSpeed) {
						velocity.y = -maxFallSpeed;
					}
				} else {
					if (velocity.y > -maxFallSpeed) {
						velocity.y -= fastFallAccelration * fdelta;
					} else if (velocity.y < -maxFallSpeed) {
						velocity.y = -maxFallSpeed;
					}
				}
			} else if (jump.ButtonDown) {
				velocity.y = jumpVelocity;
			}

			//finally set the new velocity back into the rigidbody
			rb.velocity = velocity;
		}

	}
}
