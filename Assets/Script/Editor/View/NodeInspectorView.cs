using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //팔레트로 추가한 노드의 상세정보
    public class NodeInspectorView : VisualElement
    {
        public NodeInspectorView()
        {
            style.position = Position.Absolute;
            style.right = 20;
            style.top = 20;
            style.width = 250;
            style.height = 200;
            style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            style.paddingLeft = 10;
            style.paddingTop = 10;
        }

        public void Show(PaletteItem item, System.Action onDelete)
        {
            Clear();
            if (item == null)
                return;

            //이름 필드 아이템의 이름을 변경 가능
            TextField nameField = new("Name") { value = item.MetaData.displayName };
            nameField.RegisterValueChangedCallback(evt =>
            {
                item.MetaData.displayName = evt.newValue;
                item.UpdateDisplay();
            });
            Add(nameField);

            //삭제 버튼 만들기
            if (onDelete != null)
            {
                Button deleteButton = new (() => onDelete()) { text = "Delete" };
                deleteButton.style.marginTop = 4;
                Add(deleteButton);
            }
        }
    }
}

