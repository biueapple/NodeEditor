using System.Collections.Generic;
using UnityEngine;

namespace NodeEditor
{
    public static class NodeGraphRunner
    {
        public static void Run(NodeGraphData data)
        {
            if (data == null)
            {
                Debug.Log("data is null");
                return;
            }

            //저장된 노드 불러오기
            Dictionary<string, MyNode> guidToNode = new();
            foreach(var nodeSave in data.nodes)
            {
                if(NodeFactory.TryCreate(nodeSave.type, out MyNode node))
                {
                    node.GUID = nodeSave.guid;
                    guidToNode[node.GUID] = node;
                }
            }

            Dictionary<string, List<string>> adjacency = new();
            HashSet<string> hasInput = new();
            foreach(var edge in data.edges)
            {
                if(!adjacency.TryGetValue(edge.outputNodeGUID, out var list))
                {
                    list = new();
                    adjacency[edge.outputNodeGUID] = list;
                }
                list.Add(edge.inputNodeGUID);
                hasInput.Add(edge.inputNodeGUID);
            }

            HashSet<string> visited = new();
            foreach(var guid in guidToNode.Keys)
            {
                if(!hasInput.Contains(guid))
                {
                    ExcuteRecursive(guid);
                }
            }

            void ExcuteRecursive(string guid)
            {
                if (visited.Contains(guid))
                    return;
                if (!guidToNode.TryGetValue(guid, out var node))
                    return;

                visited.Add(guid);
                node.Execute();

                if(adjacency.TryGetValue(guid, out var nextList))
                {
                    foreach(var next in nextList)
                    {
                        ExcuteRecursive(next);
                    }
                }
            }
        }
    }
}

