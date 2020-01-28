using UnityEngine;
using System.Collections;

/// <summary>
/// Used to tag objects with specific ontology classes
/// </summary>
public class OntologyClass : MonoBehaviour {
    [Tooltip("Ontology class name for this object (e.g. 'FoodVessel')")]
    public string ClassName;
    public string InstanceName;
}
