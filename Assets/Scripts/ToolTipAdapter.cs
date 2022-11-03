using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tutorial
{
    /// <summary>
    /// Attach this adaptor to a prefab for use in tool tips.
    /// </summary>
    public class ToolTipAdapter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textMeshProUGUI;
        // Start is called before the first frame update
        void Start()
        {
            if (!textMeshProUGUI)
            {
                Debug.LogError("No UI Object found. Expect errors.");
            }
        }

        public void SetText(string text)
        {
            textMeshProUGUI.text = text;
        }
    }

}
