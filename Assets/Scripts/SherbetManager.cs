using UnityEngine;
using System;
using Alchemy.Inspector;

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


    private float GetRandomPosition(DistributionPattern pattern)
    {
        // ループ処理による軽量実装
        float halfWidth = pattern.width / 2;
        float u, t, probability;

        // 最大20回までの試行で適切な値を生成
        for (int i = 0; i < 20; i++)
        {
            // 中心付近の値を生成（-0.5～0.5の範囲）
            u = UnityEngine.Random.value - 0.5f;

            // 中心からの絶対距離
            float absoluteU = Mathf.Abs(u);

            // 範囲外ならスキップ
            if (absoluteU > halfWidth) continue;

            // 中心からの距離に基づく確率
            probability = Mathf.Pow(1 - (absoluteU / halfWidth), pattern.falloff);

            // 確率に基づいて受理
            if (UnityEngine.Random.value <= probability)
            {
                // 中心位置を考慮した最終位置
                t = pattern.center + u;
                // instanceRangeを考慮してスケーリング
                return (t * 2 - 1) * instanceRange;
            }
        }

        // 20回試行しても見つからなかった場合は中心位置を返す
        return (pattern.center * 2 - 1) * instanceRange;
    }

    [Button]
    public void GenerateSherbet()
    {
        if (sherbetPrefabs == null || sherbetPrefabs.Length == 0) return;
        if (distributionPatterns == null || distributionPatterns.Length == 0) return;

        // パターンをランダムに1つ選択
        var selectedPattern = distributionPatterns[UnityEngine.Random.Range(0, distributionPatterns.Length)];

        var t = GetRandomPosition(selectedPattern);

        // インデックスを完全にランダムに選択
        int randomIndex = UnityEngine.Random.Range(0, sherbetPrefabs.Length);

        // 完全にランダムな回転を生成
        Quaternion randomRotation = Quaternion.Euler(
            UnityEngine.Random.Range(0f, 360f),
            UnityEngine.Random.Range(0f, 360f),
            UnityEngine.Random.Range(0f, 360f)
        );

        Instantiate(sherbetPrefabs[randomIndex], transform.position + new Vector3(t, 0, 0), randomRotation, transform);
    }
}