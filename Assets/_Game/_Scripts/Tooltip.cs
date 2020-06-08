using System;
using TMPro;
using UnityEngine;

namespace _Game._Scripts
{
    public class Tooltip : MonoBehaviour
    {
        /// <summary>
        /// Tooltip Text
        /// </summary>
        [field : SerializeField,
                 Tooltip("Tooltip Text")]
        private TextMeshProUGUI tooltipText;

        private void Start()
        {
            GetComponent<SampleQuestManager>().TooltipActionEvent += TooltipText;
        }

        /// <summary>
        /// Set text in the tooltip
        /// </summary>
        /// <param name="text"></param>
        private void TooltipText(string text)
        {
            tooltipText.text = text;
        }
    }
}
