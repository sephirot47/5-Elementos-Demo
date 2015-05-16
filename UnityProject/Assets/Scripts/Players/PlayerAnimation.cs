using UnityEngine;
using System.Collections;

public class PlayerAnimation
{
	private string name;

	public PlayerAnimation(string animationName)
	{
		this.name = animationName;
	}

	public string GetName() { return name; }
}
