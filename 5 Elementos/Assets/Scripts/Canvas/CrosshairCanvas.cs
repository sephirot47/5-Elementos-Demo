using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairCanvas : MonoBehaviour 
{
    public static readonly int CursorKaji = 0, CursorZap = 1, CursorLluvia = 2, CursorDefault = 3;

    private static GameObject kajiCrosshair, zapCrosshair, lluviaCrosshair, defaultCrosshair;

    private static Vector3 originalKajiCrosshairScale, originalZapCrosshairScale, originalLluviaCrosshairScale; 
    private static readonly float cursorScale = 12.0f;
    private static readonly float cursorAlpha = 0.8f;

	void Start ()
    {
        kajiCrosshair = Core.GetSubGameObject(gameObject, "KajiCrosshair");
        zapCrosshair = Core.GetSubGameObject(gameObject, "ZapCrosshair");
        lluviaCrosshair = Core.GetSubGameObject(gameObject, "LluviaCrosshair");
        defaultCrosshair = Core.GetSubGameObject(gameObject, "DefaultCrosshair");

        originalKajiCrosshairScale = kajiCrosshair.GetComponent<RectTransform>().localScale;
        originalZapCrosshairScale = zapCrosshair.GetComponent<RectTransform>().localScale;
        originalLluviaCrosshairScale = lluviaCrosshair.GetComponent<RectTransform>().localScale;

        HideAll();
	}
	
	void Update () 
    {
        if(!GameState.IsPlaying())
        {
            HideAll();
            return;
        }

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
        if (!GameState.IsPlaying()) return;

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

            if (p == Core.kaji) CanvasUtils.Show(kajiCrosshair, cursorAlpha);
            else if (p == Core.zap) CanvasUtils.Show(zapCrosshair, cursorAlpha);
            else CanvasUtils.Show(lluviaCrosshair, cursorAlpha);
        }
    }

    private static void UpdateCursorPos()
    {
        GameObject target = Core.selectedPlayer.GetTarget();

        if (target == null)
        {
            HideAll();
            return;
        }

        Vector3 crosshairPos = target.GetComponent<Targetable>().GetCrosshairPos();
        Vector2 pos = Camera.main.WorldToScreenPoint(crosshairPos);
        float scale = cursorScale / Vector3.Distance(Camera.main.transform.position, crosshairPos);

        kajiCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        kajiCrosshair.GetComponent<RectTransform>().transform.localScale = originalKajiCrosshairScale * scale;

        zapCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        zapCrosshair.GetComponent<RectTransform>().transform.localScale = originalZapCrosshairScale * scale;

        lluviaCrosshair.GetComponent<RectTransform>().transform.position = new Vector3(pos.x, pos.y, 0.0f);
        lluviaCrosshair.GetComponent<RectTransform>().transform.localScale = originalLluviaCrosshairScale * scale;

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
