# 🎨 NodeEditor (Unity Custom Editor Tool)

Unity에서 커스텀 노드 기반 툴을 만들어보고 싶어서 시작한 프로젝트입니다.  
Shader Graph처럼 노드를 연결해서 기능을 구성하는 시스템을 연습하며,  
GraphView, CustomEditor 같은 Unity 내부 기능을 조금씩 익혀가는 중입니다.

---

## 💡 구현 목표

- 노드를 드래그해서 배치하고 포트를 연결할 수 있는 시스템
- 노드마다 설정값을 저장하고 불러올 수 있는 구조
- 나중엔 실제 기능(예: 스킬 트리, 상태머신 등)에도 연결해보고 싶습니다

---

## ⚙️ 주요 기능

| 기능 | 설명 |
|------|------|
| ✔️ 노드 배치 | 마우스 우클릭으로 노드를 배치할 수 있어요 |
| ✔️ 포트 연결 | 입력/출력 포트를 클릭해서 연결 가능 |
| ✔️ 커스텀 윈도우 | Unity 메뉴에서 에디터를 열 수 있음 (`Window/Node Editor`) |

---

## 🛠️ 사용 기술

- **Unity Editor / GraphView**
- **C# / Custom EditorWindow**
- UIElements, VisualElement 등 Unity UI 시스템도 같이 사용 중입니다

---

## 📚 개발하면서 느낀 점

처음엔 어떤 구조로 만들어야 할지 막막했지만,  
GPT한테 이것저것 물어보면서 개념을 하나씩 잡아가고 있어요.  
에디터 툴의 구조나 흐름 같은 게 조금씩 이해되는 것 같습니다.

---

## 🧪 앞으로 해보고 싶은 것

- 노드 간 연결 상태 시각화 개선
- 유저 설정값 저장 / 로드 기능 추가
- 기능별 서브 노드 (예: 연산 노드, 입력 노드 등) 추가
- 노드를 실제 게임 기능에 연결 (예: 조건 판단 트리, 대미지 계산 등)


---

## 🙋‍♂️ 만든 사람

**문진영 (Jinyoung Moon)**  
📧 duqorgh@naver.com  
🌐 [GitHub biueapple](https://github.com/biueapple)
