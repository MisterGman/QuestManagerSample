using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Object/Objective", fileName = "ObjectiveSO")]
    public class ObjectiveSO : ScriptableObject
    {
        [SerializeField] 
        private string nameObjective;

        public string NameObjective => nameObjective;

        [SerializeField] 
        private List<Sprite> spriteDescription = new List<Sprite>();

        public List<Sprite> SpriteDescription => spriteDescription;
    }
}
