using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour 
{
	public static float playerToPlayerFollowDistance = 4.0f;
	public static Player selectedPlayer;
	public static Player kaji, zap, lluvia;
	public static float gravity = -3.0f;

	public static List<Player> playerList;

	void Start() 
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectiles")); 
		Cursor.visible = false;

		kaji = GameObject.Find("Kaji").GetComponent<Player>();
		zap = GameObject.Find("Zap").GetComponent<Player>();
		lluvia = GameObject.Find("Lluvia").GetComponent<Player>();
		SelectPlayer(kaji);
		
		Player[] players = {Core.kaji, Core.zap, Core.lluvia};
		playerList = new List<Player>(players);

		KeyComboManager.Init();
	}

	void Update() 
	{
		if(Core.selectedPlayer != null && Core.selectedPlayer.IsDead()) 
		{
			SwitchPlayer(true);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) && !kaji.IsDead()) SelectPlayer(kaji);
		else if (Input.GetKeyDown(KeyCode.Alpha2) && !zap.IsDead()) SelectPlayer(zap);
		else if (Input.GetKeyDown(KeyCode.Alpha3) && !lluvia.IsDead()) SelectPlayer(lluvia);

		else if (Input.GetKeyDown(KeyCode.Q)) SwitchPlayer(true);
		else if (Input.GetKeyDown(KeyCode.E)) SwitchPlayer(false);
	}
	
	void SwitchPlayer(bool right)
	{
		int step = right ? 1 : -1, 
		    i = (selectedPlayer == null) ? 0 : playerList.IndexOf(selectedPlayer);

		int counter = 0;
		i = (i + playerList.Count * 2 + step) % playerList.Count;
		while( (i <= playerList.Count && i >= 0) || counter <= playerList.Count)
		{
			int indexOfNewPlayer = (i + playerList.Count * 2) % playerList.Count;
			if(!playerList[indexOfNewPlayer].IsDead())
			{
				SelectPlayer( playerList[indexOfNewPlayer] );
				break;
			}
			i += step;
			++counter;
		}
	}

	void SelectPlayer(Player p)
	{
		if(p == null) return;
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

	public static Vector3 PlaneVector(Vector3 v)
	{
		return new Vector3(v.x, 0.0f, v.z);
	}

	public static GameObject GetSubGameObject(GameObject parent, string goName)
	{
		foreach(Transform t in parent.transform)
		{
			if(t.gameObject.name == goName) return t.gameObject;
		}
		return null;
	}
}
