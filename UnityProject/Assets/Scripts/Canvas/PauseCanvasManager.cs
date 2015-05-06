using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class PauseCanvasManager : MonoBehaviour 
{
	private static GameObject pauseMenu;

	void Start()
	{
		pauseMenu = Core.GetSubGameObject(gameObject, "PauseMenu");
		CanvasUtils.Hide(pauseMenu);
	}

	void Update()
	{
		if(!GameState.IsPlaying())
		{
		}
		else
		{
			if(GameState.AllPlayersDead())
			{
				CanvasUtils.Show(pauseMenu);
			}
		}
	}

	public static void OnPauseStart()
	{
		CanvasUtils.Show(pauseMenu);
		CanvasUtils.ShowCursor();
	}

	public static void OnPauseFinish()
	{
		CanvasUtils.Hide(pauseMenu);
		CanvasUtils.HideCursor();
	}

	void OnGUI()
	{
		if(!!GameState.IsPlaying())
		{
			DrawTargetCrossfire();
		}
	}

	void DrawCrossfire()
	{
		Vector2 v = CameraControl.GetLookScreenPoint();
		DrawBox(new Rect(v.x - 1, v.y/2 - 5, 3, 10), Color.red);
		DrawBox(new Rect(v.x - 5, v.y/2 - 1, 10, 3), Color.red);
	}

	void DrawTargetCrossfire()
	{
		Player player = Core.selectedPlayer;
		if(player != null)
		{
			GameObject target = player.GetTarget();
			if(target != null)
			{
				Vector3 targetPos = target.transform.position;
				Vector2 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos);

				float crossfireSize = Mathf.Ceil(160.0f / Vector3.Distance(Camera.main.transform.position, targetPos));
				DrawBox(new Rect(targetScreenPos.x - crossfireSize/2, 
				                 Screen.height - targetScreenPos.y - crossfireSize/2, 
				                 crossfireSize, crossfireSize), 
				      	  		 GetTargetColor(target));
			}
		}
	}

	private Color GetTargetColor(GameObject target)
	{
		if(target.CompareTag("Enemy")) return Color.red;
		if(target.CompareTag("NPC")) return Color.yellow;
		return Color.gray;
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
