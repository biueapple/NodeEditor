using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //에디터 윈도우를 꾸미는 요소중에 하나
    public class NodeGraphView : GraphView
    {
        public NodeGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //배경
            GridBackground gridBackground = new ();
            //배경의 색깔을 정함
            gridBackground.style.backgroundColor = Color.black;

            //배경을 추가함
            Insert(0, gridBackground);

            //기능 추가 (드래그로 영역을 선택)
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        //우클릭 누르면 나오는 메뉴를 정하는 메소드
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //기존에 나오는 메뉴를 일단 남겨보자
            base.BuildContextualMenu(evt);
            //노드를 생성할때 왼쪽 위에서 생성되는 문제가 발생

            //마우스의 위치를 받아 기준을 변경해서 좌표를 변경 후 적용 
            //근데 왜 기준이 this를 쓰면 안되고 evt.target as VisualElement를 하면 되는건지는 모르겠음
            //this를 쓰면 안되는 이유는 이벤트 타겟이 graphview가 아닐 수 있기 때문에 항상 graphview를 기준으로 하는것은 죄표 해석이 엉킬 수 있다고 함
            Vector2 worldMouse = evt.mousePosition;
            Vector2 localMouse = (evt.target as VisualElement).ChangeCoordinatesTo(contentViewContainer, worldMouse);
            evt.menu.AppendAction("FloatIONode", action => CreateFloatIONode(localMouse));
        }

        //어떤 port가 연결 가능한 포트인지 판탄해주는 메소드
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new ();
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

        //노드를 선택하면 호출된다 가끔 한번 클릭해도 2번 호출된다 node의 콜백보다 늦음
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            Debug.Log("AddToSelection");
        }

        //floatIONode를 자신의 요소로 추가
        private void CreateFloatIONode(Vector2 position)
        {
            FloatIONode floatIONode = new ();
            floatIONode.SetPosition(new Rect(position, new Vector2(200, 150)));
            AddElement(floatIONode);
        }
    }
}

