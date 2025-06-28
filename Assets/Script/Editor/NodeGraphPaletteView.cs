using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphPaletteView : GraphView
    {
        private NodeGraphView nodeGraphView;
        public NodeGraphPaletteView(NodeGraphView nodeGraphView)
        {
            this.nodeGraphView = nodeGraphView;

            style.position = Position.Absolute;
            style.left = 20;
            style.top = 20;
            style.width = 250;
            style.height = 400;
            style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            style.borderTopLeftRadius = 4;
            style.borderTopRightRadius = 4;
            style.borderBottomLeftRadius = 4;
            style.borderBottomRightRadius = 4;
            style.paddingLeft = 10;
            style.paddingTop = 10;

            VisualElement topElement = new VisualElement();
            topElement.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            //�ڽ��� ũ�⸦ �ڵ����� �������ִ°� (���̴� �������� ���ϰ� �¿�� �ڵ����� �����ֱ�)
            topElement.style.alignSelf = Align.Stretch;
            topElement.style.height = 50;
            //���� �� �� �е� ���ֱ�
            topElement.style.marginLeft = -10;
            topElement.style.marginRight = 10;
            topElement.style.marginTop = -10;
            topElement.style.marginBottom = 10;

            //�� �߰�
            Label label = new Label("Node Graph");
            //�ؽ�Ʈ ����
            label.style.unityTextAlign = TextAnchor.UpperLeft;
            label.style.marginTop = 10;
            label.style.marginLeft = 10;

            Button plus = new Button(() => AddParam()) { text = "+" };
            plus.style.alignSelf = Align.FlexEnd;
            plus.style.width = 20;
            plus.style.height = 20;

            topElement.Add(label);
            topElement.Add(plus);

            Add(topElement);

            //���콺 Ŭ������ �巡���Ͽ� �����̵��� �ϴ� ��� �߰�
            this.AddManipulator(new VisualElementDragger(MouseButton.LeftMouse));
        }

        //�ϴ� �׽�Ʈ �뵵�� floatIONode�� ȭ�� �߾ӿ� ����
        private void AddParam()
        {
            Vector2 viewCenter = nodeGraphView.layout.size / 2;
            //��� ũ�⸦ ����ؼ� �� �߾�����
            //Vector2 nodeSize = new Vector2(200, 150);
            //Vector2 nodePosition = viewCenter - nodeSize / 2;

            nodeGraphView.CreateFloatIONode(viewCenter);
        }
    }
}
