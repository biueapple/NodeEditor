using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NodeEditor
{
    //��� �׳� float�� ����ϸ� �Ǽ� �׳� ���������� ������ ���
    [NodeAttribute("IntO", description = "IntO", category = "Node/IntONode", isVisiblePalette = true)]
    public class IntONode : MyNode
    {
        public IntONode()
        {
            title = "IntONode";

            output.Add(InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int)));
            output[0].portName = "output";
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

