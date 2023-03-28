using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory_Game
{
    [CreateAssetMenu(menuName = "Memory Game/Level")]
    public class Level : ScriptableObject
    {
        // I cant Nest 2 lists together like List<List<T>> and idk why dont know dont care
        // Multi dimensional arrays also dont work
        public BookInfo[] firstSet;
        public BookInfo[] secondSet;

    }
}
