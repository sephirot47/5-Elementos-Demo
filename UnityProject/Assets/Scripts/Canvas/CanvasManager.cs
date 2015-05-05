using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class CanvasManager : MonoBehaviour 
{
	private static GameObject kajiLifebar, zapLifebar, lluviaLifebar;
	private static GameObject pausePanel, pauseBackground;

	void Start()
	{
		pausePanel = GameObject.Find("PausePanel");
		pauseBackground = GameObject.Find("PauseBackground");

		kajiLifebar = GameObject.Find("KajiLifebar");
		zapLifebar = GameObject.Find("ZapLifebar");
		lluviaLifebar = GameObject.Find("LluviaLifebar");
		
		Hide(pausePanel);
		Hide(pauseBackground);
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
				Show(pauseBackground, 0.5f);
			}
		}

		if(GameState.IsSpeaking())
		{
			HideLifebars();
		}
		else if(GameState.IsPlaying())
		{
			ShowLifebars();
		}
	}

	public static void OnPauseStart()
	{
		Show(pausePanel);
		Show(pauseBackground, 0.5f);
	}

	public static void OnPauseFinish()
	{
		Hide(pausePanel);
		Hide(pauseBackground);
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

	private static void Show(GameObject go) {go.GetComponent<CanvasRenderer>().SetAlpha(1); }
	private static void Show(GameObject go, float alpha) {go.GetComponent<CanvasRenderer>().SetAlpha(alpha); }
	private static void Hide(GameObject go) {go.GetComponent<CanvasRenderer>().SetAlpha(0); }
	
	private static void ShowLifebars() 
	{
		ShowGroup(kajiLifebar);
		ShowGroup(zapLifebar);
		ShowGroup(lluviaLifebar);
	}

	private static void HideLifebars() 
	{
		HideGroup(kajiLifebar);
		HideGroup(zapLifebar);
		HideGroup(lluviaLifebar);
	}
	
	private static void ShowGroup(GameObject go) {go.GetComponent<CanvasGroup>().alpha = 1; }
	private static void ShowGroup(GameObject go, float alpha) {go.GetComponent<CanvasGroup>().alpha = alpha; }
	private static void HideGroup(GameObject go) {go.GetComponent<CanvasGroup>().alpha = 0; }
}
