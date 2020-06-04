using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Object/Quest", fileName = "QuestSO")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] 
        private string nameQuest;

        public string NameQuest => nameQuest;

        [SerializeField] 
        private List<ObjectiveSO> objective;

        public List<ObjectiveSO> Objective => objective;

        [SerializeField] 
        private int rewardQuest;

        public int RewardQuest => rewardQuest;
    }
}
