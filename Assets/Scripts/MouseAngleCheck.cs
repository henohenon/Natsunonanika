using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAngleCheck : MonoBehaviour
{
    [SerializeField] private float minimumRadius = 50f;

    private Vector2 previousMousePosition;
    private Vector2 screenCenter;
    private bool isClicking = false;
    
    private Subject<float> _angleOffset = new ();
    public Observable<float> AngleOffset => _angleOffset;
    
    void Start()
    {
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        previousMousePosition = screenCenter;
    }

    void Update()
    {
        // マウス入力の取得
        Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        bool mousePressed = Mouse.current.leftButton.isPressed;
        
        // クリック状態の管理
        if (mousePressed && !isClicking)
        {
            isClicking = true;
            previousMousePosition = currentMousePosition;
        }
        else if (!mousePressed)
        {
            isClicking = false;
        }
        
        // クリック中の判定
        if (isClicking)
        {
            CheckAngle(currentMousePosition);
            previousMousePosition = currentMousePosition;
        }
    }
    
    void CheckAngle(Vector2 currentMousePosition)
    {
        // 最低半径チェック
        float distanceFromCenter = Vector2.Distance(currentMousePosition, screenCenter);
        if (distanceFromCenter < minimumRadius)
        {
            return; // 最低半径未満の場合は判定しない
        }

        // 画面中心から前フレームのマウス位置へのベクトル
        Vector2 prevVector = (previousMousePosition - screenCenter).normalized;
        // 画面中心から現在のマウス位置へのベクトル
        Vector2 currentVector = (currentMousePosition - screenCenter).normalized;
        
        // 外積を使用して回転方向を判定
        // 2Dでの外積: v1.x * v2.y - v1.y * v2.x
        float crossProduct = prevVector.x * currentVector.y - prevVector.y * currentVector.x;
        
        // 時計回りの場合の計算ロジック
        if (crossProduct < 0) // 時計回り
        {
            // 基本値1から開始
            float angleOffsetValue = 1.0f;
            
            // 移動速度の計算（前フレームからの距離）
            float movementDistance = Vector2.Distance(currentMousePosition, previousMousePosition);
            float movementSpeed = Mathf.Clamp01(movementDistance / 100.0f); // 正規化（0-1）
            float speedAdjustment = (movementSpeed - 0.5f) * 0.5f; // ±0.5の範囲で調整
            
            // 時計回りへの忠実度（crossProductの絶対値を使用）
            float clockwiseFidelity = Mathf.Clamp01(Mathf.Abs(crossProduct) * 2.0f); // 正規化（0-1）
            float fidelityAdjustment = (clockwiseFidelity - 0.5f) * 0.5f; // ±0.5の範囲で調整
            
            // 最終値の計算
            angleOffsetValue += speedAdjustment + fidelityAdjustment;
            
            _angleOffset.OnNext(angleOffsetValue);
            Debug.Log("OK - 時計回り");
        }
        else if (crossProduct > 0)
        {
            // 反時計回りの場合は従来通り
            _angleOffset.OnNext(crossProduct);
            Debug.Log("NG - 反時計回り");
        }
        // crossProduct == 0の場合は動きがないか直線的な動きなので判定しない
    }
    
    private void OnDestroy()
    {
        _angleOffset.Dispose();
    }
}