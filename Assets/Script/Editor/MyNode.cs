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
        // 기본 포트 추가 (입력 포트와 출력 포트)
        inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        inputPort.portName = "Input";
        // Edge 연결을 위한 매니퓰레이터 추가 (IEdgeConnectorListener를 구현한 리스너 필요)
        inputPort.AddManipulator(new MyEdgeConnector(new MyEdgeConnectorListener()));
        inputContainer.Add(inputPort);

        outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        outputPort.portName = "Output";
        outputContainer.Add(outputPort);

        RefreshExpandedState();
        RefreshPorts();

        // titleContainer에서 더블클릭 이벤트 등록
        titleContainer.RegisterCallback<MouseDownEvent>(OnTitleDoubleClick);
    }

    private void OnTitleDoubleClick(MouseDownEvent evt)
    {
        // 클릭 횟수가 2 이상이면 더블클릭으로 판단
        if (evt.clickCount < 2)
            return;

        // 이미 TextField가 있다면 중복 생성 방지
        if (titleContainer.Q<TextField>() != null)
            return;

        // 기존 타이틀(라벨) 감추기: title은 그대로 유지하거나 필요에 따라 제거합니다.
        // 기존 titleContainer의 자식 요소들 제거 (간단하게 초기화)
        titleContainer.Clear();

        // TextField 생성하여 타이틀 수정 모드로 전환
        TextField renameField = new TextField();
        renameField.value = title;
        renameField.style.unityTextAlign = TextAnchor.MiddleCenter;
        // TextField 크기와 스타일은 필요한 대로 조절하세요.
        titleContainer.Add(renameField);

        // 포커스를 텍스트 필드에 주고, 전체 텍스트 선택
        renameField.Focus();
        renameField.SelectAll();

        // 입력 완료(포커스 아웃 또는 엔터)를 감지하여 새 이름 적용
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
        // 새 이름 설정
        title = renameField.value;

        // 타이틀 컨테이너 초기화 후 기본 레이블 형태로 저장
        titleContainer.Clear();

        // 기본적으로 Node 클래스는 title을 그리기 위한 내부 메커니즘을 다시 재생성하지 않으므로,
        // 직접 Label을 추가하거나, 혹은 titleContainer에 TextField가 유지되도록 할 수 있습니다.
        // 여기서는 간단히 Label로 교체하는 예제를 보여드리겠습니다.
        Label titleLabel = new Label(title);
        titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        titleContainer.Add(titleLabel);
    }

    public abstract void Evaluate();
}
