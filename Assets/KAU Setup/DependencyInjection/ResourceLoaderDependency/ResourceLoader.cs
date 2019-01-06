using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services.DependencyInjection;

public class ResourceLoader : BaseMonoObject
{
    
    public UnityEngine.Object LoadByPath(string path)
    {
        return Resources.Load(path);
    }
    public GameObject LoadByPathGameObject(string path)
    {
        return Resources.Load(path, typeof(GameObject)) as GameObject;
    }

    public void InstantiateDependencyObject(string path)
    {
        Instantiate(LoadByPathGameObject(path));
    }



}
