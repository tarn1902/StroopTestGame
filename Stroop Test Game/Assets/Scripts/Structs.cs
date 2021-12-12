using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Structs
{
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
        public bool isSuccessful;
    }
}
