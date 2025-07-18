using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphEditor : EditorWindow
    {
        private NodeGraphView nodeGraphView;
        private NodeGraphPaletteView paletteView;
        private NodeInspectorView inspectorView;


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
            window.paletteView?.LoadFromGraph();
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
            paletteView?.LoadFromGraph();
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
            inspectorView = new();
            root.Add(inspectorView);

            // �ȷ�Ʈ �г� (Shader Graph ��Ÿ��)
            paletteView = new (nodeGraphView, inspectorView);
            root.Add(paletteView);

            //uiƮ�� �ֻ�� ���⿡ ��ϵ� ������ ������ â�� �׷���
            rootVisualElement.Add(root);
        }

        private void OnDisable()
        {
            nodeGraphView?.SaveToAsset(graphData);
        }
    }
}

