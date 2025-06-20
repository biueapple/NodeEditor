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
        // ���� �����̳� ����
        Toolbar toolbar = new Toolbar();

        // ��ư ���� �� �̺�Ʈ ���ε�
        Button applyButton = new Button(ApplyChanges)
        {
            text = "Apply"
        };
        toolbar.Add(applyButton);

        // ���ٸ� �ֻ�ܿ� �߰�
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

    // Apply ��ư�� ������ �� ������ ����
    private void ApplyChanges()
    {
        Debug.Log("Apply ��ư Ŭ�� - ��忡 ���� ������ �����մϴ�.");
        // 1. GraphView ���� ��� MyNode ����Ʈ ��������
        var allNodes = graphView.nodes.OfType<MyNode>().ToList();

        // 2. �� ����� ������ ī��Ʈ(�Է� ���� ��)�� ���
        Dictionary<MyNode, int> dependencyCount = new Dictionary<MyNode, int>();
        foreach (MyNode node in allNodes)
        {
            // �� ����� inputPort�� ����� ���� ���� �� ����� ������ ���Դϴ�.
            dependencyCount[node] = node.inputPort.connections.Count();
        }

        // 3. �������� ���� ���(��, �Է� ������ ���� ���)�� ���� ť�� �߰�
        Queue<MyNode> readyNodes = new Queue<MyNode>();
        foreach (var kvp in dependencyCount)
        {
            if (kvp.Value == 0)
            {
                readyNodes.Enqueue(kvp.Key);
            }
        }

        // 4. ���� ���� ������� �� ������ ����ϸ� ��� ����
        while (readyNodes.Count > 0)
        {
            MyNode currentNode = readyNodes.Dequeue();

            // ���� ��� ����
            currentNode.Evaluate();

            // ���� ����� ��� ��Ʈ ������ ���� ����� ��� �ļ� ��带 ã�� ������ ī��Ʈ�� ����
            foreach (Edge edge in currentNode.outputPort.connections)
            {
                // edge.input�� ����� �Է� ��Ʈ�̹Ƿ�, �ش� ��Ʈ�� ������ ��尡 �ļ� ����Դϴ�.
                MyNode targetNode = edge.input.node as MyNode;
                if (targetNode != null)
                {
                    dependencyCount[targetNode]--;
                    // �������� 0�� �Ǹ� ���� ������ �����̹Ƿ� ť�� �߰��մϴ�.
                    if (dependencyCount[targetNode] == 0)
                    {
                        readyNodes.Enqueue(targetNode);
                    }
                }
            }
        }
    }
}
