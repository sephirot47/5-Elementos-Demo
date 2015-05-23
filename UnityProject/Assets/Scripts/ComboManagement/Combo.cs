using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Combo
{
    protected ComboManager comboManager;
    protected List<ComboStep> steps;

    protected float delay, 
                    timeDelay = 0.0f;
    protected bool started = false, enabled = true;
    protected int currentStep = 0;
    protected string name;

    public Combo(string name, ComboManager cb, float delay = 0.5f)
    {
        this.name = name;
        steps = new List<ComboStep>();
        comboManager = cb;
        this.delay = delay; //delay permitido entre step y step
    }

    public virtual void Initialize()
    {
        started = false;
        timeDelay = 0.0f;
        currentStep = 0;
        foreach (ComboStep step in steps) step.Initialize();
    }

    public virtual void Update()
    {
        if (!enabled) return;

        if (currentStep < steps.Count)
        {
            if (!steps[currentStep].Started() && currentStep > 0) //Si estamos entre step y step
            {
                timeDelay += Time.deltaTime;
                if (timeDelay > delay)
                {
                    Cancel(); //Si excede el tiempo de delay entre step y step, cancelamos
                    return;
                }
            }

            steps[currentStep].Update();
        }
        else timeDelay = 0.0f;
    }

    public void AppendStep(ComboStep step)
    {
        steps.Add(step);
        step.SetParentCombo(this);
    }

    public bool BeingDone() { return enabled && started; }
    public bool Finished() { return currentStep >= steps.Count; }
    public string GetName() { return name; }

    public void Cancel()
    {
        Initialize();
        comboManager.OnComboCancelled(this);
    }

    public void OnStepStarted(ComboStep step)
    {
        if (currentStep == 0)
        {
            started = true;
            comboManager.OnComboStarted(this);
        }
        comboManager.OnComboStepStarted(step);
    }

    public void OnStepCancelled(ComboStep step)
    {
        Cancel();
        comboManager.OnComboStepCancelled(step);
        comboManager.OnComboCancelled(this);
    }

    public void OnStepDoing(ComboStep comboStep, float timePressed)
    {
        comboManager.OnComboStepDoing(comboStep, timePressed);
    }

    public void OnStepFinished(ComboStep step)
    {
        ++currentStep;
        comboManager.OnComboStepFinished(step);
        if (currentStep < steps.Count)
        {
            steps[currentStep].Initialize();
        }
        else //Combo finished
        {
            Initialize();
            comboManager.OnComboFinished(this);
        }
    }

    protected virtual void NextStep()
    {
        ++currentStep;
        timeDelay = 0.0f;
    }

    public List<ComboStep> GetSteps() { return steps; }
    public void SetEnabled(bool enabled) { this.enabled = enabled; }
}
