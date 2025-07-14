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

        //윈도우 창을 생성하는 코드
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

        //윈도우 창이 생성됬을때 호출
        public void CreateGUI()
        {
            ConstructGraphView();
            //자동 로드
            nodeGraphView?.LoadFromAsset(graphData);
        }

        //기본적인 배경 만들고 추가
        private void ConstructGraphView()
        {
            //view 공간 만들고 이름을 정해줌 이름은 개발자가 불러오거나 비교로 쓸 메타데이터일 뿐 사용자에게 영향을 끼치거나 보이지 않음

            var root = new VisualElement();
            root.style.position = Position.Relative; // Absolute가 동작하도록

            nodeGraphView = new NodeGraphView()
            {
                name = "nodeGraphView"
            };

            //공간을 얼마나 차지할지 1이면 남은 모든 공간을 차지
            root.style.flexGrow = 1;
            nodeGraphView.style.flexGrow = 1;

            root.Add(nodeGraphView);

            //팔레트 아이템 정보를 보여줄 인스펙터
            NodeInspectorView inspector = new();
            root.Add(inspector);

            // 팔레트 패널 (Shader Graph 스타일)
            NodeGraphPaletteView palette = new (nodeGraphView, inspector);
            root.Add(palette);

            //ui트리 최상단 여기에 등록된 순서로 에디터 창에 그려짐
            rootVisualElement.Add(root);
        }

        private void OnDisable()
        {
            nodeGraphView?.SaveToAsset(graphData);
        }
    }
}

