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

		CanvasManager.HideGroup(speakingPanel);
	}
	
	void Update() 
	{
	}

	public static void OnSpeakingStart()
	{
		CanvasManager.ShowGroup(speakingPanel);

		if(speakingNPC != null)
		{
			speakingTitle.text = speakingNPC.GetName();
			speakingText.GetComponent<TextController>().ShowText(speakingNPC.GetText());
		}
	}

	public static void OnSpeakingFinish()
	{
		CanvasManager.HideGroup(speakingPanel);
	}

	public static void SetSpeakingNPC(NPC npc)
	{
		speakingNPC = npc;
	}
}
