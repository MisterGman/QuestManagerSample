using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class QuestCurrent : MonoBehaviour
    {
        #region Variables

        [field : SerializeField,
                 Tooltip("Input")]
        private KeyAction keyAction;

        [field : SerializeField,
                 Tooltip("Rect transform of UI")]
        private RectTransform currentQuestRectTrans;

        [field : SerializeField,
                 Tooltip("Quest name field")]
        private TextMeshProUGUI questNameText;

        [field : SerializeField,
                 Tooltip("List of potions to submit")]
        private List<TextMeshProUGUI> objectiveListText;
        
        [field : SerializeField,
                 Tooltip("Objective's sprites")] 
        private List<GameObject> objectiveImages = new List<GameObject >();

        [field : SerializeField,
                 Tooltip("Money field")]
        private TextMeshProUGUI moneyText;        
        
        [field : SerializeField,
                 Tooltip("Money field")]
        private Button moveButton;


        private Vector3 _defaultPos;
        
        private QuestSO _currQuest;
        
        private SampleQuestManager _questMan;
        
        private List<List<Image>> _listImages = new List<List<Image>>();
        
        private float _normWindow = 1f;
        
        private bool _isDisplayed;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;
        
        #endregion
        
        
        private void Start()
        {
            currentQuestRectTrans.gameObject.SetActive(false);
        
            for (int i = 0; i < objectiveImages.Count; i++)
                _listImages.Add( objectiveImages[i].GetComponentsInChildren<Image>().ToList());

            _questMan = SampleQuestManager.Instance;

            _questMan.QuestActiveEvent += OnActiveQuest;
            _questMan.QuestFinishedEvent += OnNonActiveQuest;
            _questMan.ObjectiveSubmittedEvent += SubmittedPotion;

            keyAction = new KeyAction();
            keyAction.ToggleUI.Tab.performed += context => DisplayCurrentQuest();
            keyAction.Enable();
            
            moveButton.onClick.AddListener(DisplayCurrentQuest);
            
            PositionUpdateResolution();
        }


        #region OnEnable/DisableQuest
        
        /// <summary>
        /// Enables UI and fills all data to the text fields
        /// </summary>
        public void OnActiveQuest(QuestSO questSo)
        {
            for (int i = _listImages.Count- 1; i >= 0; i--)
            {
                for (int j =  _listImages[i].Count-1; j >= 0; j--)
                {
                    _listImages[i][j].gameObject.SetActive(false);
                    objectiveListText[i].gameObject.SetActive(false);
                }
            }
        
            currentQuestRectTrans.gameObject.SetActive(true);
            
            _currQuest = questSo;
            
            questNameText.text = _currQuest.NameQuest;
        
            for (int i = 0; i < _currQuest.Objective.Count; i++)
            {
                objectiveListText[i].text = _currQuest.Objective[i].NameObjective;
                objectiveListText[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < _currQuest.Objective.Count; i++)
                for (int j = 0; j < _currQuest.Objective[i].SpriteDescription.Count; j++)
                {
                    _listImages[i][0].gameObject.SetActive(true);

                    _listImages[i][j + 1].sprite = _currQuest.Objective[i].SpriteDescription[j];

                    _listImages[i][j+1].gameObject.SetActive(true);
                    
                }
            
            moneyText.text = (_currQuest.RewardQuest).ToString();

        }
        
        /// <summary>
        /// Disables UI
        /// </summary>
        public void OnNonActiveQuest()
        {
            currentQuestRectTrans.gameObject.SetActive(false);
            _isDisplayed = false;

            var position = currentQuestRectTrans.position;
            position = new Vector3(_defaultPos.x, position.y, position.z);
            currentQuestRectTrans.position = position;
        }
        
        /// <summary>
        /// Applies movement of UI for player to see
        /// Or to the default pos
        /// </summary>
        private void DisplayCurrentQuest()
        {
            if(!_questMan.IsQuestActive) return;
            _isDisplayed = !_isDisplayed;

            if (_isDisplayed)
            {
                currentQuestRectTrans.ForceUpdateRectTransforms();
                var rect = currentQuestRectTrans.rect;
                
                _tweener = currentQuestRectTrans.DOMoveX(_defaultPos.x - rect.width*_normWindow, 1);
            }
            else
            {
                _tweener = currentQuestRectTrans.DOMoveX(_defaultPos.x, 1);
            }

        }
        
        /// <summary>
        /// Cross objectives that was submitted
        /// </summary>
        private void SubmittedPotion()
        {
            if(_questMan.IsQuestActive)
                if (_currQuest.Objective.Count > 0)
                {
                    var potionListText = new List<string>();
        
                    for (int i = 0; i < _currQuest.Objective.Count; i++)
                        potionListText.Add(_currQuest.Objective[i].NameObjective);
        
                    for (int i = 0; i < _questMan.SubmittedObjectives.Count; i++)
                    {
                        for (int j = 0; j < _currQuest.Objective.Count; j++)
                        {
                            if (objectiveListText[j].text.Equals(potionListText[j]))
                            {
                                objectiveListText[j].text = potionListText[j].Replace
                                (
                                    _questMan.SubmittedObjectives[i].NameObjective,
                                    $"<b><s>{_questMan.SubmittedObjectives[i].NameObjective}</s></b>"
                                );
                                break;
                            }
                        }
                    }
                }
        }
        
        #endregion
        
        #region Helpers
        
        /// <summary>
        /// Set positions depending on resolution
        /// </summary>
        private void PositionUpdateResolution()
        {
            _normWindow = Screen.width / 1920f;
        
            var rect = currentQuestRectTrans.rect;
            
            _defaultPos.x = 1920f * _normWindow + rect.width/(2/_normWindow);
            
            if (!_isDisplayed)
            {
                _tweener.Kill();
                _tweener = currentQuestRectTrans.DOMoveX(_defaultPos.x, 1);
            }
            else
            {
                _tweener.Kill();
                _tweener = currentQuestRectTrans.DOMoveX(_defaultPos.x - rect.width*_normWindow, 1);
            }
        }
        
        #endregion
    }
}
