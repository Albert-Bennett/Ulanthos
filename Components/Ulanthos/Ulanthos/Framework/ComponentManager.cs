using System;
using System.Collections.Generic;
using Ulanthos.Interfaces;

namespace Ulanthos.Framework
{
    /// <summary>
    /// Defines an object that manages UlanComponents.
    /// </summary>
    public class ComponentManager : IDispose
    {
        static List<UlanComponent> components = new List<UlanComponent>();

        /// <summary>
        /// Creates a new ComponentManager.
        /// </summary>
        public ComponentManager() { }

        /// <summary>
        /// Initializes all of the UlanComponents.
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < components.Count; i++)
                if (components[i].Enabled)
                    components[i].Initialize();
        }

        /// <summary>
        /// Loads content from all of the UlanComponents.
        /// </summary>
        public void Load()
        {
            foreach (UlanComponent comp in components)
                if (comp.Enabled)
                    comp.Load();
        }

        /// <summary>
        /// Updates all of the UlanComponents.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Initialized)
                    components[i].Initialize();

                if (components[i].Enabled)
                    components[i].Update();
            }
        }

        /// <summary>
        /// Unloads content from all of the ExedusComponents.
        /// </summary>
        public void Unload()
        {
            for (int i = 0; i < components.Count; i++)
                if (components[i].Enabled)
                    components[i].Unload();
        }

        /// <summary>
        /// Draws all of the DrawableUlanComponents.
        /// </summary>
        public void Render()
        {
            for (int i = 0; i < components.Count; i++)
                if (components[i] is DrawableUlanComponent)
                    ((DrawableUlanComponent)components[i]).Render();
        }

        /// <summary>
        /// Disposes of all of the UlanComponent.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < components.Count; i++)
                if (components[i] is DrawableUlanComponent)
                    ((DrawableUlanComponent)components[i]).Dispose();
                else if (components[i] is IDispose)
                    ((IDispose)components[i]).Dispose();

            components.Clear();
            components = null;
        }

        /// <summary>
        /// Adds an UlanComponent to this.
        /// </summary>
        /// <param name="component">The UlanComponent to be added.</param>
        public static void Add(UlanComponent component)
        {
            if (components.Count == 0)
                components.Add(component);
            else
                for (int i = 0; i < components.Count; i++)
                    if (components[i].Name != component.Name)
                        components.Add(component);
        }

        /// <summary>
        /// Disables the updating of an UlanComponent.
        /// </summary>
        /// <param name="name">The name of the UlanComponent.</param>
        public void UnableComponent(string name)
        {
            UlanComponent comp = ValidateComponent(name);

            if (comp.Enabled)
                comp.ToggleEnabled();
        }

        /// <summary>
        /// Enables the updating of an UlanComponent.
        /// </summary>
        /// <param name="name">The name of the UlanComponent.</param>
        public void EnableComponent(string name)
        {
            UlanComponent comp = ValidateComponent(name);

            if (!comp.Enabled)
                comp.ToggleEnabled();
        }

        /// <summary>
        /// Disables the drawing of a DrawableUlanComponent.
        /// </summary>
        /// <param name="name">The name of the DrawableUlanComponent.</param>
        public void HideComponent(string name)
        {
            UlanComponent comp = ValidateComponent(name);

            if (comp.Visable)
                comp.ToggleVisable();
        }

        /// <summary>
        /// Enables the drawing of a DrawableUlanComponent.
        /// </summary>
        /// <param name="name">The name of the DrawableUlanComponent.</param>
        public void RevealComponent(string name)
        {
            UlanComponent comp = ValidateComponent(name);

            if (!comp.Visable)
                comp.ToggleVisable();
        }

        UlanComponent ValidateComponent(string name)
        {
            UlanComponent comp = FindComponent(name);

            if (comp == null)
                throw new ArgumentNullException(string.Format("Component dosn't exist {0}", name));
            else
                return comp;
        }

        /// <summary>
        /// Used to find an UlanComponent.
        /// </summary>
        /// <param name="name">The name of the UlanComponent.</param>
        /// <returns>The UlanComponent.</returns>
        public static UlanComponent FindComponent(string name)
        {
            for (int i = 0; i < components.Count; i++)
                if (components[i].Name == name)
                    return components[i];

            return null;
        }
    }
}
