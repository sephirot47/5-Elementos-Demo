using UnityEngine;
using System.Collections;

public class CanvasUtils
{
	public static void Show(GameObject go) 
	{
		if(go.GetComponent<CanvasGroup>() != null)
		{
			go.GetComponent<CanvasGroup>().alpha = 1; 
		}
		else if(go.GetComponent<CanvasRenderer>() != null)
		{
			go.GetComponent<CanvasRenderer>().SetAlpha(1); 
		}
	}

	public static void Show(GameObject go, float alpha) 
	{
		if(go.GetComponent<CanvasGroup>() != null)
		{
			go.GetComponent<CanvasGroup>().alpha = alpha; 
		}
		else if(go.GetComponent<CanvasRenderer>() != null)
		{
			go.GetComponent<CanvasRenderer>().SetAlpha(alpha); 
		}
	}

	public static void Hide(GameObject go) 
	{
		if(go.GetComponent<CanvasGroup>() != null)
		{
			go.GetComponent<CanvasGroup>().alpha = 0; 
		}
		else if(go.GetComponent<CanvasRenderer>() != null)
		{
			go.GetComponent<CanvasRenderer>().SetAlpha(0); 
		}
	}

	public static void ShowCursor()
	{
		Cursor.visible = true;
	}

	public static void HideCursor()
	{
		Cursor.visible = false;
	}
}
