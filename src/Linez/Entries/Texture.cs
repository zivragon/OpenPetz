using System;

namespace OpenPetz.Linez.Entries {
    public class Texture {
        public string Path {get; set;}
        public int Transparency {get; set;}
        
        public Texture(string _path, int _transparency)
        {
            Path = _path;
            Transparency = _transparency;
        }
    }
}