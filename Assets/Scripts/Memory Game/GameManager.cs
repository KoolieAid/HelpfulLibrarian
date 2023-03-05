using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory_Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Stack<GameObject> memory = new Stack<GameObject>(2);

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PrintMemory()
        {
            string buffer = "";
            foreach (var item in memory)
            {
                buffer += $"{item.name}\n";
            }
            Debug.Log(buffer);
        }

        public void Pick(GameObject obj)
        {
            if (memory.Count >= 2)
            {
                Debug.LogWarning("Stack is full");
                // May pop all items? or reset
                return;
            }

            if (memory.Contains(obj))
            {
                Debug.Log("Item is already in memory, pick a new one");
                return;
            }
            
            memory.Push(obj);

            if (memory.Count >= 2)
            {
                // Compare both items in memory
                Debug.Log("Compare time");
            }
        }
        
        
    }
}
