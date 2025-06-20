using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class MyEdgeConnector : EdgeConnector
{
    public MyEdgeConnector(IEdgeConnectorListener listener) : base()
    {
    }

    public override EdgeDragHelper edgeDragHelper => null;

    protected override void RegisterCallbacksOnTarget()
    {

    }

    protected override void UnregisterCallbacksFromTarget()
    {

    }
}