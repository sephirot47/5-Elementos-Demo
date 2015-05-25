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

    private static GameObject kajiFaceActive, kajiFaceIdle;
    private static GameObject zapFaceActive, zapFaceIdle;
    private static GameObject lluviaFaceActive, lluviaFaceIdle;

    public float transitionSpeed = 5.0f;

	void Start()
    {
		kajiLifebar = Core.GetSubGameObject(gameObject, "KajiLifebar");
		zapLifebar = Core.GetSubGameObject(gameObject, "ZapLifebar");
		lluviaLifebar = Core.GetSubGameObject(gameObject, "LluviaLifebar");

        kajiLifebarPos = firstPlanePos = kajiLifebar.GetComponent<RectTransform>().position;
        zapLifebarPos = secondPlanePos = zapLifebar.GetComponent<RectTransform>().position;
        lluviaLifebarPos = thirdPlanePos = lluviaLifebar.GetComponent<RectTransform>().position;

        kajiLifebarScale = firstPlaneScale = kajiLifebar.GetComponent<RectTransform>().localScale;
        zapLifebarScale = secondPlaneScale = zapLifebar.GetComponent<RectTransform>().localScale;
        lluviaLifebarScale = thirdPlaneScale = lluviaLifebar.GetComponent<RectTransform>().localScale;

        kajiFaceActive = Core.GetSubGameObject(kajiLifebar, "FaceActive");
        kajiFaceIdle = Core.GetSubGameObject(kajiLifebar, "FaceIdle");

        zapFaceActive = Core.GetSubGameObject(zapLifebar, "FaceActive");
        zapFaceIdle = Core.GetSubGameObject(zapLifebar, "FaceIdle");

        lluviaFaceActive = Core.GetSubGameObject(lluviaLifebar, "FaceActive");
        lluviaFaceIdle = Core.GetSubGameObject(lluviaLifebar, "FaceIdle");
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
        HideAllFaces();

        if(p == Core.kaji)
        {
            CanvasUtils.Show(kajiFaceActive);
            CanvasUtils.Show(zapFaceIdle);
            CanvasUtils.Show(lluviaFaceIdle);

            kajiLifebarPos = firstPlanePos;
            kajiLifebarScale = firstPlaneScale;

            zapLifebarPos = secondPlanePos;
            zapLifebarScale = secondPlaneScale;

            lluviaLifebarPos = thirdPlanePos;
            lluviaLifebarScale = thirdPlaneScale;
        }
        else if (p == Core.zap)
        {
            CanvasUtils.Show(kajiFaceIdle);
            CanvasUtils.Show(zapFaceActive);
            CanvasUtils.Show(lluviaFaceIdle);

            kajiLifebarPos = secondPlanePos;
            kajiLifebarScale = secondPlaneScale;

            zapLifebarPos = firstPlanePos;
            zapLifebarScale = firstPlaneScale;

            lluviaLifebarPos = thirdPlanePos;
            lluviaLifebarScale = thirdPlaneScale;
        }
        else if (p == Core.lluvia)
        {
            CanvasUtils.Show(kajiFaceIdle);
            CanvasUtils.Show(zapFaceIdle);
            CanvasUtils.Show(lluviaFaceActive);

            kajiLifebarPos = thirdPlanePos;
            kajiLifebarScale = thirdPlaneScale;

            zapLifebarPos = secondPlanePos;
            zapLifebarScale = secondPlaneScale;

            lluviaLifebarPos = firstPlanePos;
            lluviaLifebarScale = firstPlaneScale;
        }
	}

    private static void HideAllFaces()
    {
        CanvasUtils.Hide(kajiFaceActive);
        CanvasUtils.Hide(kajiFaceIdle);
        CanvasUtils.Hide(zapFaceActive);
        CanvasUtils.Hide(zapFaceIdle);
        CanvasUtils.Hide(lluviaFaceActive);
        CanvasUtils.Hide(lluviaFaceIdle);
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
