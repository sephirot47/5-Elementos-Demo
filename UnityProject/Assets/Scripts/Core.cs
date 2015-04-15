using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour 
{
	public Player kaji, zap, lluvia;

	public static float gravity = -10.0f;
	void Start() 
	{
		kaji = GameObject.Find ("Kaji").GetComponent<Player>();
		zap = GameObject.Find ("Zap").GetComponent<Player>();
		lluvia = GameObject.Find ("Lluvia").GetComponent<Player>();
		zap.selected = true;
	}

	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Alpha1)) ChangePlayer(kaji);
		else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangePlayer(zap);
		else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangePlayer(lluvia);
	}

	void ChangePlayer(Player p)
	{
		kaji.selected = zap.selected = lluvia.selected = false;
		p.selected = true;
		Camera.main.GetComponent<CameraControl>().target = p.gameObject.transform;
	}
}
