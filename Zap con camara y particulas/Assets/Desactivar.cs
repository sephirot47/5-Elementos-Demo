using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Desactivar : MonoBehaviour {
	// Use this for initialization
	public bool activeSelf;
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Kajiuno")) {
			gameObject.SetActive(false);
		}
			if (Input.GetButton ("lluviados")) {

		} 
		}

}
