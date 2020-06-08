using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class QuestDisplay : MonoBehaviour
    {
        #region Variables

        #region InspectorVisible

        /// <summary>
        /// Input system's generated class
        /// </summary>
        [field : SerializeField,
                 Tooltip("Input")]
        private KeyAction keyAction;

        /// <summary>
        /// RectTransform of UI
        /// </summary>
        [field : SerializeField,
                 Tooltip("Rect transform of UI")]
        private RectTransform currentQuestRectTrans;

        /// <summary>
        /// Quest name field
        /// </summary>
        [field : SerializeField,
                 Tooltip("Quest name field")]
        private TextMeshProUGUI questNameText;

        /// <summary>
        /// List of objectives to submit
        /// </summary>
        [field : SerializeField,
                 Tooltip("List of objectives to submit")]
        private List<TextMeshProUGUI> objectiveListText;
        
        /// <summary>
        /// Objective's sprites
        /// </summary>
        [field : SerializeField,
                 Tooltip("Objective's sprites")] 
        private List<GameObject> objectiveImages = new List<GameObject >();

        /// <summary>
        /// Money text field
        /// </summary>
        [field : SerializeField,
                 Tooltip("Money field")]
        private TextMeshProUGUI moneyText;        
        
        /// <summary>
        /// Toggle button
        /// </summary>
        [field : SerializeField,
                 Tooltip("Toggle button")]
        private Button toggleButton;

        /// <summary>
        /// Quest manager's cache
        /// </summary>
        [field : SerializeField,
                 Tooltip("Quest manager's cache")]
        private SampleQuestManager questMan;

        /// <summary>
        /// Duration for UI movement
        /// </summary>
        [field : SerializeField,
                 Tooltip("Duration for UI movement")]
        private float durationMovement = 1f;
        
        #endregion
        
        #region Private

        /// <summary>
        /// Default position of UI
        /// It changes depending on resolution
        /// </summary>
        private Vector3 _defaultPos;
        
        /// <summary>
        /// All images from the objectiveImages 
        /// </summary>
        private List<List<Image>> _listOfObjectiveImages = new List<List<Image>>();

        private const float DefaultScreenWidth = 1920f;
        
        private float _normWindow;

        private bool _isDisplayVisible;
        
        private QuestSO _currQuest;

        /// <summary>
        /// Tweener is for moving RectTransform of QuestDisplay
        /// </summary>
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;
        
        #endregion
        
        #endregion

        private void Start()
        {
            currentQuestRectTrans.gameObject.SetActive(false);
        
            for (int i = 0; i < objectiveImages.Count; i++)
                _listOfObjectiveImages.Add( objectiveImages[i].GetComponentsInChildren<Image>().ToList());
            
            questMan.QuestActiveEvent        += ActiveQuestHandler;
            questMan.QuestFinishedEvent      += NonActiveQuestHandler;
            questMan.ObjectiveSubmittedEvent += SubmittedObjectiveHandler;
            toggleButton.onClick.AddListener(ToggleCurrentQuestDisplayHandler);

            keyAction = new KeyAction();
            keyAction.ToggleUI.Tab.performed += context => ToggleCurrentQuestDisplayHandler();
            keyAction.Enable();
            
            PositionUpdateResolution();
        }
        
        #region Handlers
        
        /// <summary>
        /// Enables UI and fills all data to the text fields
        /// </summary>
        private void ActiveQuestHandler(QuestSO questSo)
        {
            for (int i = _listOfObjectiveImages.Count - 1; i >= 0; i--)
            for (int j = _listOfObjectiveImages[i].Count - 1; j >= 0; j--)
            {
                _listOfObjectiveImages[i][j].gameObject.SetActive(false);
                objectiveListText[i].gameObject.SetActive(false);
            }

            currentQuestRectTrans.gameObject.SetActive(true);
            
            _currQuest = questSo;
            
            questNameText.text = _currQuest.QuestName;
        
            // Set objective name visible on the UI
            for (int i = 0; i < _currQuest.Objective.Count; i++)
            {
                objectiveListText[i].text = _currQuest.Objective[i].ObjectiveName;
                objectiveListText[i].gameObject.SetActive(true);
            }

            // Set objective description icons visible on the UI
            for (int i = 0; i < _currQuest.Objective.Count; i++)
            for (int j = 0; j < _currQuest.Objective[i].SpriteDescription.Count; j++)
            {
                _listOfObjectiveImages[i][0].gameObject.SetActive(true);
                _listOfObjectiveImages[i][j + 1].sprite = _currQuest.Objective[i].SpriteDescription[j];
                _listOfObjectiveImages[i][j+1].gameObject.SetActive(true);
            }
            
            moneyText.text = $"{questSo.RewardQuest}";
        }
        
        /// <summary>
        /// Disables UI
        /// </summary>
        private void NonActiveQuestHandler()
        {
            currentQuestRectTrans.gameObject.SetActive(false);
            _isDisplayVisible = false;

            var position = currentQuestRectTrans.position;
            position = new Vector3(_defaultPos.x, position.y, position.z);
            currentQuestRectTrans.position = position;
        }
        
        /// <summary>
        /// Applies movement of UI for player to see
        /// Or to the default pos
        /// </summary>
        private void ToggleCurrentQuestDisplayHandler()
        {
            if(!questMan.IsQuestActive) return;
            
            _isDisplayVisible = !_isDisplayVisible;

            if (_isDisplayVisible)
            {
                var rect = currentQuestRectTrans.rect;
                var endVal = _defaultPos.x - rect.width * _normWindow;

                _tweener = currentQuestRectTrans.DOMoveX(endVal, 1);
            }
            else
            {
                _tweener = currentQuestRectTrans.DOMoveX(_defaultPos.x, durationMovement);
            }
        }
        
        /// <summary>
        /// Cross objectives that was submitted
        /// </summary>
        private void SubmittedObjectiveHandler()
        {
            if (!questMan.IsQuestActive)
                return;

            if (_currQuest.Objective.Count <= 0)
                return;
            
            var objList = new List<string>();
        
            for (int i = 0; i < _currQuest.Objective.Count; i++)
                objList.Add(_currQuest.Objective[i].ObjectiveName);
            
            for (int i = 0; i < questMan.SubmittedObjectives.Count; i++)
            for (int j = 0; j < _currQuest.Objective.Count; j++)
                if (questMan.SubmittedObjectives[i].ObjectiveName.Equals(objList[j]))
                {
                    objectiveListText[j].text = objList[j].Replace
                    (
                        questMan.SubmittedObjectives[i].ObjectiveName,
                        $"<b><s>{questMan.SubmittedObjectives[i].ObjectiveName}</s></b>"
                    );

                    break;
                }
        }

        #endregion
        
        #region Helpers
        
        /// <summary>
        /// Set positions depending on resolution
        /// </summary>
        private void PositionUpdateResolution()
        {
            _normWindow = Screen.width / DefaultScreenWidth;
        
            var rect = currentQuestRectTrans.rect;
            
            _defaultPos.x = DefaultScreenWidth * _normWindow + rect.width / (2 / _normWindow);
            
            _tweener.Kill();
            _tweener = !_isDisplayVisible 
                ? currentQuestRectTrans.DOMoveX(_defaultPos.x, durationMovement)
                : currentQuestRectTrans.DOMoveX(_defaultPos.x - rect.width * _normWindow, durationMovement);
        }
        
        #endregion
    }
}
