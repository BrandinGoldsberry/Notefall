using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class SongLoader
    {
        public static Song LoadSong(string JSONPath) 
        {
            string loadedJson = File.ReadAllText(JSONPath);
            return JsonUtility.FromJson<Song>(loadedJson);
        }
    }
}
