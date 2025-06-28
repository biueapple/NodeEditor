using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphPaletteView : GraphView
    {
        public NodeGraphPaletteView()
        {
            //SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
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

            this.AddManipulator(new VisualElementDragger());
        }
    }
}
