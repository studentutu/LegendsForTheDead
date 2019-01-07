
using System.Collections.Generic;
using UnityEngine;

namespace Services.DependencyInjection
{
    public abstract class IDependencyContext : MonoBehaviour
    {
        [SerializeField] public DependencyContextTypes context = DependencyContextTypes.None;
        // Instead Of Types use Custom Enum!
        private Dictionary<AllPossibleDependencies, IDepBase> currentObjectContext = new Dictionary<AllPossibleDependencies, IDepBase>();

#if UNITY_EDITOR
        [Header(" Test Field")]
        [SerializeField] protected List<IDepBase> allDependencies = new List<IDepBase>();
        protected virtual void Update()
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
        protected virtual void Awake()
        {
            if (this.context == DependencyContextTypes.Global)
            {
                DontDestroyOnLoad(this);
            }
            RegisterContext(this);
        }

        public void ReAttachToGL()
        {
            Awake();
        }
        public void DestroyMe(bool AllowRemoving)
        {
            if (AllowRemoving)
                OnDestroy();

            _destroyed = true;
            GameObject.Destroy(this.gameObject);
        }
        private bool _destroyed = false;
        protected virtual void OnDestroy()
        {
            if (!_destroyed)
                RemoveContext(this);
            _destroyed = true;
        }

        protected void RegisterContext(IDependencyContext itemToAdd)
        {
            Dependencies.AddContext(itemToAdd);
        }

        protected void RemoveContext(IDependencyContext itemToAdd)
        {
            Dependencies.RemoveContext(this);
        }

        public void AddToGivenContext(IDepBase baseDepObject, SettingsForType typeSettings)
        {
            IDepBase previous = null;
            if (currentObjectContext.ContainsKey(typeSettings.typeOfDependency))
                previous = currentObjectContext[typeSettings.typeOfDependency];

            if (previous != null && previous != baseDepObject)
                previous.RemoveFromLocalContextAndDestroy();

            currentObjectContext.Remove(typeSettings.typeOfDependency);
            currentObjectContext.Add(typeSettings.typeOfDependency, baseDepObject);

        }

        public IDepBase GetFromLocalContext(SettingsForType typeSettings)
        {
            IDepBase previous = null;
            if (currentObjectContext.ContainsKey(typeSettings.typeOfDependency))
            {
                previous = currentObjectContext[typeSettings.typeOfDependency];
            }
            return previous;
        }

        public void RemoveFromContext(SettingsForType typeSettings)
        {
            currentObjectContext.Remove(typeSettings.typeOfDependency);
        }

    }
}
