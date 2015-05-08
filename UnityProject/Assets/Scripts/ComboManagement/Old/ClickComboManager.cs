using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickComboManager : MonoBehaviour
{
	private static int nClicks = 0;
	private static double maxDelay = 1.0, time = 0.0;

	private static KeyCode[] allControlKeys =
	{
		KeyCode.LeftControl, KeyCode.RightControl, 
		KeyCode.LeftAlt, KeyCode.RightAlt, 
		KeyCode.LeftShift, KeyCode.RightShift,
		KeyCode.Less, KeyCode.Greater, KeyCode.Tab
	};

	void Update() 
	{
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;

		time += Time.deltaTime;
		if (time >= maxDelay) Reset();

		if(Core.selectedPlayer == null) return;

		KeyCode heldControlKey = KeyCode.None; //Pillamos la tecla de control pulsada
		for(int i = 0; i < allControlKeys.Length; ++i) 
			if(Input.GetKey( allControlKeys[i] )) 
				heldControlKey = allControlKeys[i];

		if(Input.GetMouseButtonDown(0))
		{
			++nClicks;
			Core.selectedPlayer.OnClickComboDown(heldControlKey, nClicks, 0);
		}
		else if(Input.GetMouseButtonDown(1))
		{
			Core.selectedPlayer.OnClickComboDown(heldControlKey, nClicks, 1);
		}
		else if(Input.GetMouseButton(0)) //Mantenido
		{
			++nClicks;
			Core.selectedPlayer.OnClickCombo(heldControlKey, nClicks, 0);
		}
		else if(Input.GetMouseButton(1)) //Mantenido
		{
			Core.selectedPlayer.OnClickCombo(heldControlKey, nClicks, 1);
		}
	}

	private void Reset()
	{
		time = 0.0;
		nClicks = 0;	
	}
}
