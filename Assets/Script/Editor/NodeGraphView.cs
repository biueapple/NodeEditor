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

        [SerializeField]
        public NodeGraphData graphData;

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

            graphData = AssetDatabase.LoadAssetAtPath<NodeGraphData>("Assets/Script/Editor/NodeGraphData/Node Graph Data.asset");
            if (graphData == null)
            {
                graphData = ScriptableObject.CreateInstance<NodeGraphData>();
                AssetDatabase.CreateAsset(graphData, "Assets/Script/Editor/NodeGraphData/Node Graph Data.asset");
                AssetDatabase.SaveAssets();
            }
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

        private void SaveToAsset(NodeGraphData asset)
        {
            asset.nodes.Clear();

            foreach (var element in graphElements)
            {
                if (element is MyNode node)
                {
                    var data = new NodeSaveData
                    {
                        type = node.GetType().Name,
                        guid = node.GUID,
                        position = node.GetPosition().position
                    };
                    asset.nodes.Add(data);
                }
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
#endif
        }

        private void LoadFromAsset(NodeGraphData asset)
        {
            DeleteElements(graphElements);

            foreach(var data in asset.nodes)
            {
                if(NodeFactory.TryCreate(data.type, out MyNode node))
                {
                    node.GUID = data.guid;
                    node.SetPosition(new Rect(data.position, new Vector2(200, 150)));
                    AddElement(node);
                }
            }
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

