using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlledCombo : Combo
{
	private float timeDelay = 0.0f; //Para contar el tiempo pasado

	public ControlledCombo(string name, ComboManager cb) : base(name, cb, 0.5f)
	{
        delay = ControlledComboStep.blend * 2.0f;
	}
}
