using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRotationController : MonoBehaviour
{
    [Header("回転設定")]
    [SerializeField] private float rotationSensitivity = 2f;
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Y;
    [SerializeField] private bool invertRotation = false;
    
    [Header("入力設定")]
    [SerializeField] private bool requireMouseClick = true;
    
    private bool isRotating = false;
    private Vector2 lastMousePosition;
    private Vector2 mouseDelta;
    
    // Input System用のアクション
    private InputAction mousePositionAction;
    private InputAction mouseClickAction;
    
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }
    
    private void Awake()
    {
        // Input Systemのアクションを設定
        mousePositionAction = new InputAction("MousePosition", InputActionType.Value, "<Mouse>/position");
        mouseClickAction = new InputAction("MouseClick", InputActionType.Button, "<Mouse>/leftButton");
        
        // アクションを有効化
        mousePositionAction.Enable();
        mouseClickAction.Enable();
        
        // マウスクリックのイベントを設定
        mouseClickAction.started += OnMouseDown;
        mouseClickAction.canceled += OnMouseUp;
    }
    
    private void OnDestroy()
    {
        // アクションを無効化してメモリリークを防ぐ
        mousePositionAction?.Disable();
        mouseClickAction?.Disable();
        
        // イベントの購読を解除
        if (mouseClickAction != null)
        {
            mouseClickAction.started -= OnMouseDown;
            mouseClickAction.canceled -= OnMouseUp;
        }
    }
    
    private void Update()
    {
        // マウスクリックが必要な場合、クリック中のみ回転
        if (requireMouseClick && !isRotating)
            return;
            
        // マウスの位置とデルタを取得
        Vector2 currentMousePosition = mousePositionAction.ReadValue<Vector2>();
        mouseDelta = currentMousePosition - lastMousePosition;
        lastMousePosition = currentMousePosition;
        
        // 初回フレームではデルタが大きくなるのを防ぐ
        if (Time.frameCount < 2)
        {
            mouseDelta = Vector2.zero;
        }
        
        // 回転を適用
        ApplyRotation(mouseDelta);
    }
    
    private void OnMouseDown(InputAction.CallbackContext context)
    {
        isRotating = true;
        lastMousePosition = mousePositionAction.ReadValue<Vector2>();
    }
    
    private void OnMouseUp(InputAction.CallbackContext context)
    {
        isRotating = false;
    }
    
    private void ApplyRotation(Vector2 delta)
    {
        if (delta.magnitude < 0.1f)
            return;
            
        // マウスのX軸の動きを回転に使用（より直感的）
        float rotationAmount = delta.x * rotationSensitivity * Time.deltaTime;
        
        if (invertRotation)
            rotationAmount = -rotationAmount;
        
        // 指定された軸で回転
        Vector3 rotationVector = Vector3.zero;
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = Vector3.right * rotationAmount;
                break;
            case RotationAxis.Y:
                rotationVector = Vector3.up * rotationAmount;
                break;
            case RotationAxis.Z:
                rotationVector = Vector3.forward * rotationAmount;
                break;
        }
        
        // 回転を適用
        transform.Rotate(rotationVector, Space.Self);
    }
    
    /// <summary>
    /// 回転の感度を設定
    /// </summary>
    public void SetRotationSensitivity(float sensitivity)
    {
        rotationSensitivity = sensitivity;
    }
    
    /// <summary>
    /// 回転軸を設定
    /// </summary>
    public void SetRotationAxis(RotationAxis axis)
    {
        rotationAxis = axis;
    }
    
    /// <summary>
    /// マウスクリック要求の設定
    /// </summary>
    public void SetRequireMouseClick(bool require)
    {
        requireMouseClick = require;
    }
    
    /// <summary>
    /// 回転方向を反転する設定
    /// </summary>
    public void SetInvertRotation(bool invert)
    {
        invertRotation = invert;
    }
}