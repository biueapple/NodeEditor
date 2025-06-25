using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    //��峢���� ������ å������ Ŭ����
    public class MyEdgeConnector : EdgeConnector
    {
        //�� �̰� port ������ ���ִ� Ŭ������鼭 null�̿��� ���� �ߵǰ� ���� ���°�
        public override EdgeDragHelper edgeDragHelper => null;

        public MyEdgeConnector()
        {

        }


        //Node�� AddManipulator() �� ȣ���� ��带 ������ �� ȣ��
        protected override void RegisterCallbacksOnTarget()
        {
            Debug.Log("���� Ŭ���� registOnTarget");
        }

        //Node�� RemoveManipulator() �� ȣ���� ��带 ������ �� ȣ��
        protected override void UnregisterCallbacksFromTarget()
        {
            Debug.Log("���� Ŭ���� unregistFromTarget");
        }
    }
}
