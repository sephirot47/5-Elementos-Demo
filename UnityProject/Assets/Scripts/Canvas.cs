using UnityEngine;
using System.Collections;

public class Canvas : MonoBehaviour 
{
	void Start()
	{
	
	}

	void Update()
	{
	
	}

	void OnGUI()
	{
		//DrawCrossfire();
	}

	void DrawCrossfire()
	{
		GUI.backgroundColor = Color.red;
		GUI.Box(new Rect(Screen.width/2 - 1, Screen.height/2 - 1, 3, 3), "");
	}
}
