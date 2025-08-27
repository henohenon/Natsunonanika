using UnityEngine;

/// <summary>
/// MouseRotationControllerの使用例とデモンストレーション
/// このスクリプトをGameObjectにアタッチすると、マウスでオブジェクトを回転させることができます
/// </summary>
public class MouseRotationDemo : MonoBehaviour
{
    [Header("デモ設定")]
    [SerializeField] private bool createTestCube = true;
    [SerializeField] private Material testMaterial;
    
    private MouseRotationController rotationController;
    private GameObject testObject;
    
    private void Start()
    {
        SetupDemo();
    }
    
    private void SetupDemo()
    {
        if (createTestCube)
        {
            CreateTestCube();
        }
        
        SetupRotationController();
        DisplayInstructions();
    }
    
    private void CreateTestCube()
    {
        // テスト用のキューブを作成
        testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        testObject.name = "RotatableTestCube";
        testObject.transform.position = Vector3.zero;
        
        // マテリアルを適用（指定されている場合）
        if (testMaterial != null)
        {
            var renderer = testObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = testMaterial;
            }
        }
        
        // コライダーを削除（必要に応じて）
        var collider = testObject.GetComponent<Collider>();
        if (collider != null)
        {
            DestroyImmediate(collider);
        }
        
        Debug.Log("テスト用キューブを作成しました: " + testObject.name);
    }
    
    private void SetupRotationController()
    {
        // 回転させたいオブジェクトを決定（テストキューブまたは現在のオブジェクト）
        GameObject targetObject = testObject != null ? testObject : gameObject;
        
        // MouseRotationControllerコンポーネントを追加
        rotationController = targetObject.GetComponent<MouseRotationController>();
        if (rotationController == null)
        {
            rotationController = targetObject.AddComponent<MouseRotationController>();
        }
        
        // デフォルト設定を適用
        rotationController.SetRotationSensitivity(2f);
        rotationController.SetRotationAxis(MouseRotationController.RotationAxis.Y);
        rotationController.SetRequireMouseClick(true);
        rotationController.SetInvertRotation(false);
        
        Debug.Log("MouseRotationControllerを設定しました on: " + targetObject.name);
    }
    
    private void DisplayInstructions()
    {
        Debug.Log("=== マウス回転デモ ===");
        Debug.Log("使用方法:");
        Debug.Log("- マウスの左ボタンをクリックしながらドラッグしてオブジェクトを回転");
        Debug.Log("- 現在の設定: Y軸回転、感度2.0、クリック必須");
        Debug.Log("- Inspectorで設定を変更可能");
    }
    
    private void Update()
    {
        // デモ用の追加機能（キーボードショートカット）
        HandleDemoControls();
    }
    
    private void HandleDemoControls()
    {
        if (rotationController == null) return;
        
        // 数字キーで回転軸を変更
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rotationController.SetRotationAxis(MouseRotationController.RotationAxis.X);
            Debug.Log("回転軸をX軸に変更しました");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rotationController.SetRotationAxis(MouseRotationController.RotationAxis.Y);
            Debug.Log("回転軸をY軸に変更しました");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            rotationController.SetRotationAxis(MouseRotationController.RotationAxis.Z);
            Debug.Log("回転軸をZ軸に変更しました");
        }
        
        // Rキーで回転方向を反転
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Note: 現在の反転状態を取得するプロパティがないため、常に反転をトグル
            rotationController.SetInvertRotation(true); // 一度反転させる
            Debug.Log("回転方向を反転しました");
        }
        
        // Spaceキーでクリック要求をトグル
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Note: 現在のクリック要求状態を取得するプロパティがないため、常にトグル
            rotationController.SetRequireMouseClick(false); // クリック不要に変更
            Debug.Log("クリック要求を無効にしました（マウス移動のみで回転）");
        }
    }
    
    private void OnGUI()
    {
        // 画面上部に簡単な使用方法を表示
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== マウス回転デモ ===");
        GUILayout.Label("左クリック+ドラッグ: 回転");
        GUILayout.Label("1/2/3キー: 回転軸変更(X/Y/Z)");
        GUILayout.Label("Rキー: 回転方向反転");
        GUILayout.Label("Spaceキー: クリック要求切替");
        GUILayout.EndArea();
    }
}