using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphEditor : EditorWindow
    {
        private NodeGraphView nodeGraphView;

        [SerializeField]
        private NodeGraphData graphData;

        //������ â�� �����ϴ� �ڵ�
        public static void OpenGraphEditor(NodeGraphData data)
        {
            var window = GetWindow<NodeGraphEditor>();

            if(window.nodeGraphView != null && window.graphData != null && window.graphData != data)
            {
                window.nodeGraphView.SaveToAsset(window.graphData);
            }

            window.titleContent = new GUIContent("Node Graph Editor");
            window.graphData = data;
            window.nodeGraphView?.LoadFromAsset(data);
        }

        public void OnEnable()
        {
            if (graphData == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:NodeGraphData");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    graphData = AssetDatabase.LoadAssetAtPath<NodeGraphData>(path);
                }
            }
        }

        //������ â�� ���������� ȣ��
        public void CreateGUI()
        {
            ConstructGraphView();
            //�ڵ� �ε�
            nodeGraphView?.LoadFromAsset(graphData);
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

            root.Add(nodeGraphView);

            //�ȷ�Ʈ ������ ������ ������ �ν�����
            NodeInspectorView inspector = new();
            root.Add(inspector);

            // �ȷ�Ʈ �г� (Shader Graph ��Ÿ��)
            NodeGraphPaletteView palette = new (nodeGraphView, inspector);
            root.Add(palette);

            //uiƮ�� �ֻ�� ���⿡ ��ϵ� ������ ������ â�� �׷���
            rootVisualElement.Add(root);
        }

        private void OnDisable()
        {
            nodeGraphView?.SaveToAsset(graphData);
        }
    }
}

