using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    [System.Serializable]
    public class TimeLineFrameData : Cloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FrameCount { get; set; }
        public int FrameStart { get; set; }
        public int FrameEnd { get; set; }

    }


}

