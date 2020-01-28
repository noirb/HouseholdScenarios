using UnityEngine;
using System.Collections.Generic;

public abstract class PropertyActionProvider : MonoBehaviour {
    protected List<PropertyAction> actions;

    public List<PropertyAction> GetActions() { return actions; }

    public PropertyActionProvider()
    {
        actions = new List<PropertyAction>();
    }


    protected void AddAction(PropertyAction action)
    {
        if (!actions.Contains(action))
        {
            actions.Add(action);
        }
    }

    protected void RemoveAction(PropertyAction action)
    {
        if (!actions.Contains(action))
        {
            return;
        }

        actions.Remove(action);
    }

    public virtual string GetPropertyStatus() { return ""; }
}
