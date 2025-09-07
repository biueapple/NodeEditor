using System;
using UnityEngine;

namespace NodeEditor
{
    public class NodeMetaData
    {
        public string displayName;
        public string description;
        public string category;
        public Texture2D icon;
        public Func<MyNode> constructor;
        public Vector2 size;
        public bool isVisiblePalette = true;
    }
}
