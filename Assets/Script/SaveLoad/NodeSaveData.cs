using UnityEngine;

namespace NodeEditor
{
    //노드 하나의 정보를 담는 클래스
    [System.Serializable]
    public class NodeSaveData
    {
        public string type;
        public string guid;
        public Vector2 position;
        public Vector2 size;
    }
}

