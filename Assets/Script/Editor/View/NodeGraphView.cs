using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //에디터 윈도우를 꾸미는 요소중에 하나
    public class NodeGraphView : GraphView
    {
        private GridBackground gridBackground;
        public GridBackground GridBackground => gridBackground;

        private NodeGraphData graphData;
        public NodeGraphData GraphData
        {
            get => graphData;
            set { graphData = value; }
        }

        public NodeGraphView()
        {
            //줌 기능
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //배경
            gridBackground = new ();
            gridBackground.name = "gridBackground";

            //배경의 색깔을 정함
            gridBackground.style.backgroundColor = Color.black;

            //배경을 추가함
            Insert(0, gridBackground);

            CreateToolbar();

            //기능 추가 (노드 선택) 유니티 기능
            this.AddManipulator(new SelectionDragger());
            //기능 추가 (드래그로 영역을 선택) 유니티 기능
            this.AddManipulator(new RectangleSelector());
            //기능 추가 (마우스 중간으로 윈도우 이동) 커스텀 기능
            this.AddManipulator(new VisualElementContentViewDragger(this, MouseButton.MiddleMouse));
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

        //테스트 용으로 view 위에 툴바를 만들어 세이브 로드 버튼을 달아줌
        private void CreateToolbar()
        {
            var toolbar = new Toolbar();
            toolbar.name = "toolbar";

            Button saveButton = new Button(() => SaveToAsset(graphData)) { text = "Save" };
            Button loadButton = new Button(() => LoadFromAsset(graphData)) { text = "Load" };
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);

            Add(toolbar);
        }

        public void AutoSave()
        {
            SaveToAsset(graphData);
        }

        public void Load()
        {
            LoadFromAsset(graphData);
        }

        //노드와 선을 저장
        private void SaveToAsset(NodeGraphData asset)
        {
            //저장할 asset이 null이면 리턴
            if(asset == null)
            {
                Debug.Log("save null");
                return;
            }
            //일단 저장되어 있는거 없애고
            asset.nodes.Clear();
            asset.edges.Clear();

            //모든 속성에 대해서
            foreach (var element in graphElements)
            {
                //내가 정의한 노드라면
                if (element is MyNode node)
                {
                    var data = new NodeSaveData
                    {
                        type = node.GetType().Name,
                        guid = node.GUID,
                        position = node.GetPosition().position
                    };

                    //저장
                    asset.nodes.Add(data);
                }
            }

            //모든 선들도 마찬가지
            foreach(var edge in edges)
            {
                EdgeSaveData edgeSaveData = new EdgeSaveData(edge);
                asset.edges.Add(edgeSaveData);
            }

#if UNITY_EDITOR
            asset.hideFlags = HideFlags.None;   // 반드시 저장 전에 플래그 해제 이거 안하면 에러가 남 (왜 scriptobject에다가 Serializable 된 클래스인데도 에러가 나는건지)
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
#endif
        }

        //노드와 선을 로드하는 메소드
        private void LoadFromAsset(NodeGraphData asset)
        {
            //일단 현재 모든 속성들 없애야 함
            DeleteElements(graphElements);

            //불러올게 없나봐
            if (asset == null)
            {
                Debug.Log("load nu;;");
                return;
            }
                
            //모든 노드를 불러오기
            foreach(var data in asset.nodes)
            {
                //생성 가능한것들만
                if(NodeFactory.TryCreate(data.type, out MyNode node))
                {
                    node.GUID = data.guid;
                    node.SetPosition(new Rect(data.position, new Vector2(200, 150)));
                    AddElement(node);
                }
            }

            //모든 선들도 불러오기
            foreach(var data in asset.edges)
            {
                var outputNode = FindNodeByGUID(data.outputNodeGUID);
                var inputNode = FindNodeByGUID(data.inputNodeGUID);
                var outputPort = FindPort(outputNode, data.outputName, Direction.Output);
                var inputPort = FindPort(inputNode, data.inputName, Direction.Input);

                if(outputPort != null && inputPort != null)
                {
                    var edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);
                }
            }
        }

        //노드를 guid로 찾아오기
        private MyNode FindNodeByGUID(string guid)
        {
            foreach (var element in graphElements)
            {
                if (element is MyNode node)
                {
                    if (node.GUID.Equals(guid))
                        return node;
                }
            }
            return null;
        }

        //선들이 어느 노드의 포트에 연결되어 있는지 리턴
        private Port FindPort(MyNode node, string portName, Direction direction)
        {
            if (node == null)
                return null;

            if(direction == Direction.Input)
            {
                if (node.input != null && node.input.portName == portName)
                    return node.input;
            }
            else
            {
                foreach(var port in node.output)
                {
                    if (port.portName == portName)
                        return port;
                }
            }
            return null;
        }


        //노드를 선택하면 호출된다 가끔 한번 클릭해도 2번 호출된다 node의 콜백보다 늦음
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            Debug.Log("AddToSelection");
        }

        //floatIONode를 자신의 요소로 추가 factory o
        public void CreateFloatIONode(Vector2 position)
        {
            FloatIONode floatIONode = NodeFactory.Create<FloatIONode>();
            floatIONode.name = "floatIONode";
            floatIONode.SetPosition(new Rect(position, new Vector2(200, 150)));
            AddElement(floatIONode);
        }

        //factory x
        public void CreateFloatONode(Vector2 position)
        {
            FloatONode floatONode = new();
            floatONode.SetPosition(new Rect(position, new Vector2(200, 150)));
            AddElement(floatONode);
        }
    }
}

