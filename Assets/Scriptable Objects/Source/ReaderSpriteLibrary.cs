using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sprite Library/Reader Sprites")]

public class ReaderSpriteLibrary : ScriptableObject
{
    [SerializeField] private List<sprite> Library = new List<sprite>();


    public Sprite GetSprite(string name)
    {
        
        foreach(sprite n in Library)
        {
            if(n.emotion == name)
            {
                return n.readerSprite;
            }
        }

        return null; 
    }
}

[Serializable]
public struct sprite
{
    public string emotion;
    public Sprite readerSprite;
}