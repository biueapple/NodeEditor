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
            label = new();
            Add(label);
            style.paddingBottom = 2;
            style.paddingTop = 10;
            style.marginTop = 11;
            style.paddingLeft = 4;
            style.paddingRight = 4;
            style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        }

        public void UpdateDisplay()
        {
            label.text = MetaData.displayName;
        }
    }
}
