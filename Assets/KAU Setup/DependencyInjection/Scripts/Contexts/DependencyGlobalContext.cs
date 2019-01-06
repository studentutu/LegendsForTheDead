﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Services.DependencyInjection
{
    public class DependencyGlobalContext : MonoBehaviour, IDependencyContext, IGlobalSettings
    {
        [SerializeField] private DependencyContextTypes context = DependencyContextTypes.Global;

        private Dictionary<AllPossibleDependencies, BaseMonoObject> currentObjectContext = new Dictionary<AllPossibleDependencies, BaseMonoObject>();

#if UNITY_EDITOR
        [Header(" Test Field")]
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
        [Space]
#endif
        [SerializeField] private DependenciesSettings Container;
        public DependenciesSettings GetGlobalSettings()
        {
            return Container;
        }

        // Use this for initialization
        private void Awake()
        {
            // Add me to GLobal Context
            RegisterContext(this);
            if (Container == null)
            {
                if(ContainerSettings.Instance != null)
                    Container = ContainerSettings.Instance.allSettings;
                else
                    ScriptCustomLogger.LogError(" No ContainerSettings on the Scene!. Ensure ContainerSettings is in Scene.");
                  
            }
            DontDestroyOnLoad(this);
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
