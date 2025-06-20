using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomContentDragger : MouseManipulator
{
    private Vector2 m_Start;

    private bool m_Active;

    //
    // ���:
    //     When elements are dragged near the edges of the Graph, panning occurs. This controls
    //     the speed for said panning.
    public Vector2 panSpeed { get; set; }

    //
    // ���:
    //     If true, it does not allow the dragged element to exit the parent's edges.
    public bool clampToParentEdges { get; set; }

    //
    // ���:
    //     ContentDragger constructor.
    public MyCustomContentDragger()
    {
        m_Active = false;
        base.activators.Add(new ManipulatorActivationFilter
        {
            button = MouseButton.LeftMouse,
            modifiers = EventModifiers.Alt
        });
        base.activators.Add(new ManipulatorActivationFilter
        {
            button = MouseButton.MiddleMouse
        });
        panSpeed = new Vector2(1f, 1f);
        clampToParentEdges = false;
    }

    //
    // ���:
    //     Calculate new position of the dragged element.
    //
    // �Ű� ����:
    //   x:
    //     New x position.
    //
    //   y:
    //     New y position.
    //
    //   width:
    //     Element width.
    //
    //   height:
    //     Element height.
    //
    // ��ȯ ��:
    //     Calculated and validated position.
    protected Rect CalculatePosition(float x, float y, float width, float height)
    {
        Rect result = new Rect(x, y, width, height);
        if (clampToParentEdges)
        {
            Rect rect = base.target.hierarchy.parent.layout;
            if (result.x < rect.xMin)
            {
                result.x = rect.xMin;
            }
            else if (result.xMax > rect.xMax)
            {
                result.x = rect.xMax - result.width;
            }

            if (result.y < rect.yMin)
            {
                result.y = rect.yMin;
            }
            else if (result.yMax > rect.yMax)
            {
                result.y = rect.yMax - result.height;
            }

            result.width = width;
            result.height = height;
        }

        return result;
    }

    //
    // ���:
    //     Called to register click event callbacks on the target element.
    protected override void RegisterCallbacksOnTarget()
    {
        if (!(base.target is GraphView))
        {
            throw new InvalidOperationException("Manipulator can only be added to a GraphView");
        }

        base.target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        base.target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        base.target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        base.target.RegisterCallback<MouseCaptureOutEvent>(OnCaptureOut);
    }

    //
    // ���:
    //     Called to unregister event callbacks from the target element.
    protected override void UnregisterCallbacksFromTarget()
    {
        base.target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        base.target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        base.target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        base.target.UnregisterCallback<MouseCaptureOutEvent>(OnCaptureOut);
    }

    private void OnCaptureOut(MouseCaptureOutEvent e)
    {
        FinishDrag(e);
    }

    //
    // ���:
    //     Called on mouse down event.
    //
    // �Ű� ����:
    //   e:
    //     The event.
    protected void OnMouseDown(MouseDownEvent e)
    {
        if (m_Active)
        {
            e.StopImmediatePropagation();
        }
        else if (CanStartManipulation(e) && base.target is GraphView graphView && base.target.panel?.GetCapturingElement(PointerId.mousePointerId) == null)
        {
            m_Start = graphView.ChangeCoordinatesTo(graphView.contentViewContainer, e.localMousePosition);
            m_Active = true;
            base.target.CaptureMouse();
            e.StopImmediatePropagation();
        }
    }

    //
    // ���:
    //     Called on mouse move event.
    //
    // �Ű� ����:
    //   e:
    //     The event.
    protected void OnMouseMove(MouseMoveEvent e)
    {
        if (m_Active && base.target is GraphView graphView)
        {
            if ((e.pressedButtons & 4) == 0 && ((e.pressedButtons & 1) == 0 || !e.modifiers.HasFlag(EventModifiers.Alt)))
            {
                FinishDrag(e);
                return;
            }

            Vector2 vector = graphView.ChangeCoordinatesTo(graphView.contentViewContainer, e.localMousePosition) - m_Start;
            Vector3 scale = graphView.contentViewContainer.transform.scale;
            graphView.viewTransform.position += Vector3.Scale(vector, scale);
            e.StopPropagation();
        }
    }

    //
    // ���:
    //     Called on mouse up event.
    //
    // �Ű� ����:
    //   e:
    //     The event.
    protected void OnMouseUp(MouseUpEvent e)
    {
        if (m_Active && CanStopManipulation(e))
        {
            FinishDrag(e);
        }
    }

    private void FinishDrag(EventBase e)
    {
        if (base.target is GraphView graphView)
        {
            Vector3 position = graphView.contentViewContainer.transform.position;
            Vector3 scale = graphView.contentViewContainer.transform.scale;
            graphView.UpdateViewTransform(position, scale);
            m_Active = false;
            base.target.ReleaseMouse();
            e.StopPropagation();
        }
    }
}
