using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyEdgeConnectorListener : IEdgeConnectorListener
{
    // 포트끼리 연결되었을 때 호출되는 메서드
    public void OnDrop(GraphView graphView, Edge edge)
    {
        // 에지의 input과 output이 모두 유효한지 검사합니다.
        if (edge.input != null && edge.output != null)
        {
            // GraphView에 에지를 추가합니다.
            graphView.AddElement(edge);
        }
        else
        {
            Debug.LogWarning("에지 드롭이 유효하지 않습니다. Input 또는 Output이 null입니다.");
        }
    }

    // 유효하지 않은 위치에 에지를 드롭했을 때 호출되는 메서드
    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        Debug.Log("유효하지 않은 포트 외부에 에지를 드롭했습니다: " + position);
    }
}