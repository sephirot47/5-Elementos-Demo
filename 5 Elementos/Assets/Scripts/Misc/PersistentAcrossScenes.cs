using UnityEngine;
using System.Collections;

public class PersistentAcrossScenes : MonoBehaviour {

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
        Application.LoadLevel("proceso 1");
	}
	
}
