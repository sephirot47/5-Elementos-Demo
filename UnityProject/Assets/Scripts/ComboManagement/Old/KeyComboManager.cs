using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyComboManager : MonoBehaviour
{
	public static List<KeyCombo> combos = new List<KeyCombo>();

	public static void Init() 
	{
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.W, KeyCode.W}, 0.5, "forwardBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.D, KeyCode.D}, 0.5, "rightBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.A, KeyCode.A}, 0.5, "leftBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.S, KeyCode.S}, 0.5, "backBoost", Core.kaji) );
	}
	
	void Update() 
	{
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;

		foreach(KeyCombo c in combos) c.Update();
	}
}
