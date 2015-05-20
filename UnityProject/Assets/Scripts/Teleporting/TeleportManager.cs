using UnityEngine;
using System.Collections;

public class TeleportManager 
{
    public static void TeleportTo(string sceneName, string teleporterName)
    {
        Application.LoadLevel(sceneName);
    }
}
