using System;
using UnityEngine;

namespace NodeEditor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeAttribute : Attribute
    {
        public string displayName;
        public string description;
        public string category;
        public float width;
        public float height;
        public bool isVisiblePalette;

        public NodeAttribute(string displayName, string description = "", string category = "Node", float width = 200, float height = 150, bool isVisiblePalette = true)
        {
            this.displayName = displayName;
            this.description = description;
            this.category = category;
            this.width = width;
            this.height = height;
            this.isVisiblePalette = isVisiblePalette;
        }
    }
}

