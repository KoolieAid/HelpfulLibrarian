using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ReaderManager : MonoBehaviour
{
    public static ReaderManager Instance;
    
    public Reader currentReader;
    private int stars = 3;

    public List<BookInfo> possibleBooks = new();
    [SerializeField] private Vector3 spawnCoords;

    private void Awake()
    {
        Instance = this;
        if (currentReader != null) return;
        NextReader();
    }

    public bool Compare(Book book)
    {
        // Debug.Log($"Request: {currentReader.requestedTitle}. Book title: {book.GetTitle()}. Evaluation: {currentReader.requestedTitle == book.GetTitle()}");
        // If title does not match
        if (currentReader.requestedTitle != book.GetTitle())
        {
            stars--;
            // Update the UI here.
            
            if (stars <= 0)
            {
                Debug.Log($"Player lost. Request: {currentReader.requestedTitle}. Book Selected: {book.GetTitle()}");
                // Restart Scene?? idk
            }

            return false;
        }

        // Correct?? Next reader pls
        Debug.Log("CORRECT");
        NextReader();
        return true;
    }

    // TODO: Obviously this is like 40% done but I'm just gonna do what makes the tutorial work anyway
    private void NextReader()
    {
        // If theres no reader currently or on start of level
        if (currentReader == null)
        {
            throw new NotImplementedException();
            // currentReader = Instantiate()
            var bookinfo = possibleBooks[Random.Range(0, possibleBooks.Count)];
            possibleBooks.Remove(bookinfo);
            
            // Instantiate a new reader with the book info
            // Instantiate();

            // TODO: Figure out the spawn locations

        }
        
        // Destroy(currentReader.gameObject);
        // currentReader = null;
        
        // If theres a reader
        
        // If theres no possible reader next
        if (possibleBooks.Count < 1)
        {
            Debug.Log("Player won");
            SceneManager.LoadScene(gameObject.scene.name);
            return;
        }
        
        // If there are possible readers left
        // Spawn the next reader
        Debug.Log("possible readers, but no spawning implemented");

    }


    [Serializable]
    public struct BookInfo
    {
        public string title;
        public string description;
    }
    
}
