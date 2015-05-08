using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo
{
	[SerializeField]
	private List<ComboStep> steps;
	private float time = 0.0f; //Maximo delay entre clicks
	private float delay = 1.0f; //Maximo delay entre clicks

	private int currentStep = 0;

	public Combo()
	{
		steps = new List<ComboStep>();
	}

	public Combo(ComboStep[] comboSteps)
	{
		steps = new List<ComboStep>();
		steps.AddRange(comboSteps);
	}

	public Combo(float delay)
	{
		this.delay = delay;
		steps = new List<ComboStep>();
	}

	public Combo(float delay, ComboStep[] comboSteps)
	{
		this.delay = delay;

		steps = new List<ComboStep>();
		steps.AddRange(comboSteps);
	}

	//Must be called by the ComboManager :)
	public void Update()
	{
		if(Done()) 
		{
			//Combo done
			Debug.LogError("DONE");
			Debug.Break();
			Debug.DebugBreak();
			ResetCombo();
		}

		time += Time.deltaTime;

		steps[currentStep].Update();

		//Para evitar que el comboo se corte cuando se tiene que pulsar una tecla
		//durante un tiempo mayor que el delay del combo :)
		if(steps[currentStep].BeingDone())
		{
			time = 0.0f;
		}
        else if (time > delay) //Reset
		{
			ResetCombo();
		}

        if(steps[currentStep].Cancelled())
        {
			ResetCombo();
		}
        else
        {
			//Solo un step por frame
			if(steps[currentStep].Done())
			{
				//Yay, step hecho, vamos al siguiente!!!
				++currentStep; 
				time = 0.0f; //El time del delay se reinicia obviamente
			}
		}

		//Debug.Log(time);
	}

	public void ResetCombo()
	{
		Debug.Log("Combo reseted");
		currentStep = 0;
		time = 0.0f;

		foreach(ComboStep step in steps)
		{
			step.Reset();
		}
	}
	
	public void SetDelay(float d)
	{
		delay = d;
	}

	public void AppendStep(ComboStep step)
	{
		steps.Add(step);
	}

	public bool Done()
	{
		return currentStep >= steps.Count;
	}
}
