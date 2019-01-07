using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Services.DependencyInjection
{
    [CreateAssetMenu(fileName = "TypeSettings", menuName = "Dependencies/TypeSettings", order = 3)]
    public class SettingsForType : ScriptableObject
    {
        public AllPossibleDependencies typeOfDependency;
        public DependencyContextTypes belongingContext;

        [Tooltip("Assign the  Menu Prefab here")]
        public string PathToPrefab;

        [SerializeField] public bool IsMono = true;

    }
}
