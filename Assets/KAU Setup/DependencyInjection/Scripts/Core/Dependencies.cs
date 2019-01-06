using System.Collections;
using System.Collections.Generic;


namespace Services.DependencyInjection
{
    public static class Dependencies
    {
        // Instead Of Types use Custom Enum!
        private static Dictionary<DependencyContextTypes, IDependencyContext> allContexts = new Dictionary<DependencyContextTypes, IDependencyContext>();


        public static IDependencyContext GetContext(DependencyContextTypes contextType)
        {
            IDependencyContext result = null;
            if (allContexts.ContainsKey(contextType))
            {
                result = allContexts[contextType];
            }
            if (result == null)
            {
                result = (IDependencyContext)CreateOrFindContext(contextType);
                ScriptCustomLogger.LogWarning("[Dependencies] " + contextType.ToString()  + " Registered New ");
            }
            return result;
        }

        private static UnityEngine.MonoBehaviour CreateOrFindContext(DependencyContextTypes contextType)
        {
            UnityEngine.MonoBehaviour result = null;
            switch (contextType)
            {
                case DependencyContextTypes.Scene:
                    result = UnityEngine.GameObject.FindObjectOfType<DependencySceneContext>();
                    break;
                case DependencyContextTypes.Global:
                    result = UnityEngine.GameObject.FindObjectOfType<DependencyGlobalContext>();
                    break;
            }
            if (result == null)
            {
                // Create One
                var newGameObject = new UnityEngine.GameObject("Context" + contextType.ToString());

                switch (contextType)
                {
                    case DependencyContextTypes.Scene:
                        result = newGameObject.AddComponent<DependencySceneContext>();
                        break;
                    case DependencyContextTypes.Global:
                        result = newGameObject.AddComponent<DependencyGlobalContext>();
                        break;
                }
            }


            return result;
        }
        public static void AddContext(IDependencyContext contextToCheck)
        {
            var typeToCheck = contextToCheck.GetMyContextType();
            allContexts.Remove(typeToCheck);
            allContexts.Add(typeToCheck, contextToCheck);

        }

        public static void RemoveContext(DependencyContextTypes contextToCheck)
        {
            IDependencyContext objectToClear = null;
            if (allContexts.ContainsKey(contextToCheck))
            {
                objectToClear = allContexts[contextToCheck];
                allContexts.Remove(contextToCheck);
            }
            else
            {
                ScriptCustomLogger.LogWarning("[DependencyContext] " + contextToCheck.ToString() + " No Such Context was Found");
            }


        }

        public static BaseMonoObject Get(SettingsForType typeSettings)
        {
            BaseMonoObject result = null;
            var GlobalContext = GetContext(DependencyContextTypes.Global);

            DependencyContextTypes typeDepContext;
            for (int i = 0; i < 2; i++)
            {
                typeDepContext = (DependencyContextTypes)i;

                // Check every Context if it has the Instance
                if (allContexts.ContainsKey(typeDepContext))
                {
                    var context = allContexts[typeDepContext];
                    // It will Check only the Dictionaries!
                    result = context.GetFromLocalContext(typeSettings);
                }
                if (result != null)
                {
                    break;
                }
            }


            if (result == null)
            {
                if (typeSettings.typeOfDependency == AllPossibleDependencies.ResourcesLoader)
                {
                    result = CreateOrFindResourceLoader();
                    if (result.MyTypeSetting == null)
                    {
                        result.MyTypeSetting = typeSettings;
                        var rl = (ResourceLoader)result;
                        rl.myCurrentContext = Dependencies.GetContext(typeSettings.belongingContext);
                        rl.myCurrentContext.AddToGivenContext(result, typeSettings);
                    }
                }
                else
                {
                    // It is not in the scene, need to Create one from provided Global settings!
                    var settings = ((IGlobalSettings)GlobalContext).GetGlobalSettings();
                    var gameObjectResourceLoader = (ResourceLoader)Get(settings.ResourcesLoaderType);
                    gameObjectResourceLoader.InstantiateDependencyObject(typeSettings.PathToPrefab);
                    // right after Instantiation, Dependency object is attached to context
                    result = Get(typeSettings);

                }
            }

            return result;
        }

        private static BaseMonoObject CreateOrFindResourceLoader()
        {
            BaseMonoObject result = null;

            result = UnityEngine.GameObject.FindObjectOfType<ResourceLoader>();

            if (result == null)
            {
                // Create One , Awake will add to Context
                var newGameObject = new UnityEngine.GameObject("ResourceLoader");
                result = newGameObject.AddComponent<ResourceLoader>();

            }
            return result;
        }



    }
}