using System;
using UnityEngine;
using System.Collections.Generic;

class InstantComboStep : ComboStep
{
    public InstantComboStep(String name, IComboInput inputDown, PlayerAnimation anim) : base(name, anim)
    {
        this.inputDown = inputDown; 
    }

    public InstantComboStep(String name, IComboInput inputDown, IComboInput[] inputSimultaneous, PlayerAnimation anim) : this(name, inputDown, anim)
    {
        this.inputSimultaneous.AddRange(inputSimultaneous);
    }

    public override void Update()
    {
        if (!started)
        {
            if (inputDown.Down() && AllSimultaneousPressed())
            {
                started = true;
                animation.Play();
                parentCombo.OnStepStarted(this);
            }
        }
        else
        {
            time += Time.deltaTime;

            if( time > animation.GetDuration() )
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
