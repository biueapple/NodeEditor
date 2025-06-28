using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class VisualElementDragger : MouseManipulator
    {
        //Ŭ���� ��ġ
        private Vector2 offset;
        //�巡�� ������
        private bool isDragging;

        public VisualElementDragger()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        //���� ����ΰ�
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        //���� ���� ��� ��������
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (CanStartManipulation(evt))
            {
                offset = evt.localMousePosition;
                isDragging = true;
                target.CaptureMouse();
                evt.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (isDragging && target.HasMouseCapture())
            {
                Vector2 delta = evt.localMousePosition - offset;
                float newX = target.layout.x + delta.x;
                float newY = target.layout.y + delta.y;

                // �θ� ���� ��������
                var parentSize = target.parent.resolvedStyle;

                // �ȷ�Ʈ ũ��
                float elementWidth = target.resolvedStyle.width;
                float elementHeight = target.resolvedStyle.height;

                // ��ġ ���� (Clamp)
                newX = Mathf.Clamp(newX, 0, parentSize.width - elementWidth);
                newY = Mathf.Clamp(newY, 0, parentSize.height - elementHeight);

                // ����
                target.style.left = newX;
                target.style.top = newY;

                evt.StopPropagation();
            }
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (isDragging && CanStopManipulation(evt))
            {
                isDragging = false;
                target.ReleaseMouse();
                evt.StopPropagation();
            }
        }
    }
}
