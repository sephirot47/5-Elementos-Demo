using UnityEngine;
using System.Collections;

public class CustomAnimation
{
    private ICustomAnimationListener listener;
    private Animation anim;
	private string name;
    private float speed;

	public CustomAnimation(string animationName, Animation anim, float speed = 1.0f, ICustomAnimationListener listener = null)
	{
        this.anim = anim;
		this.name = animationName;
        this.listener = listener;
        this.speed = speed;
	}

	public string GetName() { return name; }

    public void Update() //LO HA DE LLAMAR EL LISTENER EN CUESTION
    {
        if (listener == null) return;

        if (anim[name].normalizedTime > 1)
        {
            listener.OnAnimationFinished(this);
        }
    }

    public void Play()
    {
        if (anim != null && anim.GetClip(name) != null && !IsPlaying())
        {
            anim[name].speed = speed;
            if(listener != null) listener.OnAnimationStarted(this);
            anim.CrossFade(name);
        }
    }

    public void ForcePlay()
    {
        if (anim != null && anim.GetClip(name) != null)
        {
            anim[name].speed = speed;
            if (listener != null) listener.OnAnimationStarted(this);
            anim.CrossFade(name);
        }
    }

    public void Play(float fadeTime)
    {
        if (anim != null && anim.GetClip(name) != null && !IsPlaying())
        {
            anim[name].speed = speed;
            if (listener != null) listener.OnAnimationStarted(this);
            anim.CrossFade(name, fadeTime);
        }
    }

    public bool IsPlaying()
    {
        return anim != null && anim.GetClip(name) != null && anim.IsPlaying(name) && anim[name].normalizedTime <= 1;
    }

    public float GetNormalizedTime()
    {
        Debug.Log(name);
        return anim[name].normalizedTime;
    }

    public float GetDuration()
    {
        if (anim != null && anim.GetClip(name) != null)
            return anim.GetClip(name).length / speed;
        return 0.0f;
    }

    public void SetListener(ICustomAnimationListener listener)
    {
        this.listener = listener;
    }
}
