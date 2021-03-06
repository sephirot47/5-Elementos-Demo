﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

class PlayerComboAttack : ControlledCombo
{
    Dictionary<ComboStep, PlayerAttack> stepAttack;

    public PlayerComboAttack(String name, ComboManager cb) : base(name, cb)
    {
        stepAttack = new Dictionary<ComboStep, PlayerAttack>();
    }

    public override void Initialize()
    {
        base.Initialize();
        foreach(PlayerAttack pa in stepAttack.Values) pa.Initialize();
    }

    public void AppendStep(ComboStep step, PlayerAttack attack)
    {
        base.AppendStep(step);
        stepAttack.Add(step, attack);
    }

    public PlayerAttack GetPlayerAttack(ComboStep step)
    {
        if (stepAttack.ContainsKey(step)) return stepAttack[step];
        else return null;
    }
}
