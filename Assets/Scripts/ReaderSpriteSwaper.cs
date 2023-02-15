using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaderSpriteSwaper : MonoBehaviour
{
    [SerializeField] private Reader reader;
    [SerializeField] private Image spriteRendered;

    public ReaderSpriteLibrary[] readerSpriteLibrary;

    private Sprite happySprite;
    private Sprite fineSprite;
    private Sprite angrySprite;

    public void SetReaderSprites()
    {
        int randReader = (int)Random.Range(0f, 4f);

        happySprite = readerSpriteLibrary[randReader].GetSprite("Happy");
        fineSprite = readerSpriteLibrary[randReader].GetSprite("Fine");
        angrySprite = readerSpriteLibrary[randReader].GetSprite("Angry");

        spriteRendered.sprite = happySprite;
    }

    private void Update()
    {
        if(reader.GetPatience() <= 0.2f)
        {
            spriteRendered.sprite = angrySprite;
        }
        else if (reader.GetPatience() <= 0.5f)
        {
            spriteRendered.sprite = fineSprite;
        }
    }
}
