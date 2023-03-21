using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Book")]
public class BookScriptableObject : ScriptableObject
{
    public string title;
    public Keyword keyword;
    public string description;
}
