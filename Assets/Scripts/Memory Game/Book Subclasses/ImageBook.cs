using UnityEngine;

namespace Memory_Game.Book_Subclasses
{
    [DisallowMultipleComponent]
    public class ImageBook : Mem_Book
    {
        [SerializeField] private SpriteRenderer renderer;

        public override void SetUp()
        {
            renderer.sprite = info.keyword.image;
        }
    }
}
