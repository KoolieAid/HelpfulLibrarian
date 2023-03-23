using TMPro;
using UnityEngine;

namespace Memory_Game.Book_Subclasses
{
    [DisallowMultipleComponent]
    public class TitleBook : Mem_Book
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private Color highlightColor;
        public override void SetUp()
        {
            if (info.highlightWord == "")
            {
                Debug.LogWarning($"No highlight word in {info}");
                text.text = info.title;
                return;
            }

            var newTitle = info.title.Replace(info.highlightWord, $"<color=#{ColorUtility.ToHtmlStringRGBA(highlightColor)}>{info.highlightWord}</color>");
            text.text = newTitle;
        }
    }
}
