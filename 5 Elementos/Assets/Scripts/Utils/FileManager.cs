using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileManager
{
	private static readonly string npcRootDir = "Data/NPC/",
								   npcFileExtension = "txt",
								   npcNameTag = "NAME",
								   npcTextTag = "TEXT",
								   npcFinishFieldTag = ";";
	
	public static string GetNPCName(NPC npc)
	{
		string file = npcRootDir + npc.GetId() + "." + npcFileExtension;
		List<string> lines = ReadFile(file);
		return GetAttributes(npcNameTag, lines)[0];
	}

	public static List<string> GetNPCTexts(NPC npc)
	{
		string file = npcRootDir + npc.GetId() + "." + npcFileExtension;
		List<string> lines = ReadFile(file);
		return GetAttributes(npcTextTag, lines);
	}

	private static List<string> GetAttributes(string tag, List<string> fileLines)
	{
		List<string> result = new List<string>();
		for(int i = 0; i < fileLines.Count; ++i)
		{
			string line = fileLines[i];
			
			if(line == tag)
			{
				string text = "";

				while(++i < fileLines.Count && fileLines[i] != npcFinishFieldTag)
					text += fileLines[i] + "\r\n";
				
				result.Add(text);
			}
		}

		return result;
	}

	private static List<string> ReadFile(string file)
	{
		List<string> lines = new List<string>();
		string[] linesArray = System.IO.File.ReadAllLines(file);
		lines.AddRange(linesArray);
		return lines;
	}
}
