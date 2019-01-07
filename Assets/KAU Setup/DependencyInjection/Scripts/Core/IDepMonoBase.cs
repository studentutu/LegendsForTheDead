
using UnityEngine;

namespace Services.DependencyInjection
{
    /// <summary>
    /// Always call the base Awake And Destroy when inheriting!
    /// </summary>
    public abstract class IDepMonoBase : MonoBehaviour, IDepBase
    {
        [SerializeField] private SettingsForType _MyTypeSetting;

        protected virtual void Awake()
        {
            if (_MyTypeSetting == null)
            {
                Debug.Log(" Local Settings for this type Are null!");
                return;
            }
            else
            {
                ScriptCustomLogger.Log(" Local Settings Initialized ");
            }
            var _myCurrentContext = Dependencies.GetContext(_MyTypeSetting.belongingContext,destroyed);
            _myCurrentContext.AddToGivenContext(this, _MyTypeSetting);

        }
        private bool destroyed = false;
        public virtual void OnDestroy()
        {
            if (!destroyed)
            {
                destroyed = true;
                RemoveFromLocalContext();
            }
        }


        public void RemoveFromLocalContextAndDestroy()
        {
            if (!destroyed && _MyTypeSetting != null)
            {
                destroyed = true;
                var _myCurrentContext = Dependencies.GetContext(_MyTypeSetting.belongingContext,destroyed);
                if(_myCurrentContext != null)
                    _myCurrentContext.RemoveFromContext(_MyTypeSetting);
            }
            Destroy(this.gameObject);
        }

        public SettingsForType getMyTypeSettings()
        {
            return _MyTypeSetting;
        }

        public void setMyTypeSettings(SettingsForType mynewSettings)
        {

            RemoveFromLocalContext();

            _MyTypeSetting = mynewSettings;
            AddToLocalContext();
        }

        public void RemoveFromLocalContext()
        {
            if (_MyTypeSetting != null)
            {
                var _myCurrentContext = Dependencies.GetContext(_MyTypeSetting.belongingContext,destroyed);
                if(_myCurrentContext != null)
                    _myCurrentContext.RemoveFromContext(_MyTypeSetting);
            }
        }
        public void AddToLocalContext()
        {
            var _myContext = Dependencies.GetContext(_MyTypeSetting.belongingContext,destroyed);
            _myContext.AddToGivenContext(this, _MyTypeSetting);
        }
    }

}

