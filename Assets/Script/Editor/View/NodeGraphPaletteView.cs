using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphPaletteView : GraphView
    {
        private readonly NodeGraphView nodeGraphView;
        private readonly NodeInspectorView inspectorView;
        private readonly Button plusButton;
        private readonly ScrollView listView;
        private readonly Dictionary<PaletteItem, MyNode> itemToNode = new();
        private PaletteItem selectItem;

        public NodeGraphPaletteView(NodeGraphView nodeGraphView, NodeInspectorView inspectorView)
        {
            this.nodeGraphView = nodeGraphView;
            this.inspectorView = inspectorView;

            style.position = Position.Absolute;
            style.left = 20;
            style.top = 20;
            style.width = 250;
            style.height = 400;
            style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            style.borderTopLeftRadius = 4;
            style.borderTopRightRadius = 4;
            style.borderBottomLeftRadius = 4;
            style.borderBottomRightRadius = 4;
            style.paddingLeft = 10;
            style.paddingTop = 10;

            VisualElement topElement = new ();
            topElement.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            //자신의 크기를 자동으로 설정해주는거 (높이는 수동으로 정하고 좌우는 자동으로 맞춰주기)
            topElement.style.alignSelf = Align.Stretch;
            topElement.style.height = 50;
            //왼쪽 과 위 패딩 없애기
            topElement.style.marginLeft = -10;
            topElement.style.marginTop = -10;

            //라벨 추가
            Label label = new ("Node Graph");
            //텍스트 정렬
            label.style.unityTextAlign = TextAnchor.UpperLeft;
            label.style.marginTop = 10;
            label.style.marginLeft = 10;

            //+ 버튼 생성
            plusButton = new Button(() => ShowMenu()) { text = "+" };
            plusButton.style.alignSelf = Align.FlexEnd;
            plusButton.style.width = 20;
            plusButton.style.height = 20;

            topElement.Add(label);
            topElement.Add(plusButton);

            Add(topElement);


            listView = new ();
            listView.style.flexGrow = 1;
            Add(listView);

            //마우스 클릭으로 드래그하여 움직이도록 하는 기능 추가
            this.AddManipulator(new VisualElementDragger(MouseButton.LeftMouse));
        }

        private void ShowMenu()
        {
            GenericMenu genericMenu = new ();
            foreach(var (type, metaData) in NodeFactory.NodeConstructor)
            {
                if(metaData.isVisiblePalette)
                {
                    genericMenu.AddItem(new GUIContent(metaData.displayName), false, () => AddParam(type.Name));
                }
            }
            genericMenu.ShowAsContext();
        }

        //일단 테스트 용도로 floatIONode를 화면 중앙에 생성
        private void AddParam(string typename)
        {
            Vector2 viewCenter = nodeGraphView.layout.size / 2;
            //노드 크기를 계산해서 더 중앙으로
            //Vector2 nodeSize = new Vector2(200, 150);
            //Vector2 nodePosition = viewCenter - nodeSize / 2;

            MyNode node = nodeGraphView.CreateNode(typename, viewCenter);
            if (node == null)
                return;

            if(NodeFactory.NameToType.TryGetValue(typename, out var type) && NodeFactory.NodeConstructor.TryGetValue(type, out var meta))
            {
                PaletteItem item = new(typename, meta);
                item.UpdateDisplay();
                item.RegisterCallback<MouseDownEvent>(_ => ShowDetail(item));
                listView.Add(item);
                itemToNode[item] = node;
                inspectorView.Show(item, () => RemoveItem(item));
            }
        }

        private void ShowDetail(PaletteItem item)
        {
            selectItem = item;
            TextField nameField = new("Name") { value = item.MetaData.displayName };
            nameField.RegisterValueChangedCallback(evt =>
            {
                item.MetaData.displayName = evt.newValue;
                item.UpdateDisplay();
            });

            inspectorView.Show(item, () => RemoveItem(item));
        }

        private void RemoveItem(PaletteItem item)
        {
            if(itemToNode.TryGetValue(item, out MyNode node))
            {
                nodeGraphView.RemoveElement(node);
                itemToNode.Remove(item);
            }
            listView.Remove(item);
            inspectorView.Show(null, null);
        }
    }
}
