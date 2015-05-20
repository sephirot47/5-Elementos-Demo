using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
    public string name = "teleporter";
    public string sceneDestiny = "Nombre de la escena a la que ir";
    public string teleporterDestiny = "Nombre del teleporter al que ir";

    void Start()
    {
        if(TeleportManager.lastDestinyTeleporterName == name)
            ReceiveTeleporting();
    }

    void ReceiveTeleporting()
    {
        Transform kaji = Core.GetSubGameObject(gameObject, "KajiPos").transform;
        Transform zap = Core.GetSubGameObject(gameObject, "ZapPos").transform;
        Transform lluvia = Core.GetSubGameObject(gameObject, "LluviaPos").transform;

        Core.kaji.transform.position = kaji.position;
        Core.kaji.transform.rotation = kaji.rotation;
        Core.zap.transform.position = zap.position;
        Core.zap.transform.rotation = zap.rotation;
        Core.lluvia.transform.position = lluvia.position;
        Core.lluvia.transform.rotation = lluvia.rotation;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Player p = col.gameObject.GetComponent<Player>();
            if(p.IsSelected())
                TeleportManager.TeleportTo(sceneDestiny, teleporterDestiny);
        }
        
    }
}
