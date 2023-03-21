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
        for (int i = 0; i < stackedBooks.Length; i++)
        {
            if (i < pairedBooks.Length)
            {
                stackedBooks[i].SetBooksInStack(pairedBooks[i].book1, pairedBooks[i].book2);
            }
            else
            {
                stackedBooks[i].GetComponent<Collider2D>().enabled = false;
            }

        }
    }
}
