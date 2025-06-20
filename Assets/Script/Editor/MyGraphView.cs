using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyGraphView : GraphView
{
    public MyGraphView()
    {
        // GraphView�� �ʱ� ������ �����մϴ�.
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.StretchToParentSize();

        // ���� GridBackground�� �����ϰų�, �� GridBackground �߰� �� ���ϴ� ���� ����
        var gridBackground = new GridBackground();
        // GridBackground�� �⺻ ����(ȸ��)�� ���ϴ� ��(������)���� ����
        gridBackground.style.backgroundColor = Color.black;
        // GraphView�� ���ϴ� ��ҷ� �߰�
        Insert(0, gridBackground);

        // �巡�� �� ������ ��� �߰� (�ʼ��� �ƴ����� ���� ���) ContentDragger ��� MyCustomContentDragger�� ���
        this.AddManipulator(new MyCustomContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }


    // BuildContextualMenu�� �������̵��Ͽ� ��Ŭ�� �� �޴� �׸� ����
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // �⺻ �޴� �׸��� ������ �ʿ䰡 ���ٸ� base.BuildContextualMenu(evt) ȣ���� ������ �� �ֽ��ϴ�.
        // base.BuildContextualMenu(evt);

        // "Create Node" �׸� �߰�: �޴� �׸��� �����ϸ� CreateNode �޼��� ȣ��
        evt.menu.AppendAction("Create FloatCalculationNode", action => CreateFloatCalculationNode(evt.mousePosition));
    }

    private void CreateFloatCalculationNode(Vector2 position)
    {
        FloatCalculationNode floatCalculationNode = new FloatCalculationNode();
        floatCalculationNode.SetPosition(new Rect(position, new Vector2(200, 150)));
        AddElement(floatCalculationNode);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
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


}
