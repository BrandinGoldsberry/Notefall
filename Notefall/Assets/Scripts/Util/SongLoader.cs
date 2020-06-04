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
        public static Song LoadSong(string SongName) 
        {
            TextAsset loadedJson = (TextAsset) Resources.Load(SongName, typeof(TextAsset));
            return JsonUtility.FromJson<Song>(loadedJson.text);
        }
    }
}
