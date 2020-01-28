using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Wettable/Washable dish class
/// </summary>
public class Dish : ReactiveProperty {
    [Tooltip("Whether the dish is currently wet (or dry)")]
    public bool wet = false;
    [Tooltip("Whether the dish is currently clean (or dirty)")]
    public bool clean = false;
    [Space]
    [Tooltip("Particle effect to display when dish is wet")]
    public ParticleSystem wet_fx;
    [Space]
    [Tooltip("Color to apply to dish's texture when it's dirty")]
    public Color dirtyColor = Color.gray;
    [Tooltip("Color to apply to dish's texture when it's clean")]
    public Color cleanColor = Color.white;
    [Tooltip("How quickly the dish is cleaned (lower values take more strokes to be come fully clean)")]
    public float cleanability = 0.1f;

    private MeshRenderer _renderer;
    private float _cleanliness = 0.0f;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();

        if (clean)
        {
            _renderer.material.SetColor("_Color", cleanColor);
        }
        else
        {
            _renderer.material.SetColor("_Color", dirtyColor);
        }

        if (wet_fx != null)
        {
            var em = wet_fx.emission;
            if (wet)
            {
                em.enabled = true;
            }
            else
            {
                em.enabled = false;
            }
        }
    }

    protected override void ApplyActions(List<PropertyAction> actions)
    {
        foreach (var action in actions)
        {
            switch (action)
            {
                case PropertyAction.MakeWet:
                    Wet();
                    break;
                case PropertyAction.MakeClean:
                    Clean();
                    break;
                case PropertyAction.MakeDry:
                    Dry();
                    break;
                case PropertyAction.MakeDirty:
                    Dirty();
                    break;
                default:
                    break;
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        var actionProvider = other.GetComponent<PropertyActionProvider>();
        if (actionProvider != null)
        {
            var actions = actionProvider.GetActions();
            foreach (var action in actions)
            {
                switch (action)
                {
                    case PropertyAction.MakeWet:
                        Wet();
                        break;
                    case PropertyAction.MakeClean:
                        Clean();
                        break;
                    case PropertyAction.MakeDry:
                        Dry();
                        break;
                    case PropertyAction.MakeDirty:
                        Dirty();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void Wet()
    {
        if (!this.wet)
        {
            this.wet = true;
            if (wet_fx != null)
            {
                var em = wet_fx.emission;
                em.enabled = true;
            }
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "PropertyChanged", "Wetness:Wet");
        }
    }

    void Dry()
    {
        if (this.wet)
        {
            this.wet = false;
            if (wet_fx != null)
            {
                var em = wet_fx.emission;
                em.enabled = false;
            }
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "PropertyChanged", "Wetness:Dry");
        }
    }

    void Clean()
    {
        if (!this.clean)
        {
            this.clean = true;
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "PropertyChanged", "Dirtiness:Clean");
        }

        if (_cleanliness < 1.0f)
        {
            _cleanliness += cleanability;
            _renderer.material.SetColor("_Color", Color.Lerp(dirtyColor, cleanColor, _cleanliness));
        }
    }

    void Dirty()
    {
        if (this.clean)
        {
            this.clean = false;
            _renderer.material.SetColor("_Color", dirtyColor);
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "PropertyChanged", "Dirtiness:Dirty");
        }
    }

    void OnCollisionEnter(Collision col)
    {
        var actionProvider = col.gameObject.GetComponent<PropertyActionProvider>();
        if (actionProvider != null)
        {
            ApplyActions(actionProvider.GetActions());
        }
    }

    public override string GetPropertyState()
    {
        string wet_str = wet ? "Wetness:Wet" : "Wetness:Dry";
        string clean_str = clean ? "Dirtiness:Clean" : "Dirtiness:Dirty";
        return wet_str + "|" + clean_str;
    }
}
