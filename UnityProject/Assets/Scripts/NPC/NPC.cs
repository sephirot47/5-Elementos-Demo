using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour 
{
	[SerializeField]
	private string id = "";

	private string npcName;
	private List<string> texts;

	void Start() 
	{
		npcName = FileManager.GetNPCName(this);
		texts = FileManager.GetNPCTexts(this);

		if(id == "")
			Debug.LogError("Este NPC no tiene ninguna id asignada!!!! Se rompera todo si no le pones alguna :(" );
	}
	
	void Update() 
	{

	}

	public void OnSpeakWithMe()
	{
		NPCCanvasManager.SetSpeakingNPC(this);
		GameState.ChangeState(GameState.Speaking);
	}

	public string GetName()
	{
		return npcName;
	}

	public string GetText()
	{
		return texts[0];
	}

	public string GetId()
	{
		return id;
	}
}