using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NodeEditor
{
    [NodeAttribute("Plus", description = "plus", category = "Func/Plus", isVisiblePalette = false)]
    public class PlusNode : MyNode
    {
        public PlusNode()
        {
            title = "Plus";

            Port port1 = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            port1.portName = "port1";
            Port port2 = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            port2.portName = "port2";

            inputContainer.Add(port1);
            inputContainer.Add(port2);

            input.Add(port1);
            input.Add(port2);

            Port output1 = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            output1.portName = "output";
            outputContainer.Add(output1);
            output.Add(output1);
        }

        public override void Execute()
        {
            Debug.Log(input[0].node.name);
        }
    }
}

