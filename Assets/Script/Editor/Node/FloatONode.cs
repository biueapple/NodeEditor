using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    public class FloatONode : MyNode
    {
        private Port output;

        public FloatONode() : base()
        {
            title = "Float IO Node";

            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            output.portName = "Output";
            outputContainer.Add(output);

            RefreshExpandedState();
            RefreshPorts();
        }

        public override void Excute()
        {
            Debug.Log("Excute");
        }
    }
}
