using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaler = 1.5f;
    private float originalWidth, originalHeight;

	void Start () 
    {
        originalWidth = GetComponent<RectTransform>().rect.width;
        originalHeight = GetComponent<RectTransform>().rect.height;
	}
	
	void Update () {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(originalWidth, originalHeight) * scaler;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(originalWidth, originalHeight);
    }
}
