using UnityEngine;
using System.Collections;

public class PlayerAnimation
{
	public enum Priority { Low = 0, Medium = 1, High = 2, VeryHigh = 3 }; 

	private string name;
	private Priority priority = Priority.Low;

	public PlayerAnimation(string animationName, Priority p)
	{
		this.name = animationName;
		priority = p;
	}

	public string GetName() { return name; }
	public Priority GetPriority() { return priority; }
}
