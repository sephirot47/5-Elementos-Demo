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

	public void OnSpeakWithMe(Player p)
	{
        float d = Vector3.Distance(transform.position, p.transform.position);
        if (d <= GetComponent<NPCMovement>().visionRange)
        {
            GetComponent<Animation>().CrossFade("Speak");
            GetComponent<Animation>().CrossFadeQueued("IdleDefault");
            NPCCanvasManager.SetSpeakingNPC(this);
            transform.LookAt(p.transform.position);
            GameState.ChangeState(GameState.Speaking);
        }
	}

    public void OnSpeakingNext()
    {
        GetComponent<Animation>().CrossFade("Speak");
    }

    public void OnLeaveSpeaking()
    {
        GetComponent<Animation>().CrossFade("IdleDefault");
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