using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class SampleQuestManager : MonoBehaviour
    {
        public static SampleQuestManager Instance;

        #region Variables

        /// <summary>
        /// All quests 
        /// </summary>
        [field: SerializeField] 
        private List<QuestSO> quests;

        /// <summary>
        /// Button that starts new quest
        /// </summary>
        [field: SerializeField] 
        private Button activateQuestButton;        
        
        [field: SerializeField] 
        private Button finishQuestButton;

        [field: SerializeField] 
        private Button objective1;
        [field: SerializeField] 
        private Button objective2;
        [field: SerializeField] 
        private Button objective3;

        [field: SerializeField] 
        private List<ObjectiveSO> allObjectives;

        private int _currentQuestIndex;
        
        private bool _isQuestActive;
        
        private QuestSO _currentQuest;
        
        private List<ObjectiveSO> _submittedObjectives = new List<ObjectiveSO>();
        
        public bool IsQuestActive => _isQuestActive;
        public List<ObjectiveSO> SubmittedObjectives => _submittedObjectives;

        #endregion
        

        #region Evemts

        public delegate void QuestActive(QuestSO questSo);
        public QuestActive QuestActiveEvent;        
        
        public delegate void ObjectiveSubmitted();
        public ObjectiveSubmitted ObjectiveSubmittedEvent;
        
        public delegate void QuestFinished();
        public QuestFinished QuestFinishedEvent;
        
        #endregion

        private void Awake()
        {
            Instance = this;

            activateQuestButton.onClick.AddListener(ActivateQuest);
            finishQuestButton.onClick.AddListener(FinishQuest);
            finishQuestButton.interactable = false;

            
            objective1.onClick.AddListener(PressToSubmit1);
            objective2.onClick.AddListener(PressToSubmit2);
            objective3.onClick.AddListener(PressToSubmit3);
            objective1.interactable = false;
            objective2.interactable = false;
            objective3.interactable = false;
        }

        /// <summary>
        /// Activate next quest in the list using button
        /// </summary>
        private void ActivateQuest()
        {
            if (_currentQuestIndex >= quests.Count)
            {
                Debug.Log("No more quests");
                return;
            }

            _currentQuest = quests[_currentQuestIndex];

            _isQuestActive = true;
            _currentQuestIndex++;
            
            QuestActiveEvent.Invoke(_currentQuest);

            activateQuestButton.interactable = false;
            finishQuestButton.interactable = true;
            
            objective1.interactable = true;
            objective2.interactable = true;
            objective3.interactable = true;
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
            {
                Debug.Log("Wrong objective");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objective"></param>
        private void SubmitObjective(ObjectiveSO objective)
        {
            _submittedObjectives.Add(objective);
            ObjectiveSubmittedEvent.Invoke();
            if (_submittedObjectives.Count != _currentQuest.Objective.Count)
                return;
                
            FinishQuest();
        }
        
        private bool CheckDuplicate(ObjectiveSO objective)
        {
            int count1 = 0;
            int count2 = 0;

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

        private void FinishQuest()
        {
            _isQuestActive = false;
            _currentQuest = null;

            _submittedObjectives.Clear();
            QuestFinishedEvent.Invoke();
            
            activateQuestButton.interactable = true;
            finishQuestButton.interactable = false;

            objective1.interactable = false;
            objective2.interactable = false;
            objective3.interactable = false;
        }

        private void PressToSubmit1()
        {
            TryToSubmitObjective(allObjectives[0]);
        }        
        private void PressToSubmit2()
        {
            TryToSubmitObjective(allObjectives[1]);
        }        
        private void PressToSubmit3()
        {
            TryToSubmitObjective(allObjectives[2]);
        }
    }
}
