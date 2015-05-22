using UnityEngine;
using System.Collections;

public class CustomAnimation
{
    private Animation anim;
	private string name;

	public CustomAnimation(string animationName, Animation anim)
	{
        this.anim = anim;
		this.name = animationName;
	}

	public string GetName() { return name; }

    public void Play()
    {
        if (anim != null && anim.GetClip(name) != null && !IsPlaying())
        {
            anim.CrossFade(name);
        }
    }

    public void Play(float fadeTime)
    {
        if (anim != null && anim.GetClip(name) != null && !IsPlaying())
            anim.CrossFade(name, fadeTime);
    }

    public bool IsPlaying()
    {
        return anim != null && anim.GetClip(name) != null && anim.IsPlaying(name);
    }

    public float GetDuration()
    {
        if (anim != null && anim.GetClip(name) != null)
            return anim.GetClip(name).length;
        return 0.0f;
    }
}
