using Godot;
using System;
using System.Collections.Generic;
using OpenPetz;

public partial class TextureAtlas : Node2D { //TO DO: Replace with Node
    
    public static Vector2I MaximumSize => new Vector2I(1024, 1024);
    
    private SubViewport subViewport = null;
    private List<TextureParams> textureList = null;
	
    private List<SubTextureContainer> subTexList = new List<SubTextureContainer>();
	//private List<SubTextureContainer> subTexListTransparent = new List<SubTextureContainer>();
    
    private Guid guid;
	
    public Texture2D TextureData { get; private set; } = null;
	public Texture2D Palette { get; private set; } = null;
    
    public Vector2I Size { get; private set; } = new Vector2I(128, 128);
    
    public TextureAtlas(Texture2D _palette, Guid _guid, List<TextureParams> _textureList)
    {
        //First step is checking to see if it is already cached.
        
		guid = _guid;
		Palette = _palette;
		
		textureList = _textureList;
		
        string fileName = "./cache/texture_atlas/raster/"+_guid.ToString()+".png";
        
        /*if (FileAccess.FileExists(fileName))
        {
            // Load the cache instead
            
            Image img = Image.LoadFromFile(fileName);
            img.Convert(Image.Format.R8);
            TextureData = ImageTexture.CreateFromImage(img);
            
        } else */{
            //We are bound to dynamically generating it now using SubViewport.
            subViewport = new SubViewport();
            subViewport.Size = new Vector2I(256, 128);
            subViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Once;
            AddChild(subViewport);
            
            PackTextures();
        }
    }
    
    public override void _Ready()
    {
        if (subViewport != null)
		{
            RenderingServer.FramePostDraw += SaveGeneratedAtlas;
		}
    }
    
    // CUSTOM METHODS
    
    public SubTextureCoordinations GetSubTextureCoords(int _index, int _color)
    {
        var subTex = subTexList[_index];
		
		GD.Print(subTex.Size);

        return new SubTextureCoordinations(subTex.Position.X / Size.X, subTex.Position.Y / Size.Y, (subTex.Size.X / 2f) / Size.X, subTex.Size.Y / Size.Y);
    }
    
    // HEAVILY WIP
    
    private void PackTextures()
    {
		Vector2 posPtr = new Vector2(0f, 0f);
		foreach (var texParam in textureList){
			var texture = TextureManager.FetchTexture(texParam.Path);
			var texSize = texture.GetSize();
			
			var dummyMesh = new MeshInstance2D();
			
			var material = ShaderManager.FetchShaderMaterial("texture_atlas/texture");
			
			dummyMesh.Mesh = MeshManager.FetchNormalMesh();
			dummyMesh.Material = material;
			subViewport.AddChild(dummyMesh);
			
			material.SetShaderParameter("tex", texture);
			material.SetShaderParameter("position", posPtr);
			material.SetShaderParameter("size", texSize);
			
			var subTex = new SubTextureContainer();
			subTex.Position = posPtr;
			subTex.Size = texSize;
			//
			subTex.Transparency = 1;
			
			subTexList.Add(subTex);
			
			posPtr.X += texSize.X;
		}
    }
    
    private void SaveGeneratedAtlas()
    {
        Texture2D tex = subViewport.GetTexture();
        
        Image img = tex.GetImage();
        img.Convert(Image.Format.R8);
        
        img.SavePng("./cache/texture_atlas/raster/"+guid.ToString()+".png");
        //Then unsubscribe
        RenderingServer.FramePostDraw -= SaveGeneratedAtlas;
        //Get rid of the subViewport
		RemoveChild(subViewport); // Godot complains too much
        subViewport = null; 
        
        Image img2 = Image.LoadFromFile("./cache/texture_atlas/raster/"+guid.ToString()+".png");
        img2.Convert(Image.Format.R8);
		TextureData = ImageTexture.CreateFromImage(img2);
    }
}

//

public struct TextureParams {
	public int Index = 0;
	public string Path {get; set;} = "";
    public int Transparency {get; set;} = 0;
	public int Color {get; set;} = 0;
	public TextureParams(){}
}

internal struct SubTextureContainer {
	public SubTextureContainer()
	{
		;
	}

	public Vector2 Position { get; set; } = new Vector2(0.0f, 0.0f);
    public Vector2 Size { get; set; } = new Vector2(1.0f, 1.0f);
    public int Transparency { get; set; } = 0;
}

public struct SubTextureCoordinations {
    
    public Vector2 Position { get; private set; } = new Vector2(0.0f, 0.0f);
    public Vector2 Size { get; private set; } = new Vector2(1.0f, 1.0f);
    
    public SubTextureCoordinations (float _x, float _y, float _width, float _height)
    {
        Position = new Vector2(_x, _y);
        Size = new Vector2(_width, _height);
    }
}

internal class TextureListCache {
    
}
