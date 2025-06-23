using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatCalculationNode : MyNode
{
    // �Է°� ��� �����͸� ���� �ʵ�
    public float inputValue = 0f;
    public float outputValue = 0f;

    public FloatCalculationNode() : base()
    {
        title = "Float Calculation Node";
    }


    public override void Evaluate()
    {
        outputValue = inputValue * 2.0f;
        Debug.Log($"[FloatCalculationNode] Evaluate: Input = {inputValue}, Output = {outputValue}");
    }
}
