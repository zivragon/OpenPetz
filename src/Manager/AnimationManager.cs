using Godot;
using System;
using System.Collections.Generic;
using OpenPetz.src.anim;

public static class AnimationManager {
	
	private static Bhd CatBhd { get; set; } = null;
	
	//Methods
	
	private static void LoadCatBhd()
	{
		
	}
	
  public static Bhd FetchCatBhd()
    {
        List<string> bdtFiles = new List<string>();
        for (int i = 0; i <= 492; i++)
        {
            bdtFiles.Add($"./ptzfiles/cat/CAT{i}.bdt");
        }

        CatBhd = new Bhd("./ptzfiles/cat/CAT.bhd", bdtFiles);

        if (CatBhd == null)
        {
            GD.Print("Failed to load CAT.bhd");
        }
		
		return CatBhd;
    }

}