using UnityEngine;
using System.Collections.Generic;

public abstract class ReactiveProperty : MonoBehaviour {

    protected abstract void ApplyActions(List<PropertyAction> actions);

    public abstract string GetPropertyState();
}
