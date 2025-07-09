using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditor
{
    public static class NodeFactory
    {
        private readonly static Dictionary<Type, NodeMetaData> nodeConstructor = new()
        {
            { typeof(FloatIONode), new NodeMetaData()
                {
                    displayName = "FloatIO",
                    description = "floatIO ���",
                    category = "Node/FloatIONode",
                    icon = null,
                    constructor = () => new FloatIONode(),
                    size = new Vector2(200, 150),
                    isVisiblePalette = false
                }
            },
            {
                typeof(FloatONode), new NodeMetaData()
                {
                    displayName = "FloatO",
                    description = "floatO ���",
                    category = "Node/FloatONode",
                    icon = null,
                    constructor = () => new FloatONode(),
                    size = new Vector2(200, 150),
                    isVisiblePalette = true
                }
            }
        };
        //IReadOnlyDictionary�� �ܺο��� add remove�� �Ұ����ϸ鼭 key�� ������ ������ c#�� �������ִ� ���
        public static IReadOnlyDictionary<Type, NodeMetaData> NodeConstructor => nodeConstructor;

        private readonly static Dictionary<string, Type> nameToType = new()
        {
            { "FloatIONode", typeof(FloatIONode) },
            { "FloatONode", typeof(FloatONode) },
        };
        public static IReadOnlyDictionary<string, Type> NameToType => nameToType;

        public static T Create<T>() where T : MyNode
        {
            if(nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
            {
                if (metaData.constructor() is T result)
                    return result;
            }

            Debug.Log("���� Ÿ���� ��û��");
            return null;
        }

        public static T Create<T>(Vector2 position) where T : MyNode
        {
            if (nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
            {
                if (metaData.constructor() is T result)
                {
                    result.SetPosition(new Rect(position, metaData.size));
                    return result;
                }
            }

            Debug.Log("���� Ÿ���� ��û��");
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

        public static bool TryCreate(string typeName, Vector2 position , out MyNode node)
        {
            node = null;

            if (nameToType.TryGetValue(typeName, out Type type))
            {
                if (nodeConstructor.TryGetValue(type, out NodeMetaData metaData))
                {
                    node = metaData.constructor();
                    node.SetPosition(new Rect(position, metaData.size));
                }
            }

            return node != null;
        }

        public static void Regist<T>(NodeMetaData metaData) where T : MyNode
        {
            //� ��尡 � ��Ÿ�����͸� �������� �߰�
            nodeConstructor[typeof(T)] = metaData;
            //� �̸��� � Ÿ������ �߰�
            nameToType[typeof(T).Name] = typeof(T);
        }
    }
}

