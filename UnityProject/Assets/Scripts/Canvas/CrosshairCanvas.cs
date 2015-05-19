using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairCanvas : MonoBehaviour 
{
    public static readonly int CursorKaji = 0, CursorZap = 1, CursorLluvia = 2, CursorDefault = 3;

    private static GameObject kajiCrosshair, zapCrosshair, lluviaCrosshair, defaultCrosshair;

    private static readonly float cursorAlpha = 0.8f;

	void Start () 
    {
        kajiCrosshair = Core.GetSubGameObject(gameObject, "KajiCrosshair");
        zapCrosshair = Core.GetSubGameObject(gameObject, "ZapCrosshair");
        lluviaCrosshair = Core.GetSubGameObject(gameObject, "LluviaCrosshair");
        defaultCrosshair = Core.GetSubGameObject(gameObject, "DefaultCrosshair");

        HideAll();
	}
	
	void Update () 
    {
        Refresh();
	}

    public static void Refresh()
    {
        HideAll();
        UpdateCursorPos();
        ChangeCursor(Core.selectedPlayer);
    }

    public static void ChangeCursor(Player p)
    {
        HideAll();
        UpdateCursorPos();
        GameObject target = p.GetTarget();
        if(target != null &&
           Vector3.Dot(target.transform.position - Camera.main.transform.position, Camera.main.transform.forward) < 0)
        {
            //Si el target no lo ve la camara, return
            return;
        }

        if (target != null)
        {
            Vector2 targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);

            if (target.CompareTag("Enemy"))
            {
                if (p == Core.kaji) CanvasUtils.Show(kajiCrosshair, cursorAlpha);
                else if (p == Core.zap) CanvasUtils.Show(zapCrosshair, cursorAlpha);
                else CanvasUtils.Show(lluviaCrosshair, cursorAlpha);
            }
            else CanvasUtils.Show(defaultCrosshair, cursorAlpha);
        }
        else CanvasUtils.Show(defaultCrosshair, cursorAlpha);
    }

    private static void UpdateCursorPos()
    {
        GameObject target = Core.selectedPlayer.GetTarget();

        if (target == null)
        {
            HideAll();
            return;
        }
        
        Vector2 pos = Camera.main.WorldToScreenPoint(target.transform.position);
        float scale = 15.0f / Vector3.Distance(Camera.main.transform.position, target.transform.position);

        kajiCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        kajiCrosshair.GetComponent<RectTransform>().transform.localScale = new Vector3(scale, scale, scale);

        zapCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        zapCrosshair.GetComponent<RectTransform>().transform.localScale = new Vector3(scale, scale, scale);

        lluviaCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        lluviaCrosshair.GetComponent<RectTransform>().transform.localScale = new Vector3(scale, scale, scale);

        defaultCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        defaultCrosshair.GetComponent<RectTransform>().transform.localScale = new Vector3(scale, scale, scale);
    }

    private static void HideAll()
    {
        CanvasUtils.Hide(kajiCrosshair);
        CanvasUtils.Hide(zapCrosshair);
        CanvasUtils.Hide(lluviaCrosshair);
        CanvasUtils.Hide(defaultCrosshair);
    }
}
