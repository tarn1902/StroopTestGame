using UnityEngine;
using System;

namespace Structs
{
    //Holds the string and colour
    [Serializable]
    public struct WordColour
    {
        public string word;
        public Color colour;
    }

    //Basic struct for holding records of test results
    [Serializable]
    public struct Results
    {
        public float testTime;
        public int successes;
    }
}
