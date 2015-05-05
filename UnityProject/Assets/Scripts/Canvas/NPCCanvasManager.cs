using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class NPCCanvasManager : MonoBehaviour 
{
	private static GameObject speakingPanel;
	private static Text speakingTitle, speakingText;
	private static NPC speakingNPC = null;

	void Start() 
	{
		speakingPanel = Core.GetSubGameObject(gameObject, "SpeakingPanel");
		speakingTitle = Core.GetSubGameObject(speakingPanel, "Title").GetComponent<Text>();
		speakingText = Core.GetSubGameObject(speakingPanel, "Content").GetComponent<Text>();

		HideGroup(speakingPanel);
	}
	
	void Update() 
	{
	}

	public static void OnSpeakingStart()
	{
		ShowGroup(speakingPanel);

		if(speakingNPC != null)
		{
			speakingTitle.text = speakingNPC.GetName();
			speakingText.GetComponent<TextController>().ShowText(speakingNPC.GetText());
		}
	}

	public static void OnSpeakingFinish()
	{
		HideGroup(speakingPanel);
	}

	public static void SetSpeakingNPC(NPC npc)
	{
		speakingNPC = npc;
	}

	private static void Show(GameObject go) {go.GetComponent<CanvasRenderer>().SetAlpha(1); }
	private static void Show(GameObject go, float alpha) {go.GetComponent<CanvasRenderer>().SetAlpha(alpha); }
	private static void Hide(GameObject go) {go.GetComponent<CanvasRenderer>().SetAlpha(0); }

	private static void ShowGroup(GameObject go) {go.GetComponent<CanvasGroup>().alpha = 1; }
	private static void ShowGroup(GameObject go, float alpha) {go.GetComponent<CanvasGroup>().alpha = alpha; }
	private static void HideGroup(GameObject go) {go.GetComponent<CanvasGroup>().alpha = 0; }
}
