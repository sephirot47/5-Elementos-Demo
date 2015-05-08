using UnityEngine;
using System.Collections;

public interface IComboInput 
{
	bool Down();
	bool Pressed();
	bool Up();

	bool IsClick();
	string GetId();
}
