using UnityEngine;
using System.Collections;

public class Water : PropertyActionProvider {

    void Start () {
        AddAction(PropertyAction.MakeWet);
    }

}
