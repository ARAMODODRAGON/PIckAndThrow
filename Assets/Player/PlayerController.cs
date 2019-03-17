#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {

	enum Actions {
		Transition,
		Active,
		Picking,
		Holding,
		Throwing,
		Dead
	}

	/* Main Class */
	public partial class PlayerController : MonoBehaviour {
		//reference to GM and RM
		private Master.GameManager GM;
		private Master.RoomManager RM;

		#region Input
		//input
		private Button jump;
		private Button pick;
		private Button aim;
		private Vector2 axis = Vector2.zero;
		#endregion

		#region Components
		//Colliders
		[Header("Box Colliders")]
		[SerializeField] private BoxCollider2D bigBox;
		[SerializeField] private BoxCollider2D smallBox;

		//components
		private Rigidbody2D rb;
		private SpriteRenderer sp;
		private CollCheck cc;
		#endregion

		//states
		Actions currentAction = Actions.Active;
		private bool IsCrouching {
			get {
				return smallBox.enabled && !bigBox.enabled;
			}
			set {
				smallBox.enabled = value;
				bigBox.enabled = !value;
			}
		}
		private bool FacingRight {
			get {
				return sp.flipX;
			}
			set {
				sp.flipX = value;
			}
		}
		private static int Health = 3;
		private static bool IsInWater = false;

		//Held item
		private Items.ItemBase HeldItem;

		private void Awake() {
			//setup inputs
			jump = new Button("Jump");
			pick = new Button("Pick");
			aim = new Button("Aim");

			//setup components
			rb = GetComponent<Rigidbody2D>();
			sp = GetComponent<SpriteRenderer>();
			cc = GetComponent<CollCheck>();
		}

		private void Start() {
			//get ref to GM and RM
			GM = Master.GameManager.Instance;
			if (!GM) {
				Debug.LogWarning("Could not find GameManager in scene");
			}

			RM = Master.RoomManager.Instance;
			if (!RM) {
				Debug.LogWarning("Could not find RoomManager in scene");
			}
		}

		private void FixedUpdate() {
			//check for death
			if (Health == 0) {
				currentAction = Actions.Dead;
			}

			//update inputs
			jump.updateInput();
			pick.updateInput();
			aim.updateInput();
			axis.x = Input.GetAxis("Horizontal");
			axis.y = Input.GetAxis("Vertical");

			//flip if needed
			CheckForFlip();

			//handle movement
			if (IsInWater) {

			} else {
				HandleGroundMovement();
			}

			//handle item throwning
			HandleItem();
		}

		private void LateUpdate() {
			//handle item holding
			if (HeldItem == null) {
				currentAction = Actions.Throwing;
			}
			if (currentAction == Actions.Holding) {
				HeldItem.transform.position = transform.position + Vector3.up;
			}
		}

		private void CheckForFlip() {
			if (FacingRight && axis.x < 0f) {
				FacingRight = false;
			} else if (!FacingRight && axis.x > 0f) {
				FacingRight = true;
			}
		}

		private void HandleItem() {
			if (HeldItem == null) {
				currentAction = Actions.Throwing;
			}

			if (currentAction == Actions.Throwing) {
				currentAction = Actions.Active;
			} else if (currentAction == Actions.Holding && pick.ButtonDown) {
				Debug.Log(axis);
				if (axis.x != 0f || axis.y != 0f) {
					HeldItem.Throw(axis, rb.velocity);
				} else if (FacingRight) {
					HeldItem.Throw(Vector2.right, rb.velocity);
				} else {
					HeldItem.Throw(Vector2.left, rb.velocity);
				}

				currentAction = Actions.Throwing;
			}
		}

		private void OnTriggerStay2D(Collider2D other) {
			if (currentAction != Actions.Holding && pick.ButtonDown && other.tag == "ItemSpawn") {
				//pull an item out of ground
				HeldItem = Instantiate(other.GetComponent<Items.Grass>().item).GetComponent<Items.ItemBase>();
				HeldItem.Pick();
				Destroy(other.gameObject);
				currentAction = Actions.Holding;
			}
		}

		private void OnCollisionStay2D(Collision2D other) {
			if (currentAction != Actions.Holding && pick.ButtonDown && other.gameObject.tag == "Item") {
				//pick up a repickable item
				HeldItem = other.gameObject.GetComponent<Items.ItemBase>();
				HeldItem.Pick();
				currentAction = Actions.Holding;
			}
		}
	}
}
