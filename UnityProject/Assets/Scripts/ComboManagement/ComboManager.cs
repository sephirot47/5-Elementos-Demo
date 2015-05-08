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
		ComboInputClick leftClick = new ComboInputClick(ComboInputClick.LEFT);

		Combo aerialJumpCombo = new Combo(1.0f);
		aerialJumpCombo.AppendStep( new ComboStep(t, 1.0f) );
		aerialJumpCombo.AppendStep( new ComboStep(t, 1.0f) );

		ComboStep lastStep = new ComboStep (t, 1.0f);
		lastStep.SetIsLast(true);

		aerialJumpCombo.AppendStep(lastStep);

		Combo mecCombo = new Combo(1.0f);
		mecCombo.AppendStep( new ComboStep(g) );
		mecCombo.AppendStep( new ComboStep(g) );
		mecCombo.AppendStep( new ComboStep(y) );
		mecCombo.AppendStep( new ComboStep(t) );

		Combo mecCombo2 = new Combo(1.0f);
		mecCombo2.AppendStep( new ComboStep(g) );
		mecCombo2.AppendStep( new ComboStep(g) );
		mecCombo2.AppendStep( new ComboStep(t) );
		mecCombo2.AppendStep( new ComboStep(leftClick) );



		combos.Add(aerialJumpCombo);
		combos.Add(mecCombo);
		combos.Add(mecCombo2);
	}
	
	void Update()
	{
		foreach(Combo combo in combos)
		{
			combo.Update();
		}
	}
}
