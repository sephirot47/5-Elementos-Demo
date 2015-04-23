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
		DrawCrossfire();
		DrawEnemyTargetCrossfire();
	}

	void DrawCrossfire()
	{
		Vector2 v = CameraControl.GetLookScreenPoint();
		DrawBox(new Rect(v.x - 1, v.y/2 - 5, 3, 10), Color.red);
		DrawBox(new Rect(v.x - 5, v.y/2 - 1, 10, 3), Color.red);
	}

	void DrawEnemyTargetCrossfire()
	{
		if(Core.selectedPlayer != null && Core.selectedPlayer.target != null)
		{
			Vector3 targetPos = Core.selectedPlayer.target.transform.position;
			Vector2 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos);


			float crossfireSize = Mathf.Ceil(160.0f / Vector3.Distance(Camera.main.transform.position, targetPos));
			DrawBox(new Rect(targetScreenPos.x - crossfireSize/2, 
			                 Screen.height - targetScreenPos.y - crossfireSize/2, 
			                 crossfireSize, crossfireSize), 
			        		 Color.yellow);
		}
	}

	void DrawBox(Rect r, Color c)
	{
		GUIStyle style = new GUIStyle();
		style.normal.background = new Texture2D(1,1);
		style.normal.background.SetPixel(0,0,c);
		style.normal.background.Apply();
		GUI.Box(r, "", style);
	}
}
