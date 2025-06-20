using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class MyNodeGraphEditor : EditorWindow
{
    private GraphView graphView;

    [MenuItem("Window/Node Graph Editor")]
    public static void OpenGraphEditor()
    {
        MyNodeGraphEditor window = GetWindow<MyNodeGraphEditor>();
        window.titleContent = new GUIContent("Node Graph Editor");
    }

    public void CreateGUI()
    {
        ConstructGraphView();
        CreateToolbar();
    }

    private void CreateToolbar()
    {
        // 툴바 컨테이너 생성
        Toolbar toolbar = new Toolbar();

        // 버튼 생성 및 이벤트 바인딩
        Button applyButton = new Button(ApplyChanges)
        {
            text = "Apply"
        };
        toolbar.Add(applyButton);

        // 툴바를 최상단에 추가
        rootVisualElement.Add(toolbar);
    }

    private void OnEnable()
    {
        ConstructGraphView();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new MyGraphView
        {
            name = "My Node Graph"
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    // Apply 버튼이 눌렸을 때 실행할 로직
    private void ApplyChanges()
    {
        Debug.Log("Apply 버튼 클릭 - 노드에 변경 사항을 적용합니다.");
        // 1. GraphView 내의 모든 MyNode 리스트 가져오기
        var allNodes = graphView.nodes.OfType<MyNode>().ToList();

        // 2. 각 노드의 의존성 카운트(입력 연결 수)를 계산
        Dictionary<MyNode, int> dependencyCount = new Dictionary<MyNode, int>();
        foreach (MyNode node in allNodes)
        {
            // 각 노드의 inputPort에 연결된 엣지 수가 그 노드의 의존성 수입니다.
            dependencyCount[node] = node.inputPort.connections.Count();
        }

        // 3. 의존성이 없는 노드(즉, 입력 연결이 없는 노드)를 실행 큐에 추가
        Queue<MyNode> readyNodes = new Queue<MyNode>();
        foreach (var kvp in dependencyCount)
        {
            if (kvp.Value == 0)
            {
                readyNodes.Enqueue(kvp.Key);
            }
        }

        // 4. 위상 정렬 방식으로 평가 순서를 계산하며 노드 실행
        while (readyNodes.Count > 0)
        {
            MyNode currentNode = readyNodes.Dequeue();

            // 현재 노드 실행
            currentNode.Evaluate();

            // 현재 노드의 출력 포트 연결을 통해 연결된 모든 후속 노드를 찾아 의존성 카운트를 감소
            foreach (Edge edge in currentNode.outputPort.connections)
            {
                // edge.input은 연결된 입력 포트이므로, 해당 포트가 소유한 노드가 후속 노드입니다.
                MyNode targetNode = edge.input.node as MyNode;
                if (targetNode != null)
                {
                    dependencyCount[targetNode]--;
                    // 의존성이 0이 되면 실행 가능한 상태이므로 큐에 추가합니다.
                    if (dependencyCount[targetNode] == 0)
                    {
                        readyNodes.Enqueue(targetNode);
                    }
                }
            }
        }
    }
}
