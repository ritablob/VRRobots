using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage_Bit : MonoBehaviour
{
    public GarbageType type;
}

// Black = General
// Blue = Paper
// Yellow = Plastic

public enum GarbageType
{
    General,
    Paper,
    Plastic,
    BAD
}
