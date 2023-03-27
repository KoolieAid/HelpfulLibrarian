using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory_Game
{
    public class Stack<T> : IEnumerable<T>
    {
        private List<T> list = new List<T>();
        
        public int Count
        {
            get => list.Count;
        }

        public void Push(T obj)
        {
            list.Add(obj);
        }

        public T Pop()
        {
            // This part is copied
            if (list.Count > 0)
            {
                T temp = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return temp;
            }
            else
                return default(T);
        }

        public bool ForceRemove(T obj)
        {
            return list.Remove(obj);
        }

        public bool Contains(T obj)
        {
            return list.Contains(obj);
        }

        public void Clear()
        {
            list.Clear();
        }

        public T[] ToArray()
        {
            return list.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var obj in list)
            {
                yield return obj;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
