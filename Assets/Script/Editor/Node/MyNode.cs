using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace NodeEditor
{
    //노드의 기본이 되는 노드로 일단 실행기능만 남아있음
    public abstract class MyNode : UnityEditor.Experimental.GraphView.Node
    {
        //실행
        public abstract void Excute();

        //뭔가 당하면 (삭제 복사 복제) 호출되는듯
        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);
            Debug.Log("CollectElements");
        }

        //우클릭하면 호출되는듯 근데 우클릭해도 배경을 우클릭한거로 나와서
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //기본 우클릭 메뉴 보이지 않게
            //base.BuildContextualMenu(evt);
            //노드 삭제
            evt.menu.AppendAction("Delete", action => DeleteNode());
            //더이상 이벤트가 상위로 전파되지 않도록 이게 없다면 graphview의 우클릭매뉴도 같이 보임
            evt.StopPropagation();
        }

        private void DeleteNode()
        {
            //자신의 부모중에 GraphView를 찾음 (가장 가까운)
            if (GetFirstAncestorOfType<GraphView>() is GraphView graphView)
            {
                //부모에게 자신을 삭제해달라고 요청
                graphView.RemoveElement(this);
            }
        }

        //선택 후 2번 호출되는 경우 있음
        public override void OnSelected()
        {
            base.OnSelected();
            Debug.Log("OnSelected");
        }

        //선택 시 가장 빨리 호출되고 두번 호출되는 일이 없음
        public override void Select(VisualElement selectionContainer, bool additive)
        {
            base.Select(selectionContainer, additive);
            Debug.Log("Select");
        }

    }
}
