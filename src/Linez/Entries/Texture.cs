using System;

namespace OpenPetz.Linez.Entries {
    public class Texture {
        public string Path {get; private set;}
        public int Transparency {get; private set;}
        
        public Texture(string _path, int _transparency)
        {
            Path = _path;
            Transparency = _transparency;
        }
    }
}