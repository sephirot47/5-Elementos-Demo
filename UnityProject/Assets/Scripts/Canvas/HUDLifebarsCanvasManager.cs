using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDLifebarsCanvasManager : MonoBehaviour 
{
	private static GameObject kajiLifebar, zapLifebar, lluviaLifebar;
	private static List<Vector3> initialPositions;

	void Start()
	{
		kajiLifebar = Core.GetSubGameObject(gameObject, "KajiLifebar");
		zapLifebar = Core.GetSubGameObject(gameObject, "ZapLifebar");
		lluviaLifebar = Core.GetSubGameObject(gameObject, "LluviaLifebar");

		initialPositions = new List<Vector3>();
		initialPositions.Add(kajiLifebar.transform.position);
		initialPositions.Add(zapLifebar.transform.position);
		initialPositions.Add(lluviaLifebar.transform.position);
	}

	void Update() 
	{
		if(GameState.IsSpeaking())
		{
			HideLifebars();
		}
		else if(GameState.IsPlaying())
		{
			ShowLifebars();
		}
	}

	public static void OnPlayerSelected(Player p)
	{
	}

	private static void ShowLifebars() 
	{
		CanvasUtils.Show(kajiLifebar);
		CanvasUtils.Show(zapLifebar);
		CanvasUtils.Show(lluviaLifebar);
	}
	
	private static void HideLifebars() 
	{
		CanvasUtils.Hide(kajiLifebar);
		CanvasUtils.Hide(zapLifebar);
		CanvasUtils.Hide(lluviaLifebar);
	}
}
