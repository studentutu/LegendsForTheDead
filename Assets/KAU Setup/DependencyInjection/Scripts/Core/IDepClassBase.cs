using System.Collections;
using System.Collections.Generic;

namespace Services.DependencyInjection
{

    public class IDepClassBase : IDepBase, System.IDisposable
    {
        private SettingsForType _myTypeSettings;
        public IDepClassBase()
        {
            if (_myTypeSettings == null)
            {
                Debug.Log(" Local Settings for this type Are null!");
                return;
            }
            else
            {
                ScriptCustomLogger.Log(" Local Settings Initialized ");
            }
            var _myContext = Dependencies.GetContext(_myTypeSettings.belongingContext, destroyed);
            _myContext.AddToGivenContext(this, _myTypeSettings);
        }
        public IDepClassBase(SettingsForType newSetingsToSet)
        {
            if (newSetingsToSet == null)
            {
                Debug.LogError(" You should always provide the SettingsForType");
                return;
            }
            _myTypeSettings = newSetingsToSet;
            var _myContext = Dependencies.GetContext(_myTypeSettings.belongingContext, destroyed);
            _myContext.AddToGivenContext(this, _myTypeSettings);
        }
        public SettingsForType getMyTypeSettings()
        {
            return _myTypeSettings;
        }

        public void setMyTypeSettings(SettingsForType mynewSetings)
        {
            // When Changin - first remove from context!
            RemoveFromLocalContext();
            _myTypeSettings = mynewSetings;
            AddToLocalContext();
        }

        public void RemoveFromLocalContext()
        {
            if (_myTypeSettings != null)
            {
                var context = Dependencies.GetContext(_myTypeSettings.belongingContext, destroyed);
                if(context != null)
                    context.RemoveFromContext(_myTypeSettings);
            }
        }

        private bool destroyed = false;
        public void RemoveFromLocalContextAndDestroy()
        {
            if (!destroyed && _myTypeSettings != null)
            {
                destroyed = true;
                var context = Dependencies.GetContext(_myTypeSettings.belongingContext, destroyed);
                if(context != null)
                    context.RemoveFromContext(_myTypeSettings);
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!destroyed)
            {
                destroyed = true;
                RemoveFromLocalContext();
            }
        }

        public void AddToLocalContext()
        {
            var _myContext = Dependencies.GetContext(_myTypeSettings.belongingContext, destroyed);
            _myContext.AddToGivenContext(this, _myTypeSettings);
        }
    }
}
