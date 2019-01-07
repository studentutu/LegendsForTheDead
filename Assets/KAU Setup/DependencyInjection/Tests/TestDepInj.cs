using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services.DependencyInjection;


public class TestDepInj : MonoBehaviour
{
    [SerializeField] private SettingsForType ToLoad;

    // Use this for initialization
    void Start()
    {
        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        Dependencies.Get<CheckForEventManagerCanvas>(ToLoad,false);
        timer.Stop();
        Debug.Log(" It Costed : " + timer.ElapsedMilliseconds + " ms (1/1000 s)");
    }

}
