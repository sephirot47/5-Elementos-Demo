using UnityEngine;
using System.Collections;

public class HUDCanvasManager : MonoBehaviour 
{
	private static GameObject kajiLifebar, zapLifebar, lluviaLifebar;

	void Start()
	{
		kajiLifebar = Core.GetSubGameObject(gameObject, "KajiLifebar");
		zapLifebar = Core.GetSubGameObject(gameObject, "ZapLifebar");
		lluviaLifebar = Core.GetSubGameObject(gameObject, "LluviaLifebar");
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
