using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingManager : MonoBehaviour
{
    private static BindingManager instance = null;
    private Dictionary<string, Binding> bindingsMap = new Dictionary<string, Binding>();
    [SerializeField] private List<Binding> defaultBindings = new List<Binding>();
    private List<Binding> bindingsList = new List<Binding>();

    private void Awake()
    {
        // if there is no binding manager
        if(instance == null)
        {
            instance = this; // this is the binding manager
        }
        else if(instance != this) // else if there is another
        {
            Destroy(gameObject); // destroy it 
            return;
        }

        PopulateBindingDictionaries();
        LoadBindings();
    }

    /// <summary>
    /// gets all the bindings in the default bindings list
    /// </summary>
    private void PopulateBindingDictionaries()
    {
        foreach(Binding binding in defaultBindings)
        {
            if (bindingsMap.ContainsKey(binding.Name))
            {
                continue;
            }

            bindingsMap.Add(binding.Name, binding);
            bindingsList.Add(binding);
        }
    }

    private void LoadBindings()
    {
        foreach(Binding binding in bindingsList)
        {
            binding.Load();
        }
    }

    public static bool BindingHeld(string _key)
    {
        // get binding
        Binding binding = GetBinding(_key);

        if(binding != null)
        {
            // we have a binding, is it held?
            return binding.Held();
        }

        // no binding
        Debug.LogWarning("NO" + _key);
        return false;
    }

    public static Binding GetBinding(string _key)
    {
        if (instance.bindingsMap.ContainsKey(_key))
        {
            return instance.bindingsMap[_key];
        }
        return null;
    }

    public static void Rebind(string _name, KeyCode _value)
    {
        Binding binding = GetBinding(_name);

        if(binding != null)
        {
            binding.Rebind(_value);
        }
    }
}
