using System.Collections.Generic;
using UnityEngine;

namespace NodeEditor
{
    //��� ��带 ������ ��ũ��Ʈ
    [CreateAssetMenu(menuName = "NodeGraph/GraphData")]
    public class NodeGraphData : ScriptableObject
    {
        public List<NodeSaveData> nodes = new();
        public List<EdgeSaveData> edges = new();
    }
}


