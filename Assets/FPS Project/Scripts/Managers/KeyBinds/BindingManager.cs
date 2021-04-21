using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSProject.Keybinds
{
    public class BindingManager : MonoBehaviour
    {
        private static BindingManager instance = null;
        private Dictionary<string, Binding> bindingsMap = new Dictionary<string, Binding>(); // creates a dictionary for mapping the binding
        [SerializeField] private List<Binding> defaultBindings = new List<Binding>(); // creates a list of default bindings
        private List<Binding> bindingsList = new List<Binding>(); // creates a list of bindings

        private void Awake()
        {
            // check for another instance of the binding manager 
            // if not null then delete the duplicate and make 
            // sure this is the only instance

            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                return;
            }

            PopulateBindingDictionaries();
            LoadBindings();
        }

        /// <summary>
        /// scans through the default bindings
        /// and adds them to the binding map
        /// </summary>
        private void PopulateBindingDictionaries()
        {
            // loops through each binding
            // checks if the binding is already in the map
            // if so skip that one
            // adds the bindings to the map and binding list

            foreach(Binding binding in defaultBindings)
            {
                if (bindingsMap.ContainsKey(binding.Name))
                {
                    continue;
                }

                // adds binding to the map (dictionary)
                bindingsMap.Add(binding.Name, binding);
                bindingsList.Add(binding);  // adds binding to the list
            }
        }

        private void LoadBindings()
        {
            // scans through the list of bindings and loads them
            foreach(Binding binding in bindingsList)
            {
                binding.Load();
            }
        }

        public static bool BindingHeld(string _key)
        {
            // get the binding
            Binding binding = GetBinding(_key);
            if (binding != null)
            {
                // we have a binding return held
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
}

