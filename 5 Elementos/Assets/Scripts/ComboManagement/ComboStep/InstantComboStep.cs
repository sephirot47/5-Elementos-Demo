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
                if(animation != null) animation.Play();
                parentCombo.OnStepStarted(this);
            }
        }
        else
        {
            time += Time.deltaTime;
            parentCombo.OnStepDoing(this, time);
            float duration = (animation == null ? 0.0f : animation.GetDuration());
            if( time > duration - ControlledComboStep.blend)
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
