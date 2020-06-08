using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Object/Quest", fileName = "QuestSO")]
    public class QuestSO : ScriptableObject
    {
        [field : SerializeField,
                 Tooltip("Name of Quest")]
        private string nameQuest;

        /// <summary>
        /// Name of Quest
        /// </summary>
        public string NameQuest => nameQuest;

        [field : SerializeField,
                 Tooltip("All objectives for this quest")]
        private List<ObjectiveSO> objective;

        /// <summary>
        /// All objectives for this quest
        /// </summary>
        public List<ObjectiveSO> Objective => objective;

        [field : SerializeField,
                 Tooltip("Reward sum for submitting quest")]
        private int rewardQuest;

        /// <summary>
        /// Reward sum for submitting quest
        /// </summary>
        public int RewardQuest => rewardQuest;
    }
}
