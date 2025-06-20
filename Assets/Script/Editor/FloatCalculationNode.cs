using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatCalculationNode : MyNode
{
    // 입력과 출력 데이터를 위한 필드
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
