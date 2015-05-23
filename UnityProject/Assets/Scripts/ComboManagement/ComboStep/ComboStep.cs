using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class ComboStep
{
    protected Combo parentCombo = null;
    protected CustomAnimation animation;
    protected float time = 0.0f;
    protected string name = "";
    protected bool started = false;

    public ComboStep(string name, CustomAnimation anim)
    {
        this.name = name;
        animation = anim;
    }

    public void Initialize()
    {
        time = 0.0f;
        started = false;
    }

    public abstract void Update();

    public void Cancel()
    {
        Initialize();
        if (parentCombo != null) parentCombo.OnStepCancelled(this);
    }

    public bool Started() {  return started;  }

    public CustomAnimation GetAnimation() { return animation; }
    public Combo GetParentCombo()  { return parentCombo; }
    public string GetName() { return name; }

    public void SetParentCombo(Combo c) {  parentCombo = c;  }

}
