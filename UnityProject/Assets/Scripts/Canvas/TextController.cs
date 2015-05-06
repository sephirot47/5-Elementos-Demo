using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextController : MonoBehaviour 
{
	private Text textbox;

	private float time;
	public float charsPerSecond = 40.0f;

	//Sirve para evitar que pille el input del boton de hablar
	//En la primera frame del update, porque si no hacemos esto
	//lo que hace es que toda la primera parte del texto se imprime de
	//golpe
	private bool frameWhereShowDemanded;

	private List<string> textParts;
	private int currentPart;
	private string fullText, currentText;

	private GameObject nextPartArrow; //flecha de pasar texto

	void Start()
	{
		textbox = GetComponent<Text>();
		nextPartArrow = Core.GetSubGameObject(transform.parent.gameObject, "NextPartArrow");

		time = 0.0f;

		textParts = new List<string>();
		currentPart = 0;
		fullText = currentText = "";
		frameWhereShowDemanded = false;
	}

	void Update()
	{
		if(Input.GetButtonDown("Speak") && !frameWhereShowDemanded)
		{
			if(fullText.Length == currentText.Length)  Next(); //Si pulsa, avanzamos el texto
			else currentText = fullText; //Pasar rapido :)
		}

		if(currentText.Length < fullText.Length)
		{
			time += Time.deltaTime;
			if(time > 1.0f/charsPerSecond)
			{
				currentText += fullText.ToCharArray()[currentText.Length];
				time = 0.0f;
			}
		}
		textbox.text = currentText;

		if(currentText.Length == fullText.Length && currentPart < textParts.Count - 1)
		{
			//Si ya esta el texto completo y aun quedan partes por ensenar...
			CanvasUtils.Show(nextPartArrow);
		}
		else
		{
			CanvasUtils.Hide(nextPartArrow);
		}

		frameWhereShowDemanded = false;
	}

	public void ShowText(string text)
	{
		textParts.Clear();

		textParts = GetTextParts(text);

		currentText = "";
		currentPart = 0;
		fullText = textParts[0];
		frameWhereShowDemanded = true;
	}
	
	public void Next()
	{
		if(currentPart < textParts.Count - 1) 
		{
			++currentPart;
			currentText = "";
			fullText = textParts[currentPart];
		}
		else
		{
			//Ya no hay mas, cerramos
			GameState.ChangeState(GameState.Playing);
		}
	}

	public void ShowInstantText(string text)
	{
		currentText = fullText = text;
	}

	//Obtiene las diferentes partes en las que el texto se tendra que separar si es mas
	//grande de lo que cabe en el
	private List<string> GetTextParts(string text)
	{
		List<string> result = new List<string>();
		
		int i = 0;
		while(text.Length > 0)
		{
			textbox.text = "";

			//Vamos poniendo letra a letra en el textbox para comprobar cuando se pasa de largo
			//Y asi podemos detectar las partes en las que ensenarlo
			i = 0;
			while(!TextOverflowed() && i < text.Length)
			{
				textbox.text += text[i];
				++i;
			}
			--i; //Esto deja a la i en el primer caracter de la siguiente parte

			//Cogemos todos menos el ultimo caracter(porque hemos overfloweado)
			int final = textbox.text.Length;
			string part = text.Substring(0, Mathf.Max(0, final) );
			part = part.Trim(new char[]{'\n', '\r'}); //Quitamos saltos de linea al principio y final
			
			result.Add(part);

			text = text.Substring( Mathf.Min(i, text.Length), text.Length - i ); //de i pal final
			text = text.TrimStart(new char[]{'\n', '\r'}); //Quitamos saltos de linea del PRINCIPIO solo
		}

		return result;
	}

	private bool TextOverflowed()
	{
		//Nos dice si el texto se esta escribiendo por debajo, es decir, si
		//ya se ha salido de la textbox (en vertical)
		return textbox.preferredHeight > textbox.rectTransform.rect.height;
	}
}
