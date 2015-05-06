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

		CanvasUtils.Hide(speakingPanel);
	}
	
	void Update() 
	{
	}

	public static void OnSpeakingStart()
	{
		CanvasUtils.Show(speakingPanel);

		if(speakingNPC != null)
		{
			speakingTitle.text = speakingNPC.GetName();
			speakingText.GetComponent<TextController>().ShowText(speakingNPC.GetText());
		}
	}

	public static void OnSpeakingFinish()
	{
		CanvasUtils.Hide(speakingPanel);
	}

	public static void SetSpeakingNPC(NPC npc)
	{
		speakingNPC = npc;
	}
}
