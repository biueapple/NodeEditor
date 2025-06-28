using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //VisualElement 자체를 움직일때 사용하는 클래스
    public class VisualElementDragger : MouseManipulator
    {
        //클릭한 위치
        private Vector2 offset;
        //드래그 중인지
        private bool isDragging;

        public VisualElementDragger(MouseButton mouseButton)
        {
            activators.Add(new ManipulatorActivationFilter { button = mouseButton });
        }

        //생성 등록
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        //등록 해제
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
