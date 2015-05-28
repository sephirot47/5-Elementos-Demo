using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ICustomAnimationListener
{
    void OnAnimationStarted(CustomAnimation anim);
    void OnAnimationFinished(CustomAnimation anim);
}
