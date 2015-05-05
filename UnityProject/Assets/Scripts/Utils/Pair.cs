using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pair<T, U> 
{
	public T first;
	public U second;

	public Pair() 
	{
	}
	
	public Pair(T first, U second)
	{
		this.first = first;
		this.second = second;
	}
};