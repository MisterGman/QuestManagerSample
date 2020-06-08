using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Object/Objective", fileName = "ObjectiveSO")]
    public class ObjectiveSO : ScriptableObject
    {

        [field : SerializeField,
                 Tooltip("Name of objective")]
        private string nameObjective;

        /// <summary>
        /// Name of objective
        /// </summary>
        public string NameObjective => nameObjective;
        
        [field : SerializeField,
                 Tooltip("Description (sprites) of objective")]
        private List<Sprite> spriteDescription = new List<Sprite>();

        /// <summary>
        /// Description (sprites) of objective
        /// </summary>
        public List<Sprite> SpriteDescription => spriteDescription;
    }
}
