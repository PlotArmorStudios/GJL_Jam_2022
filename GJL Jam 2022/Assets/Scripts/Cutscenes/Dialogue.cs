using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Dialogue
{
    public string[] Names;
    
    [TextArea(3, 10)] public string[] Sentences;

    public int[] NumberOfContinues;
}