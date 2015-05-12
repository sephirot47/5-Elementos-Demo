using UnityEngine;
using System.Collections;

public class ComboInputClick : IComboInput
{
	public static readonly int LEFT = 0, 
							   RIGHT =  1,
							   MIDDLE = 2;
	private int mouseButton; 
	
	public ComboInputClick(int mouseButton)
	{
		this.mouseButton = mouseButton;
	}
	
	public bool Down()
	{
		return Input.GetMouseButtonDown(mouseButton);
	}
	
	public bool Pressed()
	{
		return Input.GetMouseButtonDown(mouseButton) || Input.GetMouseButton(mouseButton);
	}
	
	public bool Up()
	{
		return Input.GetMouseButtonUp(mouseButton);
	}

	public bool IsClick()
	{
		return true;
	}	
	
	public string GetId()
	{
		if(mouseButton == LEFT) return "ClickLeft";
		else if(mouseButton == MIDDLE) return "ClickMiddle";
		return "ClickRight";
	}	
}