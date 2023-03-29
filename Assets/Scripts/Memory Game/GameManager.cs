using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;
using Asyncoroutine;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Memory_Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private Stack<Mem_Book> memory = new();

        public Level lvls;

        [SerializeField] private GameObject imagePrefab;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private BookInfo[] booksToSpawn;
        [SerializeField] private BookCover[] covers;
        [SerializeField] private GameObject bookParent;
        public int lvlIteration = 0;
        private List<Mem_Book> cards = new();

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

        [Header("UI")] [SerializeField] private Button next;


        private void Start()
        {
            Instance = this;
            try
            {
                lvls = global::GameManager.instance.levelManager.GetMinigameData();
            }
            catch
            {
                Debug.LogError("No level in the dictionary. Please check");
                return;
            }
            booksToSpawn = lvls.firstSet;
            GenerateGrid();
            StartPatienceTimer();
            next.interactable = false;
            
            
            onWin.AddListener(() =>
            {
                lvlDone = true;
                cards.ForEach(b => b.Lock());
                
                ++lvlIteration;
                // If iter is on 2, which is all done
                if (lvlIteration > 1)
                {
                    next.interactable = true;
                    UIManager.uiManager.ShowLevelStatus();
                    return;
                }
                // If not done
                NextLevel();
            });
            
            
            onLose.AddListener(() =>
            {
                lvlDone = true;
                cards.ForEach(b => b.Lock());
                
                UIManager.uiManager.ShowLevelStatus();
            });
        }

        private async void StartPatienceTimer()
        {
            while (!lvlDone)
            {
                await new WaitForEndOfFrame();

                patienceBar.fillAmount -= (1f / totalSeconds) * Time.deltaTime;

                if (patienceBar.fillAmount <= 0)
                {
                    lvlDone = true;
                    break;
                }
            }
            
            onLose.Invoke();
        }

        private void GenerateGrid()
        {
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
            
            Assert.AreEqual(cards.Count, rows * columns, "Cards need to be the same size with the grid size");

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
            
            Assert.IsFalse(copy.Length > 2, "Idk how you got here but congrats, there are more than 2 in the memory stack");

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

        public void NextLevel()
        {
            cards.ForEach(b =>
            {
                Destroy(b.gameObject);
            });
            cards.Clear();

            Assert.AreEqual(lvlIteration, 1, "Bro how u got here");

            booksToSpawn = lvls.secondSet;

            GenerateGrid();

            lvlDone = false;
            patienceBar.fillAmount = 1f;
            
            StartPatienceTimer();
        }

        public void WholeRestart()
        {
            SceneManager.LoadSceneAsync("Memory Game", LoadSceneMode.Additive).completed += _ =>
            {
                SceneManager.UnloadSceneAsync(gameObject.scene);
            };
        }

        public List<BookInfo> GetBooksToSpawn()
        {
            return lvls.firstSet.Concat(lvls.secondSet).ToList();
        }

        public void NextLevelClicked()
        {
            SortingGameManager.Instance.canvas.SetActive(true);
            SortingGameManager.Instance.ManualStart(GetBooksToSpawn());
            // await new WaitForSeconds(1);
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}
