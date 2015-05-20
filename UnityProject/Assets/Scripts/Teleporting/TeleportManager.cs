using UnityEngine;
using System.Collections;

public class TeleportManager 
{
    public static string lastDestinyTeleporterName = "";

    public static void TeleportTo(string sceneName, string teleporterName)
    {
        Application.LoadLevel(sceneName);
        lastDestinyTeleporterName = teleporterName;
    
    }
}
