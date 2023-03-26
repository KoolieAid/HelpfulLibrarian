using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Asyncoroutine;
using UnityEngine.Events;

namespace Memory_Game
{
    public class GameManager : MonoBehaviour
    {
        private Stack<Mem_Book> memory = new();

        [SerializeField] private GameObject imagePrefab;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private BookInfo[] booksToSpawn;
        [SerializeField] private BookCover[] covers;
        [SerializeField] private GameObject bookParent;

        [Header("Grid")] 
        
        [SerializeField] [Min(1)] private int rows;
        [SerializeField] [Min(1)] private int columns;
        [SerializeField] [Min(0)] private float offsetX;
        [SerializeField] [Min(0)] private float offsetY;
        [SerializeField] private int cardsLeft;

        [SerializeField] private float holderYOffset = 0.6f;

        [Header("Patience")] [SerializeField] private Image patienceBar;
        [SerializeField] private float totalSeconds = 30f;

        [Header("Events")]
        public UnityEvent onWin;
        public UnityEvent onLose;
        private bool lvlDone = false;

        private void Start()
        {
            GenerateGrid();
            StartPatienceTimer();

            UnityAction _ = () =>
            {
                lvlDone = true;
                UIManager.uiManager.ShowLevelStatus();
            };
            
            onWin.AddListener(_);
            onLose.AddListener(_);
        }

        private async void StartPatienceTimer()
        {
            while (!lvlDone)
            {
                await new WaitForEndOfFrame();

                patienceBar.fillAmount -= (1f / totalSeconds) * Time.deltaTime;

                if (patienceBar.fillAmount <= 0)
                {
                    break;
                }
            }
            
            onLose.Invoke();
        }

        private void GenerateGrid()
        {
            List<Mem_Book> cards = new();
            for (var i = 0; i < booksToSpawn.Length; i++)
            {
                BookInfo info = booksToSpawn[i];
                SpawnSingle(imagePrefab, info, cards);
                SpawnSingle(textPrefab, info, cards);
            }
            
            // EVERYDAY IM SHUFFLIN'
            // Fisher-Yates shuffle algorithm
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                var temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
            
            Assert.IsTrue(cards.Count == rows * columns, "Cards need to be the same size with the grid size");

            var index = 0;
            for (var y = 0; y < rows; ++y)
            {
                for (var x = 0; x < columns; ++x)
                {
                    cards[index].transform.position = new Vector3((x * offsetX) - (offsetX / 2),
                        (y * offsetY) - (offsetY / 2) - 1, // idk why i need a 1 here but ok
                        0);
                    index++;
                }
            }

            cardsLeft = booksToSpawn.Length * 2;

            bookParent.transform.position += Vector3.down * holderYOffset;
        }

        private void SpawnSingle(GameObject o, BookInfo info, List<Mem_Book> cards)
        {
            var card = Instantiate(o, bookParent.transform).GetComponent<Mem_Book>();
            card.info = info;
            card.ChangeCovers(covers[Random.Range(0, covers.Length)]);
            card.onTouch.AddListener(Pick);
            cards.Add(card);
        }

        public void Pick(Mem_Book obj)
        {
            if (memory.Count >= 2)
            {
                Debug.LogWarning("Stack is full");
                // Maybe pop all items? or reset
                DiscardAll();
                Pick(obj);
                return;
            }

            if (memory.Contains(obj))
            {
                // Debug.Log("Item is already in memory, pick a new one");
                memory.ForceRemove(obj);
                return;
            }
            
            memory.Push(obj);

            if (memory.Count >= 2)
            {
                // Compare both items in memory
                Compare();
            }
        }

        public void DiscardAll()
        {
            foreach (var book in memory)
            {
                book.FlipOver();
            }
            memory.Clear();
        }

        public void Compare()
        {
            var copy = memory.ToArray();

            if (copy.Length > 2)
            {
                Debug.LogError($"Idk how you got here but congrats, there are more than 2 in the memory stack");
                return;
            }

            var (first, sec) = (copy[0], copy[1]);
            if (first.info == sec.info)
            {
                Array.ForEach(copy, b => b.Lock());
                memory.Clear();
                if ((cardsLeft -= 2) <= 0)
                {
                    Debug.Log("Player wins");
                    onWin.Invoke();
                }
            }
            else
            {
                DiscardAll();
            }
        }
    }
}
