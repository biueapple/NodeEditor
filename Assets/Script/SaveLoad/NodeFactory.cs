using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public static class NodeFactory
    {
        private readonly static Dictionary<Type, NodeMetaData> nodeConstructor = new();
        //IReadOnlyDictionary�� �ܺο��� add remove�� �Ұ����ϸ鼭 key�� ������ ������ c#�� �������ִ� ���
        public static IReadOnlyDictionary<Type, NodeMetaData> NodeConstructor => nodeConstructor;

        private readonly static Dictionary<string, Type> nameToType = new();
        public static IReadOnlyDictionary<string, Type> NameToType => nameToType;


        static NodeFactory()
        {
            RegisterAllNodes();
        }

        //attributeusage nodeattribute�� ������ �ִ� ��� ������ ������ dictionary�� �����ϴ� �ܰ�
        private static void RegisterAllNodes()
        {
            foreach(var type in TypeCache.GetTypesDerivedFrom<MyNode>())
            {
                //�߻� Ŭ������ �н�
                if (type.IsAbstract)
                    continue;

                var attr = type.GetCustomAttribute<NodeAttribute>();
                if (attr == null)
                    return;

                NodeMetaData metaData = new()
                {
                    displayName = attr.displayName,
                    description = attr.description,
                    category = attr.category,
                    icon = null,
                    size = new Vector2(attr.width, attr.height),
                    isVisiblePalette = attr.isVisiblePalette,
                    constructor = () => Activator.CreateInstance(type) as MyNode
                };

                nodeConstructor[type] = metaData;
                nameToType[type.Name] = type;
            }
        }

        //�̰� �̷��� ���� ���ٸ� �ؿ� �޼ҵ���� �� ������� gpt�� ��Ű�´�� �������� ���׸����� T ���鼭 �����ɸ��鼭 ����ǵ�
        public static MyNode Create(Type type, Vector2 position)
        {
            if(nodeConstructor.TryGetValue(type, out NodeMetaData metaData))
            {
                var node = metaData.constructor();
                node.SetPosition(new Rect(position, metaData.size));
                return node;
            }
            return null;
        }

        //�ϴ� �Ⱦ��� �޼ҵ���� �ּ�ó���ϰ� ���߿� ó���ϴ°ɷ�
        //public static T Create<T>() where T : MyNode
        //{
        //    if(nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
        //    {
        //        if (metaData.constructor() is T result)
        //            return result;
        //    }

        //    Debug.Log("���� Ÿ���� ��û��");
        //    return null;
        //}

        //public static T Create<T>(Vector2 position) where T : MyNode
        //{
        //    if (nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
        //    {
        //        if (metaData.constructor() is T result)
        //        {
        //            result.SetPosition(new Rect(position, metaData.size));
        //            return result;
        //        }
        //    }

        //    Debug.Log("���� Ÿ���� ��û��");
        //    return null;
        //}

        //public static bool TryCreate<T>(out T node) where T : MyNode
        //{
        //    node = null;

        //    if (nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
        //    {
        //        if (metaData.constructor() is T result)
        //            node = result;
        //    }

        //    return node != null;
        //}

        //�̰͵鵵 �Ƹ� ����� �ٽ� ���� �� �ֱ��ѵ� string���� �޴� Ư�������� ������������
        //nodegraphview�� load ��ɿ��� ȣ������ metadata�� Type���� ������ �� ���ٴ� Ư���� �־
        //�̰� ��ü �Ұ����Ѱ� ���⵵ �ϰ�
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

        //nodegraphview�� createfloatnode �Ҷ� ȣ���ϰ� �ִµ� ��� ����� �޼ҵ�� �̰͵� ����� ���ɼ� ����
        //���ʿ� ������ ���� factory -> ���÷����� ����ϴµ� nodegraphview�� ���� �޼ҵ尡 �ʿ��Ѱ�
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
    }
}

