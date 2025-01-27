using Godot;
using System;
using System.Collections.Generic;

public static class ShaderManager {

	//temporary
	private static string colorShaderCode = "";
	//also temporary
	private static string circleShaderCode = "";
	//To do: improve this
	//private static Dictionary<string, Shader> fetchedComponentShaders = new Dictionary<string, Shader>();
	
	private static Shader FetchShader(string name)
	{
		if (colorShaderCode == ""){
			using var compFile = FileAccess.Open("./shaders/components/color.shader", FileAccess.ModeFlags.Read);
			colorShaderCode = compFile.GetAsText();
		}
		
		if (circleShaderCode == ""){
			using var compFile = FileAccess.Open("./shaders/components/circle.shader", FileAccess.ModeFlags.Read);
			circleShaderCode = compFile.GetAsText();
		}
		
		try{
			using var file = FileAccess.Open("./shaders/"+name+".shader", FileAccess.ModeFlags.Read);
			
			string code = file.GetAsText();
			
			code = code.Replace("@LoadColorShaderComponent", colorShaderCode);
			code = code.Replace("@LoadCircleShaderComponent", circleShaderCode);
			
			Shader shader = new Shader();
			
			shader.Code = code;
			
			return shader;
		} catch( Exception e){
			return null;
		}
	}
	
	private static Dictionary<string, ShaderMaterial> _shaderMaterials = new Dictionary<string, ShaderMaterial>();
	
	public static ShaderMaterial FetchShaderMaterial(string name)
	{
	
		if (!_shaderMaterials.ContainsKey(name))
		{
			var shader = ShaderManager.FetchShader(name);
			var shaderMaterial = new ShaderMaterial();
			shaderMaterial.Shader = shader;
			_shaderMaterials[name] = shaderMaterial;
		}
	
		ShaderMaterial result = (ShaderMaterial)(_shaderMaterials[name].Duplicate(false));
		return result;
	}
}
