using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class NoteTime
    {
        public float Time;
        public int SpawnLocation;

        public NoteTime() { }

        public NoteTime(float time, int spawnLocation)
        {
            Time = time;
            SpawnLocation = spawnLocation;
        }
    }
}
