using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
    public string name = "teleporter";
    public string sceneDestiny = "Nombre de la escena a la que ir";
    public string teleporterDestiny = "Nombre del teleporter al que ir";

    void Start()
    {

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
