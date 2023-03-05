using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Memory_Game
{
    public class GameManager : MonoBehaviour
    {
        private Stack<GameObject> memory = new(2);

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

        public void Pick(GameObject obj)
        {
            if (memory.Count >= 2)
            {
                Debug.LogWarning("Stack is full");
                // Maybe pop all items? or reset
                DiscardAll();
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

        public void DiscardAll()
        {
            memory.Clear();
        }

        public void Compare()
        {
            var copy = memory.ToArray();

            if (copy.Length >= 2)
            {
                Debug.LogError($"Idk how you got here but congrats, there are more than 2 in the memory stack");
                PrintMemory();
                return;
            }
            
            // TODO: Actual comparison
            
        }
    }
}
