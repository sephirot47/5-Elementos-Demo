using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyCombo 
{
	public static KeyCode[] allKeys =
	{
		KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
		KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
		KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space,
		KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
		KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
		KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.LeftShift, KeyCode.RightShift,
		KeyCode.Less, KeyCode.Greater, KeyCode.Tab
	};

	public List< Pair<KeyCode, bool> > keys;
	public double timeBetweenKeys;

	private double timePassed;

	Player player;
	string name;

	public KeyCombo(KeyCode[] keys, double timeBetweenKeys, string comboName, Player p)
	{
		this.keys = new List< Pair<KeyCode, bool> >();
		foreach(KeyCode k in keys)
			this.keys.Add( new Pair<KeyCode, bool>(k, false) );

		this.timeBetweenKeys = timeBetweenKeys;

		name = comboName;
		player = p;
	}

	public void Update()
	{
		if(Core.selectedPlayer != player) return;

		timePassed += Time.deltaTime;

		for(int i = 0; i < allKeys.Length; ++i)
		{
			KeyCode k = allKeys[i];
			if( Input.GetKeyDown(k) ) NotifyKey(k);
		}
		
		if(timePassed >= timeBetweenKeys)
		{
			timePassed = 0.0;
			Reset();
		}
	}

	public bool Done()
	{
		foreach(Pair<KeyCode, bool> k in keys) if(!k.second) return false;
		return true;
	}

	private void NotifyKey(KeyCode keyPressed)
	{
		foreach(Pair<KeyCode, bool> k in keys)
		{
			if(!k.second) //Hemos llegado a la siguiente tecla que toca pulsar
			{
				if(k.first == keyPressed)
				{
					player.OnKeyComboKeyDown(name, k.first); //Notificamos al jugador que se ha pulsado una tecla del combo
					timePassed = 0.0;
					k.second = true;
				}
				else
				{
					//Si ha pulsado alguna tecla que es del combo pero no toca ahora, acaba el combo
					foreach(Pair<KeyCode, bool> otherKey in keys)
					{
						if(otherKey.first == keyPressed)
						{
							Reset();
						}
					}
				}

				break;
			}
		}

		if(Done())
		{
			player.OnKeyComboDone(name); //Notificamos que el combo se ha hecho :)
			Reset();
		}
	}
	
	public void Reset()
	{
		foreach(Pair<KeyCode, bool> k in keys) k.second = false;
		timePassed = 0.0;
	}
}
