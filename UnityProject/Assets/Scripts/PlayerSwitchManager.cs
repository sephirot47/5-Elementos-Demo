using UnityEngine;
using System.Collections;

public class PlayerSwitchManager : MonoBehaviour 
{
	void Start () 
	{
		SelectPlayer(Core.kaji);
	}
	
	void Update () 
	{
		if(GameState.IsPlaying())
		{
			if(Core.selectedPlayer != null && Core.selectedPlayer.IsDead()) 
			{
				SwitchPlayer(true);
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha1) && !Core.kaji.IsDead()) SelectPlayer(Core.kaji);
			else if (Input.GetKeyDown(KeyCode.Alpha2) && !Core.zap.IsDead()) SelectPlayer(Core.zap);
			else if (Input.GetKeyDown(KeyCode.Alpha3) && !Core.lluvia.IsDead()) SelectPlayer(Core.lluvia);
			
			else if (Input.GetKeyDown(KeyCode.Q)) SwitchPlayer(true);
			else if (Input.GetKeyDown(KeyCode.E)) SwitchPlayer(false);
		}
	}

	void SwitchPlayer(bool right)
	{
		Player lastPlayerSelected = Core.selectedPlayer;

		int step = right ? 1 : -1, 
		i = (Core.selectedPlayer == null) ? 0 : Core.playerList.IndexOf(Core.selectedPlayer);

		int counter = 0;
		i = (i + Core.playerList.Count * 2 + step) % Core.playerList.Count;
		while( (i <= Core.playerList.Count && i >= 0) || counter <= Core.playerList.Count)
		{
			int indexOfNewPlayer = (i + Core.playerList.Count * 2) % Core.playerList.Count;
			if(!Core.playerList[indexOfNewPlayer].IsDead())
			{
				SelectPlayer( Core.playerList[indexOfNewPlayer] );
				break;
			}
			i += step;
			++counter;
		}
	}

	void SelectPlayer(Player p)
	{
		Player lastPlayerSelected = Core.selectedPlayer;

		if(p == null) return;
		Core.selectedPlayer = p;
		Camera.main.GetComponent<CameraControl>().SelectTarget(p.gameObject.transform);

		if(lastPlayerSelected != Core.selectedPlayer) //Si realmente ha habido cambio de player
		{
			HUDLifebarsCanvasManager.OnPlayerSelected(p);
			ComboManager.OnPlayerSelectedChange();
		}
	}
}
