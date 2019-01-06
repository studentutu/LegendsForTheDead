
using UnityEngine;

namespace Services.DependencyInjection
{
    public abstract class BaseMonoObject : MonoBehaviour, IDepBase
    {
        [SerializeField] public SettingsForType MyTypeSetting;
        [HideInInspector] public IDependencyContext myCurrentContext;

        protected virtual void Awake()
        {
            if (MyTypeSetting == null)
            {
                Debug.Log(" Local Settings for this type Are null!");
                return;
            }
            else
            {
                ScriptCustomLogger.Log(" Local Settings Initialized ");
            }
            myCurrentContext = Dependencies.GetContext(MyTypeSetting.belongingContext);
            myCurrentContext.AddToGivenContext(this, MyTypeSetting);

        }
        public virtual void OnDestroy()
        {
            ClearLocalContext();
        }

        public virtual void ClearLocalContext()
        {
            if (myCurrentContext != null)
                myCurrentContext.RemoveFromContext(MyTypeSetting);
            ScriptCustomLogger.Log("[BaseMonoObject] " + " Cleared");
        }

    }

}

