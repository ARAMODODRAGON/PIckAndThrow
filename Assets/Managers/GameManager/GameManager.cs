using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Master {
	static class PlayerInfo {
		static public bool FacingRight = true;
		static public int Health = 3;
	}

	public class GameManager : MonoBehaviour {
		//singleton
		public static GameManager Instance;
		
		private void Awake() {
			//setup singleton
			if (!Instance) {
				Instance = this;
			} else {
				Debug.LogWarning("Theres more than one GameManager in this scene");
			}
		}
	}
}
