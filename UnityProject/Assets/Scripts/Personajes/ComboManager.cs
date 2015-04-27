using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboManager : MonoBehaviour 
{
	public static List<Combo> kajiCombos = new List<Combo>();
	public static List<Combo> zapCombos = new List<Combo>();
	public static List<Combo> lluviaCombos = new List<Combo>();

	public static void Init() 
	{
		kajiCombos.Add( new Combo(new KeyCode[]{KeyCode.R, KeyCode.F, KeyCode.T}, 1.0, "patadaVoladora", Core.kaji) );
		//kajiCombos.Add( new Combo(new KeyCode[]{KeyCode.R, KeyCode.T, KeyCode.T}, 1.0, "salto", Core.kaji) );
	}
	
	void Update() 
	{
		foreach(Combo c in kajiCombos) c.Update();
		foreach(Combo c in zapCombos) c.Update();
		foreach(Combo c in lluviaCombos) c.Update();
	}
}
