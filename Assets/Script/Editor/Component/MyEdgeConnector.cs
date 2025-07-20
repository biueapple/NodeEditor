using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    //노드끼리의 연결을 책임지는 클래스 (사실상 EdgeConnectorListener가 뭐든걸 다 해서 의미가 없다고 하네 일단 삭제는 안하겠어)
    public class MyEdgeConnector : EdgeConnector
    {
        //왜 이게 port 연결을 해주는 클래스라면서 null이여도 연결 잘되고 문제 없는가
        public override EdgeDragHelper edgeDragHelper => helper;
        private EdgeDragHelper helper;

        public MyEdgeConnector(EdgeDragHelper helper)
        {
            this.helper = helper;
        }


        //Node의 AddManipulator() 를 호출해 노드를 생성할 때 호출
        protected override void RegisterCallbacksOnTarget()
        {
            //Debug.Log("연결 클래스 registOnTarget");
        }

        //Node의 RemoveManipulator() 를 호출해 노드를 제거할 때 호출
        protected override void UnregisterCallbacksFromTarget()
        {
            //Debug.Log("연결 클래스 unregistFromTarget");
        }
    }
}
