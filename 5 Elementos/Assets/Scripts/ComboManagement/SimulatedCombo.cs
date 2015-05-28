using System;
using UnityEngine;
using System.Collections.Generic;

public class SimulatedCombo : Combo
{                                                             
    public SimulatedCombo(string name, ComboManager cb) : base(name, cb, 0.0f)
    {
        //0 DELAY para aceptar el input, ya que no hay input xd
    }

    public void Simulate()
    {
        Initialize();
        started = true;

        SimulatedComboStep scs = null;
        try { scs = (SimulatedComboStep) steps[currentStep]; }
        catch (InvalidCastException e) { Debug.LogError("Un simulatedCombo no puede tener comboSteps no simulated");  }
        
        scs.Simulate();
    }

    public override void Update()
    {
        if (!enabled) return;

        if (started && currentStep < steps.Count)
        {
            steps[currentStep].Update();
        }
        else timeDelay = 0.0f;
    }
    
    protected override void NextStep()
    {
        ++currentStep;
        timeDelay = 0.0f;

        SimulatedComboStep scs = null;
        try { scs = (SimulatedComboStep)steps[currentStep]; }
        catch (InvalidCastException e) { Debug.LogError("Un simulatedCombo no puede tener comboSteps no simulated"); }

        scs.Simulate();
    }
}
