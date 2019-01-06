using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Services.DependencyInjection
{
    [CreateAssetMenu(fileName = "DependenciesSettings", menuName = "Dependencies/Settings", order = 3)]
    public class DependenciesSettings : ScriptableObject
    {

        #region MenuTypes
        /*
        *    Add your cases here: ALL_MENUS, MenuManagerSO
        *
        **/
        [SerializeField] private SettingsForType _EventManagerDepType;
        [SerializeField] private SettingsForType _MenuDepType;

        [SerializeField] private SettingsForType _ResourcesLoaderType;

        public SettingsForType EventManager
        {
            get
            {
                return _EventManagerDepType;
            }
        }
        public SettingsForType MenuDepType
        {
            get
            {
                return _MenuDepType;
            }
        }
        
        public SettingsForType ResourcesLoaderType
        {
            get
            {
                return _ResourcesLoaderType;
            }
        }

        #endregion
    }
}
