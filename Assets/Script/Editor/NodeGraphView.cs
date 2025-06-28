using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //������ �����츦 �ٹ̴� ����߿� �ϳ�
    public class NodeGraphView : GraphView
    {
        public NodeGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //���
            GridBackground gridBackground = new ();
            //����� ������ ����
            gridBackground.style.backgroundColor = Color.black;

            //����� �߰���
            Insert(0, gridBackground);

            //��� �߰� (�巡�׷� ������ ����)
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        //��Ŭ�� ������ ������ �޴��� ���ϴ� �޼ҵ�
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //������ ������ �޴��� �ϴ� ���ܺ���
            base.BuildContextualMenu(evt);
            //��带 �����Ҷ� ���� ������ �����Ǵ� ������ �߻�

            //���콺�� ��ġ�� �޾� ������ �����ؼ� ��ǥ�� ���� �� ���� 
            //�ٵ� �� ������ this�� ���� �ȵǰ� evt.target as VisualElement�� �ϸ� �Ǵ°����� �𸣰���
            //this�� ���� �ȵǴ� ������ �̺�Ʈ Ÿ���� graphview�� �ƴ� �� �ֱ� ������ �׻� graphview�� �������� �ϴ°��� ��ǥ �ؼ��� ��ų �� �ִٰ� ��
            Vector2 worldMouse = evt.mousePosition;
            Vector2 localMouse = (evt.target as VisualElement).ChangeCoordinatesTo(contentViewContainer, worldMouse);
            evt.menu.AppendAction("FloatIONode", action => CreateFloatIONode(localMouse));
        }

        //� port�� ���� ������ ��Ʈ���� ��ź���ִ� �޼ҵ�
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new ();
            ports.ForEach((port) =>
            {
                // ���� ��Ʈ�� �ƴϰ�, ���� ��忡 ������ ������,
                // ��Ʈ�� Ÿ���� ������ ��� ���� �����ϵ���
                if (startPort != port &&
                startPort.node != port.node &&
                port.portType == startPort.portType &&
                startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }

        //��带 �����ϸ� ȣ��ȴ� ���� �ѹ� Ŭ���ص� 2�� ȣ��ȴ� node�� �ݹ麸�� ����
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            Debug.Log("AddToSelection");
        }

        //floatIONode�� �ڽ��� ��ҷ� �߰�
        private void CreateFloatIONode(Vector2 position)
        {
            FloatIONode floatIONode = new ();
            floatIONode.SetPosition(new Rect(position, new Vector2(200, 150)));
            AddElement(floatIONode);
        }
    }
}

