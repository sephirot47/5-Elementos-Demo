using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDLifebarsCanvasManager : MonoBehaviour 
{
    private static GameObject kajiLifebar, zapLifebar, lluviaLifebar;

    private static Vector3 kajiLifebarPos, zapLifebarPos, lluviaLifebarPos;
    private static Vector3 kajiLifebarScale, zapLifebarScale, lluviaLifebarScale;

    private static Vector3 firstPlanePos, secondPlanePos, thirdPlanePos;
    private static Vector3 firstPlaneScale, secondPlaneScale, thirdPlaneScale;

    public float transitionSpeed = 5.0f;

	void Start()
	{
		kajiLifebar = Core.GetSubGameObject(gameObject, "KajiLifebar");
		zapLifebar = Core.GetSubGameObject(gameObject, "ZapLifebar");
		lluviaLifebar = Core.GetSubGameObject(gameObject, "LluviaLifebar");

        firstPlanePos = kajiLifebar.GetComponent<RectTransform>().position;
        secondPlanePos = zapLifebar.GetComponent<RectTransform>().position;
        thirdPlanePos = lluviaLifebar.GetComponent<RectTransform>().position;

        firstPlaneScale = kajiLifebar.GetComponent<RectTransform>().localScale;
        secondPlaneScale = zapLifebar.GetComponent<RectTransform>().localScale;
        thirdPlaneScale = lluviaLifebar.GetComponent<RectTransform>().localScale;
	}

	void Update() 
	{
		if(GameState.IsSpeaking())
		{
			HideLifebars();
		}
		else if(GameState.IsPlaying())
		{
			ShowLifebars();
		}

        Debug.Log(firstPlanePos);

        //LERP
        kajiLifebar.GetComponent<RectTransform>().position = Vector3.Lerp(kajiLifebar.GetComponent<RectTransform>().position, kajiLifebarPos, Time.deltaTime * transitionSpeed);
        kajiLifebar.GetComponent<RectTransform>().localScale = Vector3.Lerp(kajiLifebar.GetComponent<RectTransform>().localScale, kajiLifebarScale, Time.deltaTime * transitionSpeed);

        zapLifebar.GetComponent<RectTransform>().position = Vector3.Lerp(zapLifebar.GetComponent<RectTransform>().position, zapLifebarPos, Time.deltaTime * transitionSpeed);
        zapLifebar.GetComponent<RectTransform>().localScale = Vector3.Lerp(zapLifebar.GetComponent<RectTransform>().localScale, zapLifebarScale, Time.deltaTime * transitionSpeed);

        lluviaLifebar.GetComponent<RectTransform>().position = Vector3.Lerp(lluviaLifebar.GetComponent<RectTransform>().position, lluviaLifebarPos, Time.deltaTime * transitionSpeed);
        lluviaLifebar.GetComponent<RectTransform>().localScale = Vector3.Lerp(lluviaLifebar.GetComponent<RectTransform>().localScale, lluviaLifebarScale, Time.deltaTime * transitionSpeed);
	    //

        UpdateLifebarOrder();
    }

    private void UpdateLifebarOrder()
    {
        kajiLifebar.transform.SetSiblingIndex(GetSiblingOrder(kajiLifebar));
        zapLifebar.transform.SetSiblingIndex(GetSiblingOrder(zapLifebar));
        lluviaLifebar.transform.SetSiblingIndex(GetSiblingOrder(lluviaLifebar));
    }

    private int GetSiblingOrder(GameObject lifebar)
    {
        int order = 2;
        if (lifebar != kajiLifebar && 
            lifebar.transform.localScale.magnitude < kajiLifebar.transform.localScale.magnitude) 
            --order;

        if (lifebar != zapLifebar &&
            lifebar.transform.localScale.magnitude < zapLifebar.transform.localScale.magnitude)
            --order;

        if (lifebar != lluviaLifebar &&
            lifebar.transform.localScale.magnitude < lluviaLifebar.transform.localScale.magnitude)
            --order;

        return order;
    }

	public static void OnPlayerSelected(Player p)
	{
        if(p == Core.kaji)
        {
            kajiLifebarPos = firstPlanePos;
            kajiLifebarScale = firstPlaneScale;

            lluviaLifebarPos = secondPlanePos;
            lluviaLifebarScale = secondPlaneScale;

            zapLifebarPos = thirdPlanePos;
            zapLifebarScale = thirdPlaneScale;
        }
        else if (p == Core.zap)
        {
            kajiLifebarPos = secondPlanePos;
            kajiLifebarScale = secondPlaneScale;

            lluviaLifebarPos = thirdPlanePos;
            lluviaLifebarScale = thirdPlaneScale;

            zapLifebarPos = firstPlanePos;
            zapLifebarScale = firstPlaneScale;
        }
        else if (p == Core.lluvia)
        {
            kajiLifebarPos = thirdPlanePos;
            kajiLifebarScale = thirdPlaneScale;

            lluviaLifebarPos = firstPlanePos;
            lluviaLifebarScale = firstPlaneScale;

            zapLifebarPos = secondPlanePos;
            zapLifebarScale = secondPlaneScale;
        }
	}

	private static void ShowLifebars() 
	{
		CanvasUtils.Show(kajiLifebar);
		CanvasUtils.Show(zapLifebar);
		CanvasUtils.Show(lluviaLifebar);
	}
	
	private static void HideLifebars() 
	{
		CanvasUtils.Hide(kajiLifebar);
		CanvasUtils.Hide(zapLifebar);
		CanvasUtils.Hide(lluviaLifebar);
	}
}
