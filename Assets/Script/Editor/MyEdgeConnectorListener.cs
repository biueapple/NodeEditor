using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyEdgeConnectorListener : IEdgeConnectorListener
{
    // ��Ʈ���� ����Ǿ��� �� ȣ��Ǵ� �޼���
    public void OnDrop(GraphView graphView, Edge edge)
    {
        // ������ input�� output�� ��� ��ȿ���� �˻��մϴ�.
        if (edge.input != null && edge.output != null)
        {
            // GraphView�� ������ �߰��մϴ�.
            graphView.AddElement(edge);
        }
        else
        {
            Debug.LogWarning("���� ����� ��ȿ���� �ʽ��ϴ�. Input �Ǵ� Output�� null�Դϴ�.");
        }
    }

    // ��ȿ���� ���� ��ġ�� ������ ������� �� ȣ��Ǵ� �޼���
    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        Debug.Log("��ȿ���� ���� ��Ʈ �ܺο� ������ ����߽��ϴ�: " + position);
    }
}