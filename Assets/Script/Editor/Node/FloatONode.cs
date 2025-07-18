using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    [NodeAttribute("FloatO", description = "FloatO", category = "/Node/FloatONode", isVisiblePalette = true)]
    public class FloatONode : MyNode
    {
        public FloatONode() : base()
        {
            title = "Float O Node";

            output.Add(InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float)));
            output[0].portName = "Output";
            outputContainer.Add(output[0]);

            RefreshExpandedState();
            RefreshPorts();
        }

        public override void Execute()
        {
            Debug.Log("Execute");
        }
    }
}
