using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class MyNode : Node
{
    public Port inputPort;
    public Port outputPort;

    public MyNode()
    {
        title = "My Node";
        // �⺻ ��Ʈ �߰� (�Է� ��Ʈ�� ��� ��Ʈ)
        inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        inputPort.portName = "Input";
        // Edge ������ ���� �Ŵ�ǽ������ �߰� (IEdgeConnectorListener�� ������ ������ �ʿ�)
        inputPort.AddManipulator(new MyEdgeConnector(new MyEdgeConnectorListener()));
        inputContainer.Add(inputPort);

        outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        outputPort.portName = "Output";
        outputContainer.Add(outputPort);

        RefreshExpandedState();
        RefreshPorts();

        // titleContainer���� ����Ŭ�� �̺�Ʈ ���
        titleContainer.RegisterCallback<MouseDownEvent>(OnTitleDoubleClick);
    }

    private void OnTitleDoubleClick(MouseDownEvent evt)
    {
        // Ŭ�� Ƚ���� 2 �̻��̸� ����Ŭ������ �Ǵ�
        if (evt.clickCount < 2)
            return;

        // �̹� TextField�� �ִٸ� �ߺ� ���� ����
        if (titleContainer.Q<TextField>() != null)
            return;

        // ���� Ÿ��Ʋ(��) ���߱�: title�� �״�� �����ϰų� �ʿ信 ���� �����մϴ�.
        // ���� titleContainer�� �ڽ� ��ҵ� ���� (�����ϰ� �ʱ�ȭ)
        titleContainer.Clear();

        // TextField �����Ͽ� Ÿ��Ʋ ���� ���� ��ȯ
        TextField renameField = new TextField();
        renameField.value = title;
        renameField.style.unityTextAlign = TextAnchor.MiddleCenter;
        // TextField ũ��� ��Ÿ���� �ʿ��� ��� �����ϼ���.
        titleContainer.Add(renameField);

        // ��Ŀ���� �ؽ�Ʈ �ʵ忡 �ְ�, ��ü �ؽ�Ʈ ����
        renameField.Focus();
        renameField.SelectAll();

        // �Է� �Ϸ�(��Ŀ�� �ƿ� �Ǵ� ����)�� �����Ͽ� �� �̸� ����
        renameField.RegisterCallback<FocusOutEvent>(evtFocusOut =>
        {
            ApplyRename(renameField);
        });

        renameField.RegisterCallback<KeyDownEvent>(evtKey =>
        {
            if (evtKey.keyCode == KeyCode.Return || evtKey.keyCode == KeyCode.KeypadEnter)
            {
                ApplyRename(renameField);
            }
        });
    }

    private void ApplyRename(TextField renameField)
    {
        // �� �̸� ����
        title = renameField.value;

        // Ÿ��Ʋ �����̳� �ʱ�ȭ �� �⺻ ���̺� ���·� ����
        titleContainer.Clear();

        // �⺻������ Node Ŭ������ title�� �׸��� ���� ���� ��Ŀ������ �ٽ� ��������� �����Ƿ�,
        // ���� Label�� �߰��ϰų�, Ȥ�� titleContainer�� TextField�� �����ǵ��� �� �� �ֽ��ϴ�.
        // ���⼭�� ������ Label�� ��ü�ϴ� ������ �����帮�ڽ��ϴ�.
        Label titleLabel = new Label(title);
        titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        titleContainer.Add(titleLabel);
    }

    public abstract void Evaluate();
}
