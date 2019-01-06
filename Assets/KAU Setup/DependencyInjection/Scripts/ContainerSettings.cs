
using UnityEngine;
namespace Services.DependencyInjection
{
    public class ContainerSettings : MonoBehaviour
    {
        public static ContainerSettings Instance;

        [SerializeField] public DependenciesSettings allSettings;
        private void Awake()
        {
            Instance = this;
        }
    }
}
