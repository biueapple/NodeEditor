using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //������ �����츦 �ٹ̴� ����߿� �ϳ�
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
            //�� ���
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //���
            gridBackground = new ();
            gridBackground.name = "gridBackground";

            //����� ������ ����
            gridBackground.style.backgroundColor = Color.black;

            //����� �߰���
            Insert(0, gridBackground);

            CreateToolbar();

            //��� �߰� (��� ����) ����Ƽ ���
            this.AddManipulator(new SelectionDragger());
            //��� �߰� (�巡�׷� ������ ����) ����Ƽ ���
            this.AddManipulator(new RectangleSelector());
            //��� �߰� (���콺 �߰����� ������ �̵�) Ŀ���� ���
            this.AddManipulator(new VisualElementContentViewDragger(this, MouseButton.MiddleMouse));
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

        //�׽�Ʈ ������ view ���� ���ٸ� ����� ���̺� �ε� ��ư�� �޾���
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

        //���� ���� ����
        private void SaveToAsset(NodeGraphData asset)
        {
            //������ asset�� null�̸� ����
            if(asset == null)
            {
                Debug.Log("save null");
                return;
            }
            //�ϴ� ����Ǿ� �ִ°� ���ְ�
            asset.nodes.Clear();
            asset.edges.Clear();

            //��� �Ӽ��� ���ؼ�
            foreach (var element in graphElements)
            {
                //���� ������ �����
                if (element is MyNode node)
                {
                    var data = new NodeSaveData
                    {
                        type = node.GetType().Name,
                        guid = node.GUID,
                        position = node.GetPosition().position
                    };

                    //����
                    asset.nodes.Add(data);
                }
            }

            //��� ���鵵 ��������
            foreach(var edge in edges)
            {
                EdgeSaveData edgeSaveData = new EdgeSaveData(edge);
                asset.edges.Add(edgeSaveData);
            }

#if UNITY_EDITOR
            asset.hideFlags = HideFlags.None;   // �ݵ�� ���� ���� �÷��� ���� �̰� ���ϸ� ������ �� (�� scriptobject���ٰ� Serializable �� Ŭ�����ε��� ������ ���°���)
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
#endif
        }

        //���� ���� �ε��ϴ� �޼ҵ�
        private void LoadFromAsset(NodeGraphData asset)
        {
            //�ϴ� ���� ��� �Ӽ��� ���־� ��
            DeleteElements(graphElements);

            //�ҷ��ð� ������
            if (asset == null)
            {
                Debug.Log("load nu;;");
                return;
            }
                
            //��� ��带 �ҷ�����
            foreach(var data in asset.nodes)
            {
                //���� �����Ѱ͵鸸
                if(NodeFactory.TryCreate(data.type, out MyNode node))
                {
                    node.GUID = data.guid;
                    node.SetPosition(new Rect(data.position, new Vector2(200, 150)));
                    AddElement(node);
                }
            }

            //��� ���鵵 �ҷ�����
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

        //��带 guid�� ã�ƿ���
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

        //������ ��� ����� ��Ʈ�� ����Ǿ� �ִ��� ����
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


        //��带 �����ϸ� ȣ��ȴ� ���� �ѹ� Ŭ���ص� 2�� ȣ��ȴ� node�� �ݹ麸�� ����
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            Debug.Log("AddToSelection");
        }

        //floatIONode�� �ڽ��� ��ҷ� �߰� factory o
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

