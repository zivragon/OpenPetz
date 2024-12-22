using Godot;
using System;
using System.Collections.Generic;

public static class TextureManager {
	
	private static Dictionary<string, Texture2D> fetchedTextures = new Dictionary<string, Texture2D>();
	
	private static Texture2D emptyTexture = null;
	
	//Methods
	
	private static Texture2D CreateEmptyTexture()
	{
		//Hexadecimal of 10 is in fact, 0x0A
		byte[] pixel = new byte[] { 0x0A };
		
		Image emptyImage = Image.CreateFromData(1, 1, false, Image.Format.R8, pixel);
		
		var textureImg = ImageTexture.CreateFromImage(emptyImage);
		
		return textureImg as Texture2D;
	}
	
	public static Texture2D FetchEmptyTexture()
	{
		if (emptyTexture == null)
			emptyTexture = CreateEmptyTexture();
		return emptyTexture; 
	}
	
	public static Texture2D FetchTexture(string path)
	{
		//Lazy initialize the empty Texture2D
		if (emptyTexture == null)
			emptyTexture = CreateEmptyTexture();
		
		if (fetchedTextures.ContainsKey(path))
		{
			return fetchedTextures[path];
		}
		
		//Try to load the Bitmap
		
		BMP image = new BMP();
		
		image.LoadFile(path);
		
		Texture2D texture = image.GetData();
		
		//if it fails, then cache it as an empty texture
		if (texture == null)
			texture = emptyTexture;
			
		fetchedTextures.Add(path, texture);
		
		return texture;
	}

}

