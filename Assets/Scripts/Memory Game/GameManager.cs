using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Memory_Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
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

        private void Awake()
        {
            Instance = this;
        }

        #region Debug Parts

        public List<Mem_Book> cards;
        [Header("Debugging")]
        [Min(0)]
        public int size;

        public void PrintMemory()
        {
            StringBuilder builder = new();
            foreach (var item in memory)
            {
                builder.Append(item.name);
                builder.Append("\n");
            }
            Debug.Log(builder);
        }

        private void Update()
        {
            size = memory.Count;
        }
        #endregion

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            // List<Mem_Book> cards = new();
            for (var i = 0; i < booksToSpawn.Length; i++)
            {
                BookInfo info = booksToSpawn[i];
                var imageCard = Instantiate(imagePrefab, bookParent.transform).GetComponent<Mem_Book>();
                imageCard.info = info;
                imageCard.ChangeCovers(covers[Random.Range(0, covers.Length)]);
                imageCard.onTouch.AddListener(book => Instance.Pick(book));
                cards.Add(imageCard);

                var textCard = Instantiate(textPrefab, bookParent.transform).GetComponent<Mem_Book>();
                textCard.info = info;
                textCard.ChangeCovers(covers[Random.Range(0, covers.Length)]);
                textCard.onTouch.AddListener(book => Instance.Pick(book));
                cards.Add(textCard);
                
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
            
            // TODO: set up locations
            Assert.IsTrue(cards.Count <= rows * columns, "Cards are more than the number of grid");

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
        }

        private void SpawnSingle(GameObject o, BookInfo info)
        {
            var obj = Instantiate(o, transform).GetComponent<Mem_Book>();
            obj.info = info;
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
                // obj.FlipOver();
                memory.ForceRemove(obj);
                return;
            }
            
            memory.Push(obj);

            if (memory.Count >= 2)
            {
                // Compare both items in memory
                Debug.Log("Compare time");
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
                PrintMemory();
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
                }
            }
            else
            {
                DiscardAll();
            }
        }
    }
}
