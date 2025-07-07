using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphEditor : EditorWindow
    {
        private NodeGraphView nodeGraphView;

        //������ â�� �����ϴ� �ڵ�
        [MenuItem("Window/Node Graph Editor")]
        public static void OpenGraphEditor()
        {
            var window = GetWindow<NodeGraphEditor>();
            window.titleContent = new GUIContent("Node Graph Editor");
        }

        //������ â�� �����ϴ� �ڵ�
        public static void OpenGraphEditor(NodeGraphData data)
        {
            var window = GetWindow<NodeGraphEditor>();
            window.titleContent = new GUIContent("Node Graph Editor");
            window.nodeGraphView.GraphData = data;
            window.nodeGraphView.Load();
        }

        //������ â�� ���������� ȣ��
        public void CreateGUI()
        {
            ConstructGraphView();
        }

        //�⺻���� ��� ����� �߰�
        private void ConstructGraphView()
        {
            //view ���� ����� �̸��� ������ �̸��� �����ڰ� �ҷ����ų� �񱳷� �� ��Ÿ�������� �� ����ڿ��� ������ ��ġ�ų� ������ ����

            var root = new VisualElement();
            root.style.position = Position.Relative; // Absolute�� �����ϵ���

            nodeGraphView = new NodeGraphView()
            {
                name = "nodeGraphView"
            };

            //������ �󸶳� �������� 1�̸� ���� ��� ������ ����
            root.style.flexGrow = 1;
            nodeGraphView.style.flexGrow = 1;

            //nodeGraphView.GraphData = nodeGraphData;

            root.Add(nodeGraphView);

            // �ȷ�Ʈ �г� (Shader Graph ��Ÿ��)
            NodeGraphPaletteView palette = new NodeGraphPaletteView(nodeGraphView);
            root.Add(palette);

            //uiƮ�� �ֻ�� ���⿡ ��ϵ� ������ ������ â�� �׷���
            rootVisualElement.Add(root);
        }

        private void OnDisable()
        {
            nodeGraphView?.AutoSave();
        }
    }
}

