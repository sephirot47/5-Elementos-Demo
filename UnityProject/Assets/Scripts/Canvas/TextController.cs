using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextController : MonoBehaviour 
{
	public float charsPerSecond = 15.0f;

	private float time;
	private Text textbox;
	private string fullText, currentText;

	void Start()
	{
		time = 0.0f;
		fullText = currentText = "";
		textbox = GetComponent<Text> ();
	}

	void Update()
	{
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
	}

	public void ShowText(string text)
	{
		currentText = "";
		fullText = text;
	}

	public void ShowInstantText(string text)
	{
		currentText = fullText = text;
	}
}
