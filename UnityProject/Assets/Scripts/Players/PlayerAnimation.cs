using UnityEngine;
using System.Collections;

public class PlayerAnimation
{
    private Animation anim;
	private string name;

	public PlayerAnimation(string animationName, Animation anim)
	{
        this.anim = anim;
		this.name = animationName;
	}

	public string GetName() { return name; }

    public void Play()
    {
        if(anim != null && anim.GetClip(name) != null && !IsPlaying()) 
            anim.CrossFade(name);
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

    public void Stop()
    {
        if ( IsPlaying() )
            anim.Stop();
    }
}
