using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour 
{
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int agro;
	
	public static Player selectedPlayer;
	public static Player kaji, zap, lluvia;
	
	private Transform myTransform;
	private GameObject go ;
	
	
	
	void Awake()
	{
		myTransform = transform;
	}
	
	void Start () 
	{
		
		
	}
	
	void Update () 
	{
		go = Core.selectedPlayer.gameObject;
		target = go.transform;
		Debug.DrawLine (target.transform.position, myTransform.position, Color.blue);
	}
}