using UnityEngine;
using System.Collections;

public class triger : MonoBehaviour {

	void OnTriggerStay (Collider col) {
		if (col.gameObject.name == "Zap") {

			Destroy(col.gameObject);
		}
	}
}
