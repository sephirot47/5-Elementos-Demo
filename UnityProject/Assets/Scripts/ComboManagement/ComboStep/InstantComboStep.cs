using System;
using UnityEngine;
using System.Collections.Generic;

class InstantComboStep : ComboStep
{
    public InstantComboStep(String name, IComboInput inputDown, PlayerAnimation anim) 
                     : base(name, inputDown, anim)
    {
    }

    public InstantComboStep(String name, IComboInput inputDown, IComboInput[] inputSimultaneous, PlayerAnimation anim)
                     : base(name, inputDown, inputSimultaneous, anim)
    {
    }

    public override void Update()
    {
        if (!started)
        {
            if (inputDown.Down() && AllSimultaneousPressed() && NoWrongControlKeysPressed())
            {
                started = true;
                animation.Play();
                parentCombo.OnStepStarted(this);
            }
        }
        else
        {
            time += Time.deltaTime;

            if( time > animation.GetDuration() - ComboStep.blend)
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
