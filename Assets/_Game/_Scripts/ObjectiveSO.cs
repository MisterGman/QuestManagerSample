using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Object/Objective", fileName = "ObjectiveSO")]
    public class ObjectiveSO : ScriptableObject
    {
        [field : SerializeField,
                 Tooltip("Name of objective")]
        private string objectiveName;

        /// <summary>
        /// Name of objective
        /// </summary>
        public string ObjectiveName => objectiveName;
        
        [field : SerializeField,
                 Tooltip("Description (sprites) of objective")]
        private List<Sprite> spriteDescription = new List<Sprite>();

        /// <summary>
        /// Description (sprites) of objective
        /// </summary>
        public List<Sprite> SpriteDescription => spriteDescription;
    }
}
