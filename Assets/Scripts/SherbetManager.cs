using UnityEngine;
using System;

[Serializable]
public class DistributionPattern
{
    [Range(0f, 1f)] public float center = 0.5f;   // ピーク位置
    [Range(0f, 1f)] public float width = 0.4f;    // 有効幅 (両側)
    [Range(0.1f, 5f)] public float falloff = 2f;  // 減衰 (1=線形,2=2次…)
}

public class SherbetManager: MonoBehaviour
{
    [SerializeField] private float instanceRange = 4f;
    [SerializeField] private GameObject[] sherbetPrefabs;

    [Header("確率分布パターン")]
    [SerializeField] private DistributionPattern[] distributionPatterns = new DistributionPattern[] 
    {
        new DistributionPattern { center = 0.5f, width = 1f, falloff = 1.5f }
    };

    /// <summary>
    /// 指定されたパターンに従って center ± width の範囲で、
    /// 距離に falloff 乗を掛けてバイアスサンプリング
    /// </summary>
    private float SampleFromPattern(DistributionPattern pattern)
    {
        // -1 ～ 1 の一様乱数
        var u = UnityEngine.Random.value * 2f - 1f;

        // 距離を falloff 乗で減衰させる（0 に近いほど値が小さくなる）
        var d = Mathf.Pow(Mathf.Abs(u), pattern.falloff);

        // 符号を戻しつつ幅を掛け中心に足す
        var offset = Mathf.Sign(u) * d * pattern.width;
        var normalizedValue = Mathf.Clamp01(pattern.center + offset);  // 0～1 に収める
        
        // 0～1 の値を -instanceRange～instanceRange の範囲に変換
        return (normalizedValue * 2f - 1f) * instanceRange;
    }

    public void GenerateSherbet()
    {
        if (sherbetPrefabs == null || sherbetPrefabs.Length == 0) return;
        if (distributionPatterns == null || distributionPatterns.Length == 0) return;

        // パターンをランダムに1つ選択
        var selectedPattern = distributionPatterns[UnityEngine.Random.Range(0, distributionPatterns.Length)];

        // 選んだパターンでサンプリング
        var t = SampleFromPattern(selectedPattern);
        int index = Mathf.Clamp(
            Mathf.FloorToInt(t * sherbetPrefabs.Length),
            0, sherbetPrefabs.Length - 1);

        Instantiate(sherbetPrefabs[index], transform.position, Quaternion.identity, transform);
    }
}