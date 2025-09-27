using Godot;
using System;
using System.Collections.Generic;

namespace OpenPetz.Linez {
    public class Database {
        public List<Entries.BallInfo> BallzInfo {get; private set;} = new List<Entries.BallInfo>();
        
        public Database()
        {
			
        }
    }
}