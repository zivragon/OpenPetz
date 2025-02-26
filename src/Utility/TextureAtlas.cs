using Godot;
using System;
using System.Collections.Generic;
using OpenPetz;

public partial class TextureAtlas : Node2D { //TO DO: Replace with Node
    
    public static Vector2I MaximumSize => new Vector2I(1024, 1024);
    
    private SubViewport subViewport = null;
    private List<Texture2D> textureList = null;
    private List<SubTextureContainer> subTexList = new List<SubTextureContainer>();
    
    private Guid guid;
    
    public Texture2D TextureData { get; private set; } = null;
    
    public Vector2I Size { get; private set; } = new Vector2I(1024, 64);
    
    public TextureAtlas(Guid _guid, List<OpenPetz.Linez.Entries.Texture> _textureList)
    {
        //First step is checking to see if it is already cached.
        
        string fileName = "./cache/texture_atlas/raster/"+_guid.ToString()+".png";
        
        if (FileAccess.FileExists(fileName))
        {
            // Load the cache instead
            /*Bmp bitmap = new Bmp();
            
            bitmap.LoadFile(fileName, Bmp.LoadType.Raster);
            
            Size = new Vector2I(bitmap.Width, bitmap.Height);
            
            TextureData = bitmap.GetData();*/
            
            Image img = Image.LoadFromFile(fileName);
            TextureData = ImageTexture.CreateFromImage(img);
            
        } else {
            //We are bound to dynamically generating it now using SubViewport.
            subViewport = new SubViewport();
            subViewport.Size = new Vector2I(1024, 64);
            subViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Always;
            AddChild(subViewport);
            
            PackTextures();
        }
    }
    
    public override void _Ready()
    {
        if (subViewport != null)
            RenderingServer.FramePostDraw += SaveGeneratedAtlas;
    }
    
    // CUSTOM METHODS
    
    public SubTextureCoordinations GetSubTextureCoords(int _index, int _color)
    {
        var subTex = subTexList[_index];
        
        if (subTex.Transparency == 0)
        {
            return new SubTextureCoordinations(subTex.Dest.X / Size.X, subTex.Dest.Y / Size.Y, subTex.Size.X / Size.X, subTex.Size.Y / Size.Y);
        }
        
        if (subTex.Transparency == 1)
        {
            int color = _color / 10;
            float moveByX = subTex.Size.X * color + subTex.Dest.X;
            return new SubTextureCoordinations(moveByX / Size.X, subTex.Dest.Y / Size.Y, subTex.Size.X / Size.X, subTex.Size.Y / Size.Y);
        }
        
        return new SubTextureCoordinations(subTex.Dest.X / Size.X, subTex.Dest.Y / Size.Y, subTex.Size.X / Size.X, subTex.Size.Y / Size.Y);
    }
    
    // HEAVILY WIP
    
    private void PackTextures()
    {
        var texture = TextureManager.FetchTexture("./art/textures/flower.bmp");
		var palette = PaletteManager.FetchPalette("oddballz");
		
		var dummyMesh = new MeshInstance2D();
		
		var immediateMesh = new ImmediateMesh();
		var material = ShaderManager.FetchShaderMaterial("texture_atlas/texture");
		
		dummyMesh.Mesh = immediateMesh;
		dummyMesh.Material = material;
		subViewport.AddChild(dummyMesh);
		
		immediateMesh.ClearSurfaces();
		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);
		
		immediateMesh.SurfaceAddVertex(new Vector3(0, 0, 0));
		immediateMesh.SurfaceAddVertex(new Vector3(0, 63, 0));
		immediateMesh.SurfaceAddVertex(new Vector3(1023, 63, 0));
		
		immediateMesh.SurfaceAddVertex(new Vector3(0, 0, 0));
		immediateMesh.SurfaceAddVertex(new Vector3(1023, 0, 0));
		immediateMesh.SurfaceAddVertex(new Vector3(1023, 63, 0));
		
		immediateMesh.SurfaceEnd();
		
		material.SetShaderParameter("tex", texture);
		material.SetShaderParameter("palette", palette);
		
		var subTex = new SubTextureContainer();
		subTex.Dest = new Vector2(0.0f,0.0f);
		subTex.Size = new Vector2(64.0f,64.0f);
		subTex.Transparency = 0;
		
		subTexList.Add(subTex);
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
        RemoveChild(subViewport);
        subViewport = null;
        
        Image img2 = Image.LoadFromFile("./cache/texture_atlas/raster/"+guid.ToString()+".png");
        TextureData = ImageTexture.CreateFromImage(img2);
    }
}

//

internal struct SubTextureContainer {
	public SubTextureContainer()
	{
		;
	}

	public Vector2 Dest { get; set; } = new Vector2(0.0f, 0.0f);
    public Vector2 Size { get; set; } = new Vector2(1.0f, 1.0f);
    public int Transparency { get; set; } = 0;
}

public struct SubTextureCoordinations {
    
    public Vector2 Dest { get; private set; } = new Vector2(0.0f, 0.0f);
    public Vector2 Size { get; private set; } = new Vector2(1.0f, 1.0f);
    
    public SubTextureCoordinations (float _x, float _y, float _width, float _height)
    {
        Dest = new Vector2(_x, _y);
        Size = new Vector2(_width, _height);
    }
}

internal class TextureListCache {
    
}