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
        //IReadOnlyDictionary는 외부에서 add remove가 불가능하면서 key로 접근은 가능한 c#이 제공해주는 기능
        public static IReadOnlyDictionary<Type, NodeMetaData> NodeConstructor => nodeConstructor;

        private readonly static Dictionary<string, Type> nameToType = new();
        public static IReadOnlyDictionary<string, Type> NameToType => nameToType;


        static NodeFactory()
        {
            RegisterAllNodes();
        }

        //attributeusage nodeattribute를 가지고 있는 모든 노드들의 정보를 dictionary에 저장하는 단계
        private static void RegisterAllNodes()
        {
            foreach(var type in TypeCache.GetTypesDerivedFrom<MyNode>())
            {
                //추상 클래스는 패스
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

        //이걸 이렇게 만들어서 쓴다면 밑에 메소드들은 왜 만든건지 gpt가 시키는대로 귀찮지만 제네릭으로 T 쓰면서 오래걸리면서 만든건데
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

        //일단 안쓰는 메소드들을 주석처리하고 나중에 처리하는걸로
        //public static T Create<T>() where T : MyNode
        //{
        //    if(nodeConstructor.TryGetValue(typeof(T), out NodeMetaData metaData))
        //    {
        //        if (metaData.constructor() is T result)
        //            return result;
        //    }

        //    Debug.Log("없는 타입을 요청함");
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

        //    Debug.Log("없는 타입을 요청함");
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

        //이것들도 아마 지우고 다시 만들 수 있긴한데 string으로 받는 특성때문에 남아있을지도
        //nodegraphview에 load 기능에서 호출중임 metadata에 Type형은 저장할 수 없다는 특성이 있어서
        //이건 대체 불가능한거 같기도 하고
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

        //nodegraphview가 createfloatnode 할때 호출하고 있는데 사실 사라질 메소드라서 이것도 사라질 가능성 있음
        //애초에 생성을 전부 factory -> 리플렉션을 사용하는데 nodegraphview에 따로 메소드가 필요한가
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

