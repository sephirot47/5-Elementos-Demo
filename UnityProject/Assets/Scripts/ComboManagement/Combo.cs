using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo
{
	[SerializeField]
	private List<ComboStep> steps;
	private float time = 0.0f; //Para contar el tiempo pasado
	private float delay = 1.0f; //Maximo delay entre pulsaciones
	private string name = "No name";
	private int currentStep = 0;
	private bool started = false;

	public Combo(string name)
	{
		this.name = name;
		steps = new List<ComboStep>();
	}

	public Combo(string name, ComboStep[] comboSteps)
	{
		this.name = name;
		steps = new List<ComboStep>();
		steps.AddRange(comboSteps);
	}

	public Combo(string name, float delay)
	{
		this.name = name;
		this.delay = delay;
		steps = new List<ComboStep>();
	}

	public Combo(string name, float delay, ComboStep[] comboSteps)
	{
		this.name = name;
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
			ComboManager.OnComboDone(name);
			ResetCombo();
		}

		time += Time.deltaTime;

		if(steps[currentStep].BeingDone() && !started)
		{
			started = true;
			ComboManager.OnComboStarted(name);
		}

		steps[currentStep].Update();

		//Para evitar que el comboo se corte cuando se tiene que pulsar una tecla
		//durante un tiempo mayor que el delay del combo :)
		if(steps[currentStep].BeingDone())
		{
			time = 0.0f;
			if(!started)
			{
				started = true;
				ComboManager.OnComboStarted(name);
			}
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
	}

	public void ResetCombo()
	{
		currentStep = 0;
		time = 0.0f;
		started = false;

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

	public string GetName()
	{
		return name;
	}
}
