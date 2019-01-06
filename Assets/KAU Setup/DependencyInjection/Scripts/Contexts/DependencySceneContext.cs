// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.DependencyInjection
{
    sealed class DependencySceneContext : MonoBehaviour, IDependencyContext
    {
        [SerializeField] private DependencyContextTypes context = DependencyContextTypes.Scene;

#if UNITY_EDITOR
        [Header (" Test Field")]
        [SerializeField] private List<BaseMonoObject> allDependencies = new List<BaseMonoObject>();
        private void Update()
        {
            if (currentObjectContext.Count != allDependencies.Count)
            {
                allDependencies.Clear();
                foreach (var item in currentObjectContext.Keys)
                {
                    allDependencies.Add(currentObjectContext[item]);
                }
            }
        }
#endif

        // Instead Of Types use Custom Enum!
        private Dictionary<AllPossibleDependencies, BaseMonoObject> currentObjectContext = new Dictionary<AllPossibleDependencies, BaseMonoObject>();

        // Use this for initialization
        private void Awake()
        {
            // Add me to GLobal Context
            RegisterContext(this);
        }
        public void RegisterContext(IDependencyContext itemToAdd)
        {
            Dependencies.AddContext(itemToAdd);
        }

        private void OnDestroy()
        {
            RemoveContext(this);
        }


        public DependencyContextTypes GetMyContextType()
        {
            return context;
        }

        public void RemoveContext(IDependencyContext itemToAdd)
        {
            Dependencies.RemoveContext(itemToAdd.GetMyContextType());
        }

        public void AddToGivenContext(BaseMonoObject baseMonoObject, SettingsForType typeSettings) 
        {
            BaseMonoObject previous = null;
            if (currentObjectContext.ContainsKey(typeSettings.typeOfDependency))
                previous = currentObjectContext[typeSettings.typeOfDependency];

            if (previous != null && previous != baseMonoObject)
                previous.ClearLocalContext();

            currentObjectContext.Add(typeSettings.typeOfDependency, baseMonoObject);

        }

        public BaseMonoObject GetFromLocalContext(SettingsForType typeSettings) 
        {
            BaseMonoObject previous = null;
            if (currentObjectContext.ContainsKey(typeSettings.typeOfDependency))
            {
                previous = currentObjectContext[typeSettings.typeOfDependency];
            }

            return previous;
        }

        
        public void RemoveFromContext(SettingsForType typeSettings) 
        {
            if (currentObjectContext.ContainsKey(typeSettings.typeOfDependency))
            {
                currentObjectContext.Remove(typeSettings.typeOfDependency);
            }
        }

    }
}
