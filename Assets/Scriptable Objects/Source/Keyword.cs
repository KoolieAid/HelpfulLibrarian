using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Keyword")]
public class Keyword : ScriptableObject
{
    public Sprite image;
    
    [Tooltip("Filipino Version")]
    public string wordVersion;
    
    [Tooltip("English Version")]
    public string wordTranslation;
}
