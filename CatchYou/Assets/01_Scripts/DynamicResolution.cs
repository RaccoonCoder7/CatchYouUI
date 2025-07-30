using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 해상도 대응 스크립트
/// </summary>
public class DynamicResolution : MonoBehaviour
{
    void Start()
    {
        var canvasScaler = GetComponent<CanvasScaler>();

        // 비율 계산
        float fixedAspectRatio = 9f / 16f;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        // 비율에 맞게 ratio 조정
        if (currentAspectRatio >= fixedAspectRatio)
            canvasScaler.matchWidthOrHeight = 1;
        else
            canvasScaler.matchWidthOrHeight = 0;
    }
}
