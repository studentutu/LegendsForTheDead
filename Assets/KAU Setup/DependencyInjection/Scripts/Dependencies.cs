using System.Collections;
using System.Collections.Generic;


namespace Services.DependencyInjection
{
    public static class Dependencies
    {
        private static List<IDependencyContext> allContexts = new List<IDependencyContext>();
        public static IDependencyContext GetContext(DependencyContextTypes contextType, bool OnDestroy)
        {
            IDependencyContext result = null;
            for (int i = 0; i < allContexts.Count; i++)
            {
                if (allContexts[i].context == contextType)
                {
                    result = allContexts[i];
                    break;
                }
            }
            if (!OnDestroy && result == null)
            {
                result = CreateOrFindContext(contextType);
            }
            return result;
        }

        private static IDependencyContext CreateOrFindContext(DependencyContextTypes contextType)
        {
            IDependencyContext result = null;
            var allContexts = UnityEngine.GameObject.FindObjectsOfType<AnyContext>();
            for (int i = 0; i < allContexts.Length; i++)
            {
                if (allContexts[i].context == contextType)
                    result = allContexts[i];
            }
            if (result == null)
            {
                // Create One
                var newGameObject = new UnityEngine.GameObject("Context" + contextType.ToString());
                Debug.LogWarning(" Created New Context : " + "Context" + contextType.ToString());
                result = newGameObject.AddComponent<AnyContext>();
                Dependencies.RemoveContext(result);
                result.context = contextType;
                result.ReAttachToGL();
            }

            return result;
        }
        public static void AddContext(IDependencyContext contextToCheck)
        {
            var typeToCheck = contextToCheck.context;
            int AppendTo = -1;

            for (int i = 0; i < allContexts.Count; i++)
            {
                if (allContexts[i].context == typeToCheck)
                {
                    AppendTo = i;
                    break;
                }
            }

            if (AppendTo == -1)
                allContexts.Add(contextToCheck);
            else
            {
                if (allContexts[AppendTo] != contextToCheck)
                    allContexts[AppendTo].DestroyMe(false);
                allContexts[AppendTo] = contextToCheck;
            }

        }

        public static void RemoveContext(IDependencyContext contextToRemove)
        {
            allContexts.Remove(contextToRemove);
        }

        public static IDepBase Get<T>(SettingsForType typeSettings, bool OnDestroy) where T : IDepBase
        {
            IDepBase result = null;

            for (int i = 0; i < allContexts.Count; i++)
            {
                // It will Check only the Dictionaries!
                result = allContexts[i].GetFromLocalContext(typeSettings);

                if (result != null)
                {
                    return result;
                }
            }
            if (OnDestroy)
            {
                return result;
            }
            // Not Found, need to create/ (find when mono)
            if (typeSettings.typeOfDependency == AllPossibleDependencies.ResourcesLoader)
            {
                result = new ResourceLoader(typeSettings);
            }
            else
            {
                // It is not in the scene, need to Create one from provided Global settings!
                DependenciesSettings settings = null;
                if (ContainerSettings.Instance != null)
                    settings = ContainerSettings.Instance.allSettings;
                else
                {
                    ScriptCustomLogger.LogError(" No ContainerSettings on the Scene!. Ensure ContainerSettings is in Scene.");
                    return null;
                }

                ResourceLoader refToResourceLoader = (ResourceLoader)Get<ResourceLoader>(settings.ResourcesLoaderType, OnDestroy);
                result = refToResourceLoader.GetNewDependency<T>(typeSettings);
                // right after Instantiation, Dependency object is attached to context
            }

            result.setMyTypeSettings(typeSettings);
            result = Get<T>(typeSettings, OnDestroy);
            return result;
        }

    }
}