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
    }

    public override void Update()
    {
        if (!enabled) return;

        Debug.Log("GOaaaES");
        if (started && currentStep < steps.Count)
        {
            steps[currentStep].Update();
        }
        else timeDelay = 0.0f;
    }
}
