using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyComboManager : MonoBehaviour
{
	public static List<KeyCombo> combos = new List<KeyCombo>();

	public static void Init() 
	{
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.W, KeyCode.W}, 1.0, "forwardBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.D, KeyCode.D}, 1.0, "rightBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.A, KeyCode.A}, 1.0, "leftBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.S, KeyCode.S}, 1.0, "backBoost", Core.kaji) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.R, KeyCode.T, KeyCode.T}, 1.0, "salto", Core.kaji) );

		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.R, KeyCode.R, KeyCode.F}, 1.0, "zapPunetazo", Core.zap) );
		combos.Add( new KeyCombo(new KeyCode[]{KeyCode.R, KeyCode.R, KeyCode.T, KeyCode.A}, 1.0, "zapPunetazo", Core.zap) );
	}
	
	void Update() 
	{
		foreach(KeyCombo c in combos) c.Update();
	}
}
