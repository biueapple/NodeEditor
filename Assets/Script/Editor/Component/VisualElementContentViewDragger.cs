using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class VisualElementContentViewDragger : MouseManipulator
    {
        //����� �� ������ (���� ���� �������� �׳� Graph view�� �ʵ�� grid�� ������ ���� �ʾƼ�)
        private NodeGraphView nodeGraphView;
        //Ŭ���� ��ġ
        private Vector2 startMouse;
        //Ŭ���� graph�� ��ġ
        private Vector3 startTransform;
        //�巡�� ������
        private bool isDragging;

        public VisualElementContentViewDragger(NodeGraphView nodeGraphView, MouseButton mouseButton)
        {
            this.nodeGraphView = nodeGraphView;
            activators.Add(new ManipulatorActivationFilter { button = mouseButton });
        }

        //���� ���
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        //���� ���� ���
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (!CanStartManipulation(evt)) return;

            isDragging = true;
            startMouse = evt.localMousePosition;
            startTransform = target.contentContainer.transform.position;

            target.CaptureMouse();
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (!isDragging || !target.HasMouseCapture()) return;

            Vector2 delta = evt.localMousePosition - startMouse;

            //ȭ�� �̵�
            nodeGraphView.contentContainer.transform.position = startTransform + (Vector3)delta;
            //��� �̵�
            nodeGraphView.GridBackground.transform.position = -nodeGraphView.contentContainer.transform.position;

            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (!isDragging || !CanStopManipulation(evt)) return;

            isDragging = false;
            target.ReleaseMouse();
            evt.StopPropagation();
        }
    }
}

