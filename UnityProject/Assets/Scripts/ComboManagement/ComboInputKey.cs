using UnityEngine;
using System.Collections;

public class ComboInputKey : IComboInput
{
	private KeyCode key; 

	public ComboInputKey(KeyCode key)
	{
		this.key = key;
	}

	public bool Down()
	{
		return Input.GetKeyDown(key);
	}

	public bool Pressed()
	{
		return Input.GetKeyDown(key) || Input.GetKey(key);
	}

	public bool Up()
	{
		return Input.GetKeyUp(key);
	}	
	
	public bool IsClick()
	{
		return false;
	}	

	public string GetId()
	{
		return key.ToString();
	}	
}
