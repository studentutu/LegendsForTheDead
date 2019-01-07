using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services.DependencyInjection;

public class ResourceLoader : IDepClassBase
{
    public ResourceLoader(SettingsForType settings) : base(settings)
    {
    }
    public GameObject LoadByPathGameObject(string path)
    {
        return  Resources.Load<GameObject>(path); 
    }

    public GameObject InstantiateMonoDependency(string path)
    {
        return GameObject.Instantiate(LoadByPathGameObject(path));
    }
    public IDepBase GetNewDependency(SettingsForType typeSettings)
    {
        var newObj =  InstantiateMonoDependency(typeSettings.PathToPrefab);
        return newObj.GetComponent<IDepBase>();
    }

    public IDepBase GetNewDependency<T>(SettingsForType typeSettings) where T : IDepBase
    {
        
        if (typeSettings.IsMono){
            
            return GetNewDependency(typeSettings);
        }
        else
        {
            return System.Activator.CreateInstance<T>();
        }
    }

}
