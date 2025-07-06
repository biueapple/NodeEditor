using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //Input = float, output = float 인 노드
    public class FloatIONode : MyNode
    {
        public FloatIONode() : base()
        {
            title = "Float IO Node";

            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            input.portName = "Input";

            //node 등록과 삭제 관련된 클래스인듯
            MyEdgeConnector edgeConnector = new ();
            input.AddManipulator(edgeConnector);

            inputContainer.Add(input);

            output.Add(InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float)));
            output[0].portName = "Output";
            outputContainer.Add(output[0]);

            RefreshExpandedState();
            RefreshPorts();
        }

        public override void Excute()
        {
            Debug.Log("Excute");
        }
    }
}
