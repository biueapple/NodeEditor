using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class VisualElementDragger : MouseManipulator
    {
        //클릭한 위치
        private Vector2 offset;
        //드래그 중인지
        private bool isDragging;

        public VisualElementDragger()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        //생성 등록인가
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        //생성 해제 등록 해제같음
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

                // 부모 영역 가져오기
                var parentSize = target.parent.resolvedStyle;

                // 팔레트 크기
                float elementWidth = target.resolvedStyle.width;
                float elementHeight = target.resolvedStyle.height;

                // 위치 제한 (Clamp)
                newX = Mathf.Clamp(newX, 0, parentSize.width - elementWidth);
                newY = Mathf.Clamp(newY, 0, parentSize.height - elementHeight);

                // 적용
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
