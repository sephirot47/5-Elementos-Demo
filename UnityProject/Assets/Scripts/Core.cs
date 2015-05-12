using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour
{
	public static float playerToPlayerFollowDistance = 4.0f;
	public static Player selectedPlayer;
	public static Player kaji, zap, lluvia;
	public static float gravity = -3.0f;

	public static List<Player> playerList = new List<Player>();

	void Start() 
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectiles")); 

		kaji = GameObject.Find("Kaji").GetComponent<Player>();
		zap = GameObject.Find("Zap").GetComponent<Player>();
		lluvia = GameObject.Find("Lluvia").GetComponent<Player>();
		
		Player[] players = {Core.kaji, Core.zap, Core.lluvia};
		playerList.AddRange(players);

		KeyComboManager.Init();
		
		CanvasUtils.HideCursor();
	}

	void Update() 
	{

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
	
	//Busca por toda la jerarquia del parent, hasta el fondo de todos los objetos, infinite deep (DFS)
	public static GameObject GetSubGameObject(GameObject parent, string goName)
	{
		foreach(Transform t in parent.transform)
		{
			if(t.gameObject.name == goName) return t.gameObject;
			else
			{
				GameObject go = GetSubGameObject(t.gameObject, goName);
				if(go != null) return go;
			}
		}
		return null;
	}
}
