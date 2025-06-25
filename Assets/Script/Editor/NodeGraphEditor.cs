using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class NodeGraphEditor : EditorWindow
    {
        private NodeGraphView nodeGraphView;

        //������ â�� �����ϴ� �ڵ�
        [MenuItem("Window/Node Graph Editor")]
        public static void OpenGraphEditor()
        {
            var window = GetWindow<NodeGraphEditor>();
            window.titleContent = new GUIContent("Node Graph Editor");
        }

        //������ â�� ���������� ȣ��
        public void CreateGUI()
        {
            ConstructGraphView();
        }

        //�⺻���� ��� ����� �߰�
        private void ConstructGraphView()
        {
            //view ���� ����� �̸��� ������ �̸��� �����ڰ� �ҷ����ų� �񱳷� �� ��Ÿ�������� �� ����ڿ��� ������ ��ġ�ų� ������ ����
            nodeGraphView = new NodeGraphView()
            {
                name = "nodeGraphView"
            };

            //������ �󸶳� �������� 1�̸� ���� ��� ������ ����
            nodeGraphView.style.flexGrow = 1;

            //uiƮ�� �ֻ�� ���⿡ ��ϵ� ������ ������ â�� �׷���
            rootVisualElement.Add(nodeGraphView);
        }
    }
}

