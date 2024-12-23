using Godot;
using System;
using System.Collections.Generic;

public static class ShaderManager {
	private static Dictionary<string, Shader> fetchedShaders = new Dictionary<string, Shader>();
	
	//temporary
	private static string textureShaderCode = "";
	//To do: improve this
	private static Dictionary<string, Shader> fetchedComponentShaders = new Dictionary<string, Shader>();
	
	public static Shader FetchShader(string name)
	{
		if (fetchedShaders.ContainsKey(name))
		{
			return fetchedShaders[name];
		}
		
		if (textureShaderCode == ""){
			using var compFile = FileAccess.Open("./shaders/components/texture.shader", FileAccess.ModeFlags.Read);
			textureShaderCode = compFile.GetAsText();
		}
		
		try{
			using var file = FileAccess.Open("./shaders/"+name+".shader", FileAccess.ModeFlags.Read);
			
			string code = file.GetAsText();
			
			code = code.Replace("@LoadTextureShaderComponent", textureShaderCode);
			
			Shader shader = new Shader();
			
			shader.Code = code;
			
			return shader;
		} catch( Exception e){
			return null;
		}
	}
	
	public static ShaderMaterial FetchShaderMaterial(string name)
	{
		var shader = ShaderManager.FetchShader(name);
		
		var shaderMaterial = new ShaderMaterial();
		
		shaderMaterial.Shader = shader;
		
		return shaderMaterial;
	}
}

