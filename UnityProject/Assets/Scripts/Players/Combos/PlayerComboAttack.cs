using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

class PlayerComboAttack : ControlledCombo
{
    Dictionary<ControlledComboStep, PlayerAttack> stepAttack;

    public PlayerComboAttack(String name, ComboManager cb) : base(name, cb)
    {
        stepAttack = new Dictionary<ControlledComboStep, PlayerAttack>();
    }

    public void AppendStep(ControlledComboStep step, PlayerAttack attack)
    {
        base.AppendStep(step);
        stepAttack.Add(step, attack);
    }

    public PlayerAttack GetPlayerAttack(ControlledComboStep step)
    {
        if (stepAttack.ContainsKey(step)) return stepAttack[step];
        else return null;
    }
}
