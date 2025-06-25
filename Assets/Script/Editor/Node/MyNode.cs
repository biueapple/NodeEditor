using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace NodeEditor
{
    //����� �⺻�� �Ǵ� ���� �ϴ� �����ɸ� ��������
    public abstract class MyNode : UnityEditor.Experimental.GraphView.Node
    {
        //����
        public abstract void Excute();

        //���� ���ϸ� (���� ���� ����) ȣ��Ǵµ�
        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);
            Debug.Log("CollectElements");
        }

        //��Ŭ���ϸ� ȣ��Ǵµ� �ٵ� ��Ŭ���ص� ����� ��Ŭ���Ѱŷ� ���ͼ�
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //�⺻ ��Ŭ�� �޴� ������ �ʰ�
            //base.BuildContextualMenu(evt);
            //��� ����
            evt.menu.AppendAction("Delete", action => DeleteNode());
            //���̻� �̺�Ʈ�� ������ ���ĵ��� �ʵ��� �̰� ���ٸ� graphview�� ��Ŭ���Ŵ��� ���� ����
            evt.StopPropagation();
        }

        private void DeleteNode()
        {
            //�ڽ��� �θ��߿� GraphView�� ã�� (���� �����)
            if (GetFirstAncestorOfType<GraphView>() is GraphView graphView)
            {
                //�θ𿡰� �ڽ��� �����ش޶�� ��û
                graphView.RemoveElement(this);
            }
        }

        //���� �� 2�� ȣ��Ǵ� ��� ����
        public override void OnSelected()
        {
            base.OnSelected();
            Debug.Log("OnSelected");
        }

        //���� �� ���� ���� ȣ��ǰ� �ι� ȣ��Ǵ� ���� ����
        public override void Select(VisualElement selectionContainer, bool additive)
        {
            base.Select(selectionContainer, additive);
            Debug.Log("Select");
        }

    }
}
