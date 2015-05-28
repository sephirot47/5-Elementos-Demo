using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseCanvasManager : MonoBehaviour 
{
	private static GameObject pauseCanvas;
	private static Button resumeButton, exitButton;

	void Start()
    {
		pauseCanvas = gameObject;
		resumeButton = Core.GetSubGameObject(pauseCanvas, "ResumeButton").GetComponent<Button>();
        exitButton = Core.GetSubGameObject(pauseCanvas, "ExitButton").GetComponent<Button>();

		CanvasUtils.Hide(pauseCanvas);
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
				CanvasUtils.Show(pauseCanvas);
			}
		}
	}

	public void OnResumeButtonPressed()
    {
        GameState.ChangeState(GameState.Playing);
	}

    public void OnExitButtonPressed()
    {
        UnityEditor.EditorApplication.isPlaying = false; //Para pararlo en el editor :)
        Application.Quit();
    }


	public static void OnPauseStart()
	{
		CanvasUtils.Show(pauseCanvas);
		CanvasUtils.ShowCursor();
	}

	public static void OnPauseFinish()
	{
		CanvasUtils.Hide(pauseCanvas);
		CanvasUtils.HideCursor();
	}

	void OnGUI()
	{
		if(GameState.IsPlaying())
		{
			//DrawTargetCrosshair();
		}
	}

	void DrawCrosshair()
	{
		Vector2 v = CameraControl.GetLookScreenPoint();
		DrawBox(new Rect(v.x - 1, v.y/2 - 5, 3, 10), Color.red);
		DrawBox(new Rect(v.x - 5, v.y/2 - 1, 10, 3), Color.red);
	}

	void DrawTargetCrosshair()
	{
		Player player = Core.selectedPlayer;
		if(player != null)
		{
			GameObject target = player.GetTarget();
			if(target != null)
			{
				Vector3 targetPos = target.transform.position;
				Vector2 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos);

				float crosshairSize = Mathf.Ceil(160.0f / Vector3.Distance(Camera.main.transform.position, targetPos));
				DrawBox(new Rect(targetScreenPos.x - crosshairSize/2, 
				                 Screen.height - targetScreenPos.y - crosshairSize/2, 
				                 crosshairSize, crosshairSize), 
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
