using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Keyword")]
public class Keyword : ScriptableObject
{
    public Sprite image;
    public string request;
    
    [Tooltip("Words to be highlighted in the request. CASE SENSITIVE")]
    public List<string> highlightedWords;
}
