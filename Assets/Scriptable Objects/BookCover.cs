using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Book Styles", menuName = "ScriptableObjects/Book Style ScriptableObject")]
public class BookCover : ScriptableObject
{
    public Sprite bookFront;
    public Sprite bookBack;
}
