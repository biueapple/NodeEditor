using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    //�ȷ�Ʈ�� ������ ����� ���� (�ȷ�Ʈ ���� ����Ʈ�� ������� ��)
    public class PaletteItem : VisualElement
    {
        public string TypeName { get; }
        public NodeMetaData MetaData { get; }
        public readonly Label label;

        public PaletteItem(string typeName, NodeMetaData nodeMetaData)
        {
            TypeName = typeName;
            MetaData = nodeMetaData;
            label = new()
            {
                name = "paletteItem"
            };
            Add(label);
            style.paddingTop = 5;
            style.paddingBottom = 5;
            style.marginTop = 5;
            style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        }

        public void UpdateDisplay()
        {
            label.text = MetaData.displayName;
        }
    }
}
