using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
	public static readonly int Paused = 0;
	public static readonly int Speaking = 1;
	public static readonly int Playing = 2;

	private static int currentState = Playing;

	public static int GetState()
	{
		return currentState;
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape))
		{
			if(!GameState.IsPlaying()) GameState.ChangeState(GameState.Playing);
			else GameState.ChangeState(GameState.Paused);
		}
	}

	public static void ChangeState(int s)
	{
		OnStateChange(currentState, s);
		currentState = s;
	}

	public static bool IsPaused() { return currentState == Paused; }
	public static bool IsSpeaking() { return currentState == Speaking; }
	public static bool IsPlaying() { return currentState == Playing; }

	private static void OnPausedStart()
	{
		PauseCanvasManager.OnPauseStart();
	}
	
	private static void OnSpeakingStart()
	{
		NPCCanvasManager.OnSpeakingStart();
	}

	private static void OnPlayingStart()
	{

	}

	private static void OnStateChange(int prevState, int currentState)
	{
		if(prevState == Paused)
		{
			PauseCanvasManager.OnPauseFinish();
		}

		if(currentState == Paused)
		{
			PauseCanvasManager.OnPauseStart();
		}
		
		if(prevState == Speaking)
		{
			NPCCanvasManager.OnSpeakingFinish();
		}

		if(currentState == Speaking)
		{
			NPCCanvasManager.OnSpeakingStart();
		}
	}
	
	public static bool AllPlayersDead()
	{
		foreach(Player p in Core.playerList) if(!p.IsDead()) return false;
		return true;
	}

}
