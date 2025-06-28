using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class VisualElementContentViewDragger : MouseManipulator
    {
        //대상이 될 윈도우 (내가 만든 윈도우임 그냥 Graph view는 필드로 grid를 가지고 있지 않아서)
        private NodeGraphView nodeGraphView;
        //클릭한 위치
        private Vector2 startMouse;
        //클릭한 graph의 위치
        private Vector3 startTransform;
        //드래그 중인지
        private bool isDragging;

        public VisualElementContentViewDragger(NodeGraphView nodeGraphView, MouseButton mouseButton)
        {
            this.nodeGraphView = nodeGraphView;
            activators.Add(new ManipulatorActivationFilter { button = mouseButton });
        }

        //생성 등록
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        //생성 해제 등록
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

            //화면 이동
            nodeGraphView.contentContainer.transform.position = startTransform + (Vector3)delta;
            //배경 이동
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

