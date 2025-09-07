using System.Collections.Generic;
using UnityEngine;

namespace NodeEditor
{
    //모든 노드를 저장할 스크립트
    [CreateAssetMenu(menuName = "NodeGraph/GraphData")]
    public class NodeGraphData : ScriptableObject
    {
        public List<NodeSaveData> nodes = new();
        public List<EdgeSaveData> edges = new();
    }
}


