using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Master {
	public class RoomManager : MonoBehaviour {
		//singleton
		//this one will change from level to level
		public static RoomManager Instance;

		//Player prefab ref for spawning
		[SerializeField] private GameObject player;

		private void Awake() {
			//set instance
			if (!Instance) {
				Instance = this;
			} else if (GameObject.FindGameObjectsWithTag("RoomManager").Length > 0) {
				Debug.LogError("There is more than one RoomManager in the scene");
			} else {
				Debug.LogWarning("RoomManager instance was not cleared on scene exit");
				Instance = this;
			}
		}

		private void OnDestroy() {
			//remove instance
			Instance = null;
		}
	}
}
