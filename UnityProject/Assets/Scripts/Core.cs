using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour 
{
	public static float playerToPlayerFollowDistance = 4.0f;
	public static Player selectedPlayer;
	public static Player kaji, zap, lluvia;
	public static float gravity = -3.0f;

	public static bool paused = false;

	void Start() 
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectiles")); 
		Cursor.visible = false;

		kaji = GameObject.Find("Kaji").GetComponent<Player>();
		zap = GameObject.Find("Zap").GetComponent<Player>();
		lluvia = GameObject.Find("Lluvia").GetComponent<Player>();

		SelectPlayer(kaji);

		KeyComboManager.Init();
	}

	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Alpha1)) SelectPlayer(kaji);
		else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectPlayer(zap);
		else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectPlayer(lluvia);
		else if (Input.GetKeyDown(KeyCode.Q)) SwitchPlayer(true);
		else if (Input.GetKeyDown(KeyCode.E)) SwitchPlayer(false);

		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			if(paused) CanvasManager.OnPause();
			else CanvasManager.OnResume();
		}
	}
	
	void SwitchPlayer(bool right)
	{
		if(right)
		{
			if(selectedPlayer == kaji) SelectPlayer(zap);
			else if(selectedPlayer == zap) SelectPlayer(lluvia);
			else SelectPlayer(kaji);
		}
		else
		{
			if(selectedPlayer == kaji) SelectPlayer(lluvia);
			else if(selectedPlayer == zap) SelectPlayer(kaji);
			else SelectPlayer(zap);
		}
	}

	void SelectPlayer(Player p)
	{
		if(p == null) return;

		kaji.selected = zap.selected = lluvia.selected = false;
		p.selected = true;
		selectedPlayer = p;
		Camera.main.GetComponent<CameraControl>().SelectTarget(p.gameObject.transform);
	}

	public static Player GetOtherFollowerPlayer(Player p)
	{
		if(kaji != p && kaji != selectedPlayer) return kaji;
		if(lluvia != p && lluvia != selectedPlayer) return lluvia;
		if(zap != p && zap != selectedPlayer) return zap;
		return null;
	}
}
