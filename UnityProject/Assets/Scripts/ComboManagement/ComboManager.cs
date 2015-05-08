using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboManager : MonoBehaviour 
{
	public static List<Combo> combos;

	void Start () 
	{
		combos = new List<Combo>();
		
		ComboInputKey y = new ComboInputKey(KeyCode.Y);
		ComboInputKey t = new ComboInputKey(KeyCode.T);
		ComboInputKey g = new ComboInputKey(KeyCode.G);

		Combo aerialJumpCombo = new Combo(1.0f);
		aerialJumpCombo.AppendStep( new ComboStep(t, 1.0f) );
		aerialJumpCombo.AppendStep( new ComboStep(t, 1.0f) );

		ComboStep lastStep = new ComboStep (t, 1.0f);
		lastStep.SetIsLast(true);

		aerialJumpCombo.AppendStep(lastStep);

		combos.Add(aerialJumpCombo);

		/*aerialJumpCombo.AppendStep( new ComboStep(t) );
		aerialJumpCombo.AppendStep( new ComboStep(y) );
		aerialJumpCombo.AppendStep( new ComboStep(g) );
		aerialJumpCombo.AppendStep( new ComboStep(y) );*/
		//aerialJumpCombo.AppendStep( new ComboStep(g, 2.0f) );
		//aerialJumpCombo.AppendStep( new ComboStep(g) );

	}
	
	void Update()
	{
		foreach(Combo combo in combos)
		{
			combo.Update();
		}

		if( combos[0].Done() )
		{
			Debug.LogError("DONE");
			Debug.Break();
			Debug.DebugBreak();
			combos[0].ResetCombo();
		}
	}
}
