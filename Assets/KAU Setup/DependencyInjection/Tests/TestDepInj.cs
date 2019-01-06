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
        Dependencies.Get(ToLoad);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
