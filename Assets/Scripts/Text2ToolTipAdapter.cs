using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tutorial
{
    [AddComponentMenu("Tutorial/Tool Tip Adapter x2")]
    public class Text2ToolTipAdapter : ToolTipAdapter
    {
        [SerializeField] private TextMeshProUGUI secondTextBox;

        public void SetSecondText(string t)
        {
            secondTextBox.text = t;
        }
    }

}
