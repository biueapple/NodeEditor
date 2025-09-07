using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    [System.Serializable]
    public class EdgeSaveData
    {
        public string outputNodeGUID;
        public string outputName;

        public string inputNodeGUID;
        public string inputName;

        public EdgeSaveData(Edge edge)
        {
            if(edge.output?.node is MyNode outNode)
            {
                outputNodeGUID = outNode.GUID;
                outputName = edge.output.portName;
            }

            if(edge.input?.node is MyNode inNode)
            {
                inputNodeGUID = inNode.GUID;
                inputName = edge.input.portName;
            }
        }
    }
}

