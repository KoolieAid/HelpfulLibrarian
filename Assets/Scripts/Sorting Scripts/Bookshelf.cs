using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   public enum Topics 
    {
        Topic1,
        Topic2,
        Topic3,
        Topic4,
        Topic5,
        Topic6,
        Topic7,
    };

public class Bookshelf : MonoBehaviour
{
 
    [SerializeField] private Topics category;

    public delegate void CheckBookAction(bool isCorrect, string name);
    public static event CheckBookAction OnSort;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BookStack bookStack = collision.GetComponent<BookStack>();
        if (collision.GetComponent<BookStack>() && bookStack.category == category)
        {
            if (OnSort != null)
                OnSort(true, bookStack.name);
        }
        else
        {
            if (OnSort != null)
                OnSort(false, bookStack.name);
        }
    }
}
