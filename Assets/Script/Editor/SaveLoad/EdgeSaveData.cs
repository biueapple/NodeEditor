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

        public EdgeSaveData(MyNode node)
        {
            for(int i = 0; i < node.output.Count; i++)
            {
                outputNodeGUID = node.GUID;
                outputName = node.output[i].portName;
            }

            if (node.input == null)
                return;

            inputNodeGUID = node.GUID;
            inputName = node.input.portName;
        }
    }
}

