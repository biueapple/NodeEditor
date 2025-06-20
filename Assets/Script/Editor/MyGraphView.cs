using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyGraphView : GraphView
{
    public MyGraphView()
    {
        // GraphView의 초기 설정을 진행합니다.
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.StretchToParentSize();

        // 기존 GridBackground를 제거하거나, 새 GridBackground 추가 후 원하는 배경색 지정
        var gridBackground = new GridBackground();
        // GridBackground의 기본 색상(회색)을 원하는 색(검은색)으로 변경
        gridBackground.style.backgroundColor = Color.black;
        // GraphView의 최하단 요소로 추가
        Insert(0, gridBackground);

        // 드래그 및 셀렉션 기능 추가 (필수는 아니지만 편의 기능) ContentDragger 대신 MyCustomContentDragger를 사용
        this.AddManipulator(new MyCustomContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }


    // BuildContextualMenu를 오버라이드하여 우클릭 시 메뉴 항목 구성
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // 기본 메뉴 항목을 유지할 필요가 없다면 base.BuildContextualMenu(evt) 호출을 생략할 수 있습니다.
        // base.BuildContextualMenu(evt);

        // "Create Node" 항목 추가: 메뉴 항목을 선택하면 CreateNode 메서드 호출
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
            // 같은 포트가 아니고, 같은 노드에 속하지 않으며,
            // 포트의 타입이 동일할 경우 연결 가능하도록
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
