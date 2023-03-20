using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BookPairs
{
    public BookScriptableObject book1;
    public BookScriptableObject book2;
}
public class SortingGameManager : MonoBehaviour
{
    [SerializeField] private BookPairs[] pairedBooks;
    [SerializeField] private BookStack[] stackedBooks;

    private void Start()
    {
        GetPairedBooks();// testing only
    }
    void GetPairedBooks()
    {
        Debug.Log("GetPairedBooks()");
        for (int i = 0; i < stackedBooks.Length; i++)
        {
            Debug.Log("Loop");
            if (i < pairedBooks.Length)
            {
                Debug.Log("Book Pair");
                stackedBooks[i].SetBooksInStack(pairedBooks[i].book1, pairedBooks[i].book2);
            }
            else
            {
                Debug.Log("No Book Pair");
                stackedBooks[i].GetComponent<Collider2D>().enabled = false;
            }

        }
    }
}
