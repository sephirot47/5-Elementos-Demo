using UnityEngine;
using System.Collections;

public class NPCCanvasManager : MonoBehaviour 
{
	private static GameObject speakingPanel;
	private static NPC speakingNPC = null;

	void Start() 
	{
		speakingPanel = Core.GetSubGameObject(gameObject, "SpeakingPanel");
		Hide(speakingPanel);
	}
	
	void Update() 
	{
		if(GameState.IsSpeaking() && speakingNPC != null)
		{
		}
	}

	public static void OnSpeakingStart()
	{
		Show(speakingPanel);
	}

	public static void OnSpeakingFinish()
	{
		Hide(speakingPanel);
	}

	public static void SetSpeakingNPC(NPC npc)
	{
		speakingNPC = npc;
	}

	private static void Show(GameObject go) {go.GetComponent<CanvasRenderer>().SetAlpha(1); }
	private static void Show(GameObject go, float alpha) {go.GetComponent<CanvasRenderer>().SetAlpha(alpha); }
	private static void Hide(GameObject go) {go.GetComponent<CanvasRenderer>().SetAlpha(0); }
}
