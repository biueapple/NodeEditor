using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    public class MyEdgeConnectorListener : IEdgeConnectorListener
    {
        public void OnDrop(GraphView graphView, Edge edge)
        {
            Debug.Log("OnDrop");
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            Debug.Log("OnDropOutsidePort");
        }
    }
}
