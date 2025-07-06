using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace NodeEditor
{
    public class NodeGraphAssetOpener
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);

            if(asset is NodeGraphData graphData)
            {
                NodeGraphEditor.OpenGraphEditor(graphData);
                return true;
            }
            return false;
        }
    }
}

