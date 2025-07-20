using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    //��峢���� ������ å������ Ŭ���� (��ǻ� EdgeConnectorListener�� ����� �� �ؼ� �ǹ̰� ���ٰ� �ϳ� �ϴ� ������ ���ϰھ�)
    public class MyEdgeConnector : EdgeConnector
    {
        //�� �̰� port ������ ���ִ� Ŭ������鼭 null�̿��� ���� �ߵǰ� ���� ���°�
        public override EdgeDragHelper edgeDragHelper => helper;
        private EdgeDragHelper helper;

        public MyEdgeConnector(EdgeDragHelper helper)
        {
            this.helper = helper;
        }


        //Node�� AddManipulator() �� ȣ���� ��带 ������ �� ȣ��
        protected override void RegisterCallbacksOnTarget()
        {
            //Debug.Log("���� Ŭ���� registOnTarget");
        }

        //Node�� RemoveManipulator() �� ȣ���� ��带 ������ �� ȣ��
        protected override void UnregisterCallbacksFromTarget()
        {
            //Debug.Log("���� Ŭ���� unregistFromTarget");
        }
    }
}
