using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(fileName = "TooltipText", menuName = "Scriptable Object/TooltipText")]
    public class TooltipTextSO : ScriptableObject
    {
        [field : SerializeField,
                 Tooltip("Message that shows which quest was activated")]
        private string questActive = "{0} was activated";
        
        /// <summary>
        /// Message that shows which quest was activated
        /// </summary>
        public string QuestActive => questActive;    
    
        [field : SerializeField,
                 Tooltip("Message that shows which quest was finished")]
        private string questFinished = "{0} was finished";
        
        /// <summary>
        /// Message that shows which quest was finished
        /// </summary>
        public string QuestFinished => questFinished;    
        
        [field : SerializeField,
                 Tooltip("Message that shows that there is no more quests left")]
        private string noMoreQuest = "No more quests";
        
        /// <summary>
        /// Message that shows that there is no more quests left
        /// </summary>
        public string NoMoreQuest => noMoreQuest;    
        
        [field : SerializeField,
                 Tooltip("Message that shows which objective was submitted")]
        private string objectiveSubmitted = "{0} was submitted";
        
        /// <summary>
        /// Message that shows which objective was submitted
        /// </summary>
        public string ObjectiveSubmitted => objectiveSubmitted;        
        
        [field : SerializeField,
                 Tooltip("Message that shows that objective was wrong")]
        private string wrongObjective = "Wrong objective";
        
        /// <summary>
        /// Message that shows that objective was wrong
        /// </summary>
        public string WrongObjective => wrongObjective;
    }
}
