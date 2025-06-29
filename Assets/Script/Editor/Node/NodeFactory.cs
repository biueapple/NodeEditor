using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditor
{
    public static class NodeFactory
    {
        private static Dictionary<Type, NodeMetaData> nodeConstructor = new()
        {
            { typeof(FloatIONode), new NodeMetaData()
                {
                    displayName = "FloatIO",
                    description = "float 노드",
                    category = "Node/FloatNode",
                    icon = null,
                    constructor = () => new FloatIONode(),
                    isVisiblePalette = false
                } 
            }
        };
        private static Dictionary<string, Type> nameToType = new()
        {
            { "FloatIONode", typeof(FloatIONode) }
        };

        public static T Create<T>() where T : MyNode
        {
            if(nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
            {
                if (metaData.constructor() is T result)
                    return result;
            }

            Debug.Log("없는 타입을 요청함");
            return null;
        }

        public static bool TryCreate<T>(out T node) where T : MyNode
        {
            node = null;

            if (nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
            {
                if (metaData.constructor() is T result)
                    node = result;
            }

            return node != null;
        }

        public static bool TryCreate(string typeName, out MyNode node)
        {
            node = null;

            if (nameToType.TryGetValue(typeName, out Type type))
            {
                if (nodeConstructor.TryGetValue(type, out NodeMetaData metaData))
                {
                    node = metaData.constructor();
                }
            }

            return node != null;
        }

        public static void Regist<T>(NodeMetaData metaData) where T : MyNode
        {
            nodeConstructor[typeof(T)] = metaData;
        }
    }
}

