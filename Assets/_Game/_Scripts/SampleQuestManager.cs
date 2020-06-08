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
        /// Tooltip text keeper
        /// </summary>
        [field : SerializeField,
                 Tooltip("Tooltip text keeper")] 
        private TooltipTextSO tooltipTextSO;
        
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
        /// 0 based
        /// </summary>
        private int _currentQuestIndex;

        private bool _isQuestActive;
        
        private float _currentMoney;

        private QuestSO _currentQuest;

        private List<ObjectiveSO> _submittedObjectives = new List<ObjectiveSO>();
        
        /// <summary>
        /// Current quest
        /// </summary>
        public bool IsQuestActive => _isQuestActive;
        
        /// <summary>
        /// Objectives of current quest that player have already submitted
        /// </summary>
        public List<ObjectiveSO> SubmittedObjectives => _submittedObjectives;
        
        #endregion

        #endregion
        
        #region Events

        public delegate void QuestActive(QuestSO questSo);
        public delegate void ActionPerformed();
        public delegate void TooltipAction(string text);

        /// <summary>
        /// When quest activates
        /// </summary>
        public event QuestActive QuestActiveEvent;        
        
        /// <summary>
        /// Invokes when objective to the current quest is submitted
        /// </summary>
        public event ActionPerformed ObjectiveSubmittedEvent;
        
        /// <summary>
        /// Invokes when current quest is finished
        /// </summary>
        public event ActionPerformed QuestFinishedEvent;

        /// <summary>
        /// Invokes when info needs to be presented
        /// </summary>
        public event TooltipAction TooltipActionEvent;
        
        #endregion

        private void Awake()
        {
            activateQuestButton.onClick.AddListener(ActivateQuest);
            finishQuestButton.onClick.AddListener(FinishQuest);
            finishQuestButton.interactable = false;

            objective1.onClick.AddListener(() => TryToSubmitObjective(allObjectives[0]));
            objective2.onClick.AddListener(() => TryToSubmitObjective(allObjectives[1]));
            objective3.onClick.AddListener(() => TryToSubmitObjective(allObjectives[2]));
            
            objective1.interactable = objective2.interactable = objective3.interactable = false;

            moneyText.text = $"{_currentMoney}";
        }
        
        #region QuestManagement

        /// <summary>
        /// Activate next quest in the list using button
        /// </summary>
        private void ActivateQuest()
        {
            if (_currentQuestIndex >= quests.Count)
            {
                TooltipActionEvent?.Invoke(tooltipTextSO.NoMoreQuest);
                return;
            }

            _currentQuest = quests[_currentQuestIndex];
            _currentQuestIndex++;
            ChangeQuestActiveState(true);
        }
        
        /// <summary>
        /// Checking if objective can be submitted
        /// </summary>
        /// <param name="objective"></param>
        private void TryToSubmitObjective(ObjectiveSO objective)
        {
            if (_currentQuest.Objective.Contains(objective) && CanAcceptObjective(objective))
                SubmitObjective(objective);
            else
                TooltipActionEvent?.Invoke(tooltipTextSO.WrongObjective);
        }

        /// <summary>
        /// Add objective to the _submittedObjectives and check if quest can be finished
        /// </summary>
        /// <param name="objective"></param>
        private void SubmitObjective(ObjectiveSO objective)
        {
            _submittedObjectives.Add(objective);
            
            ObjectiveSubmittedEvent?.Invoke();
            string endValue = string.Format(tooltipTextSO.ObjectiveSubmitted, objective.ObjectiveName); 
            TooltipActionEvent?.Invoke(endValue);
            
            if (_submittedObjectives.Count == _currentQuest.Objective.Count)
                FinishQuest();
        }
        
        /// <summary>
        /// Check if quest has multiple same objectives
        /// </summary>
        /// <param name="objective"></param>
        /// <returns></returns>
        private bool CanAcceptObjective(ObjectiveSO objective)
        {
            int count1 = 0, count2 = 0;

            for (int i = 0; i < _currentQuest.Objective.Count; i++)
                if (_currentQuest.Objective[i].ObjectiveName.Equals(objective.ObjectiveName))
                    count1++;
            
            if (count1 == 0)
                return false;

            for (int i = 0; i < _submittedObjectives.Count; i++)
                if (_submittedObjectives[i].ObjectiveName.Equals(objective.ObjectiveName))
                    count2++;

            return count1 > count2;
        }

        /// <summary>
        /// Finish quest and clear all variables
        /// </summary>
        private void FinishQuest()
        {
            _currentMoney += _currentQuest.RewardQuest;
            moneyText.text = $"{_currentMoney}";

            _submittedObjectives.Clear();

            ChangeQuestActiveState(false);
        }

        /// <summary>
        /// Change state of current quest
        /// </summary>
        /// <param name="newState"></param>
        private void ChangeQuestActiveState(bool newState)
        {
            _isQuestActive = newState;

            activateQuestButton.interactable = !newState;
            finishQuestButton.interactable = newState;

            objective1.interactable = objective2.interactable = objective3.interactable = newState;
            
            if (newState)
            {
                string endValue = string.Format(tooltipTextSO.QuestActive, _currentQuest.QuestName); 

                QuestActiveEvent?.Invoke(_currentQuest);
                TooltipActionEvent?.Invoke(endValue);
            }
            else
            {
                string endValue = string.Format(tooltipTextSO.QuestFinished, _currentQuest.QuestName); 
                
                QuestFinishedEvent?.Invoke();
                TooltipActionEvent?.Invoke(endValue);
            }
        }
        
        #endregion
    }
}
