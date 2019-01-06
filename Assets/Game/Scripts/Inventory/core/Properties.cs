using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Properties
{
    public string nameOfProperty = "Generic Property";
    public int PropertyId = 0;

    public virtual System.Object getPropertyValue()
    {
        return null;
    }

    public virtual void setPropertyValue(System.Object newValue)
    {
		
    }

    public override string ToString() {
        return string.Format(" Property : {0}, ID:{1}, value: {2}", nameOfProperty, PropertyId,getPropertyValue());
    }
}
