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
        private string guid;
        public string GUID { get => guid; set => guid = value; }
        private Vector2 position;
        public Vector2 Position => position;


        public MyNode()
        {
            titleContainer.RegisterCallback<MouseDownEvent>(OnTitleDoubleClick);
            guid = Guid.NewGuid().ToString();
        }

        //실행
        public abstract void Excute();

        //뭔가 당하면 (삭제 복사 복제) 호출되는듯
        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);
        }

        //우클릭하면 호출
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
        }

        //선택 해제될때 호출 근데 mouse up 해도 호출됨
        public override void OnUnselected()
        {
            base.OnUnselected();
        }

        //선택 시 가장 빨리 호출되고 두번 호출되는 일이 없음
        public override void Select(VisualElement selectionContainer, bool additive)
        {
            base.Select(selectionContainer, additive);
        }

        private void OnTitleDoubleClick(MouseDownEvent mouseDownEvent)
        {
            //더블클릭이 아님 (한번만 클릭함)
            if (mouseDownEvent.clickCount < 2)
                return;

            //이미 편집 중인지 확인
            if (titleContainer.Q<TextField>() != null)
                return;

            //기존 영역 지우기
            titleContainer.Clear();

            //새로운 이름을 정할 입력창 만들기
            TextField rename = new();
            //일단 지금 있던 이름 넣어두기
            rename.value = title;
            //중앙 정렬
            rename.style.unityTextAlign = TextAnchor.MiddleCenter;
            //입력창 넣기
            titleContainer.Add(rename);

            //입력창에 포커스
            rename.Focus();
            //입력되있는 텍스트 전부 선택
            rename.SelectAll();

            //포커스를 잃었을때 콜백
            rename.RegisterCallback<FocusOutEvent>(evtFocus => { ApplyRename(rename); });

            //enter를 누르면 콜백
            rename.RegisterCallback<KeyDownEvent>(evtKey => { if (evtKey.keyCode == KeyCode.Return || evtKey.keyCode == KeyCode.KeypadEnter) ApplyRename(rename); });
        }

        //이름 적용
        private void ApplyRename(TextField textField)
        {
            //넣을 title의 값을 입력창의 값으로 변경
            title = textField.value;

            //넣었던 입력창 없애기
            titleContainer.Clear();

            //라벨을 만들어서 title의 값을 넣기
            Label titleLabel = new Label(title);
            //중앙 정렬
            titleLabel.name = "title-label";

            //라벨 넣어서 이름 표시하기
            titleContainer.Add(titleLabel);
        }
    }
}
