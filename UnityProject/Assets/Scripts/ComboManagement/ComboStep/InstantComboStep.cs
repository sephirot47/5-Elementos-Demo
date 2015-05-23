using System;
using UnityEngine;
using System.Collections.Generic;

//Es un click de tecla
class InstantComboStep : ControlledComboStep
{
    public InstantComboStep(String name, IComboInput inputDown, CustomAnimation anim) 
                     : base(name, inputDown, anim)
    {
    }

    public InstantComboStep(String name, IComboInput inputDown, IComboInput[] inputSimultaneous, CustomAnimation anim)
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
            parentCombo.OnStepDoing(this, time);
            if( time > animation.GetDuration() - ControlledComboStep.blend)
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
