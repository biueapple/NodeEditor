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
        private readonly GridBackground gridBackground;
        public GridBackground GridBackground => gridBackground;

        public NodeGraphView()
        {
            //�� ���
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            //���
            gridBackground = new()
            {
                name = "gridBackground"
            };

            //����� ������ ����
            gridBackground.style.backgroundColor = Color.black;

            //����� �߰���
            Insert(0, gridBackground);

            //CreateToolbar();

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
            //evt.menu.AppendAction("FloatIONode", action => CreateFloatIONode(localMouse));

            foreach (var kv in NodeFactory.NodeConstructor)
            {
                var type = kv.Key;
                var meta = kv.Value;
                //�ȷ�Ʈ�� ���̴°� ��Ŭ������ ���̸� �ȵ���
                if (meta.isVisiblePalette)
                    continue;
                evt.menu.AppendAction(meta.displayName, _ => { var node = NodeFactory.Create(type, localMouse); if (node != null) AddElement(node); });
            }
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

        //���� ���� ����
        public void SaveToAsset(NodeGraphData asset)
        {
            //������ asset�� null�̸� ����
            if (asset == null)
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
                        position = node.GetPosition().position,
                        size = node.GetPosition().size
                    };

                    //����
                    asset.nodes.Add(data);
                }
            }

            //��� ���鵵 ��������
            foreach(var edge in edges)
            {
                EdgeSaveData edgeSaveData = new (edge);
                asset.edges.Add(edgeSaveData);
            }

#if UNITY_EDITOR
            asset.hideFlags = HideFlags.None;   // �ݵ�� ���� ���� �÷��� ���� �̰� ���ϸ� ������ �� (�� scriptobject���ٰ� Serializable �� Ŭ�����ε��� ������ ���°���)
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
#endif
        }

        //���� ���� �ε��ϴ� �޼ҵ�
        public void LoadFromAsset(NodeGraphData asset)
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
                    node.SetPosition(new Rect(data.position, data.size != Vector2.zero ? data.size : NodeFactory.NodeConstructor[NodeFactory.NameToType[data.type]].size));
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
                foreach (var port in node.input)
                {
                    if (port != null && port.portName == portName)
                        return port;
                }
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


        //������ ����
        public MyNode CreateNode(string typeName, Vector2 position)
        {
            if(NodeFactory.TryCreate(typeName, position, out MyNode node))
            {
                node.name = "typeName";
                AddElement(node);
                return node;
            }
            return null;
        }
    }
}

