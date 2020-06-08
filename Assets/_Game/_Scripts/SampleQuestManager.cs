using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class SampleQuestManager : MonoBehaviour
    {

        #region Variables

        #region InspectorVisible

        /// <summary>
        /// All quests 
        /// </summary>
        [field : SerializeField,
                 Tooltip("All quest that will be in the game")]
        private List<QuestSO> quests;

        /// <summary>
        /// Button that starts new quest
        /// </summary>
        [field : SerializeField,
                 Tooltip("Button that starts new quest")]
        private Button activateQuestButton;

        /// <summary>
        /// Button that finish current quest
        /// </summary>
        [field : SerializeField,
                 Tooltip("Button that finish current quest")]
        private Button finishQuestButton;


        #region Objectives

        /// <summary>
        /// Button that tries to submit objective one
        /// </summary>
        [field : SerializeField,
                 Tooltip("Button that tries to submit objective one")]
        private Button objective1;

        /// <summary>
        /// Button that tries to submit objective two
        /// </summary>
        [field : SerializeField,
                 Tooltip("Button that tries to submit objective two")]
        private Button objective2;

        /// <summary>
        /// Button that tries to submit objective three
        /// </summary>
        [field : SerializeField,
                 Tooltip("Button that tries to submit objective three")]
        private Button objective3;

        /// <summary>
        /// All objectives for this example
        /// </summary>
        [field : SerializeField,
                 Tooltip("All objectives for this example")]
        private List<ObjectiveSO> allObjectives;

        /// <summary>
        /// Money display text
        /// </summary>
        [field : SerializeField,
                 Tooltip("Money display text")]
        private TextMeshProUGUI moneyText;

        #endregion
        
        #endregion
        
        #region Private

        /// <summary>
        /// Index which informs current quest count
        /// </summary>
        private int _currentQuestIndex;
        
        /// <summary>
        /// Informs if quest is currently active
        /// </summary>
        private bool _isQuestActive;
        
        /// <summary>
        /// Current quest
        /// </summary>
        private QuestSO _currentQuest;

        private float _currentMoney;
        
        /// <summary>
        /// Objectives of current quest that player have already submitted
        /// </summary>
        private List<ObjectiveSO> _submittedObjectives = new List<ObjectiveSO>();
        
        public bool IsQuestActive => _isQuestActive;
        public List<ObjectiveSO> SubmittedObjectives => _submittedObjectives;
        
        #endregion

        #endregion
        
        #region Evemts

        public delegate void QuestActive(QuestSO questSo);
        public QuestActive QuestActiveEvent;        
        
        public delegate void ActionPerformed();
        public ActionPerformed ObjectiveSubmittedEvent;
        public ActionPerformed QuestFinishedEvent;

        public delegate void TooltipAction(string text);
        public TooltipAction TooltipActionEvent;
        
        #endregion

        private void Awake()
        {
            activateQuestButton.onClick.AddListener(ActivateQuest);
            finishQuestButton.onClick.AddListener(FinishQuest);
            finishQuestButton.interactable = false;

            
            objective1.onClick.AddListener(PressToSubmit1);
            objective2.onClick.AddListener(PressToSubmit2);
            objective3.onClick.AddListener(PressToSubmit3);
            
            objective1.interactable = objective2.interactable = objective3.interactable = false;

            moneyText.text = $"{_currentMoney}";;
        }


        #region QuestManagement

        /// <summary>
        /// Activate next quest in the list using button
        /// </summary>
        private void ActivateQuest()
        {
            if (_currentQuestIndex >= quests.Count)
            {
                TooltipActionEvent?.Invoke("No more quests");
                return;
            }

            _currentQuest = quests[_currentQuestIndex];

            _isQuestActive = true;
            _currentQuestIndex++;
            
            QuestActiveEvent?.Invoke(_currentQuest);
            TooltipActionEvent?.Invoke($"{_currentQuest.NameQuest} was activated");


            activateQuestButton.interactable = false;
            finishQuestButton.interactable = true;
            
            objective1.interactable = objective2.interactable = objective3.interactable = true;

        }
        
        /// <summary>
        /// Checking if objective can be submitted
        /// </summary>
        /// <param name="objective"></param>
        private void TryToSubmitObjective(ObjectiveSO objective)
        {
            if(!IsQuestActive) return;
            
            if (_currentQuest.Objective.Contains(objective))
            {
                if (CheckDuplicate(objective))
                    SubmitObjective(objective);
            }
            else
                TooltipActionEvent?.Invoke("Wrong objective");
        }

        /// <summary>
        /// Add objective to the _submittedObjectives and check if quest can be finished
        /// </summary>
        /// <param name="objective"></param>
        private void SubmitObjective(ObjectiveSO objective)
        {
            _submittedObjectives.Add(objective);
            ObjectiveSubmittedEvent?.Invoke();
            TooltipActionEvent?.Invoke($"{objective.NameObjective} was submitted");

            
            if (_submittedObjectives.Count == _currentQuest.Objective.Count)
                FinishQuest();
        }
        
        /// <summary>
        /// Check if quest has multiple same objectives
        /// </summary>
        /// <param name="objective"></param>
        /// <returns></returns>
        private bool CheckDuplicate(ObjectiveSO objective)
        {
            int count1 = 0, count2 = 0;

            for (int i = 0; i < _currentQuest.Objective.Count; i++)
                if (_currentQuest.Objective[i].NameObjective.Equals(objective.NameObjective))
                    count1++;

            for (int i = 0; i < _submittedObjectives.Count; i++)
                if (_submittedObjectives[i].NameObjective.Equals(objective.NameObjective))
                    count2++;
            
            if (count1 == 0)
                return false;
            
            return count1 > count2;
        }

        /// <summary>
        /// Finish quest and clear all variables
        /// </summary>
        private void FinishQuest()
        {
            _currentMoney += _currentQuest.RewardQuest;
            moneyText.text = $"{_currentMoney}";
            
            TooltipActionEvent?.Invoke($"{_currentQuest.NameQuest} was finished");

            _isQuestActive = false;

            _submittedObjectives.Clear();
            QuestFinishedEvent?.Invoke();
            
            activateQuestButton.interactable = true;
            finishQuestButton.interactable = false;

            objective1.interactable = objective2.interactable = objective3.interactable = false;
        }
        
        #endregion


        #region SubmitObjective

        private void PressToSubmit1() => TryToSubmitObjective(allObjectives[0]);

        private void PressToSubmit2() => TryToSubmitObjective(allObjectives[1]);
        
        private void PressToSubmit3() => TryToSubmitObjective(allObjectives[2]);

        #endregion
    }
}
