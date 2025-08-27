# マウス回転システム (Mouse Rotation System)

Unity Input Systemを使用してマウスのクリック+ドラッグでオブジェクトを1軸回転させるシステムです。

## ファイル構成

- `MouseRotationController.cs` - メインの回転制御コンポーネント
- `MouseRotationDemo.cs` - 使用例とデモンストレーション
- `README_MouseRotation.md` - このドキュメント

## 必要な要件

- Unity 2022.3 以降
- Input System package (1.14.0 以降) - 既にプロジェクトに含まれています

## 基本的な使用方法

### 1. 簡単なセットアップ (デモ使用)

1. 空のGameObjectを作成
2. `MouseRotationDemo`スクリプトをアタッチ
3. プレイモードに入る
4. マウス左クリック+ドラッグでテストキューブが回転

### 2. 既存オブジェクトに追加

1. 回転させたいGameObjectを選択
2. `MouseRotationController`コンポーネントを追加
3. Inspector で設定を調整

## MouseRotationController 設定項目

### 回転設定 (Rotation Settings)
- **Rotation Sensitivity** (回転感度): マウス移動に対する回転の感度 (デフォルト: 2.0)
- **Rotation Axis** (回転軸): 回転する軸を選択 (X, Y, Z)
- **Invert Rotation** (回転反転): 回転方向を反転するかどうか

### 入力設定 (Input Settings)
- **Require Mouse Click** (マウスクリック要求): クリック中のみ回転するかどうか

## コードでの制御

```csharp
// MouseRotationControllerの取得
MouseRotationController rotationController = GetComponent<MouseRotationController>();

// 感度を設定
rotationController.SetRotationSensitivity(3.0f);

// 回転軸を変更
rotationController.SetRotationAxis(MouseRotationController.RotationAxis.Y);

// クリック要求を無効化（マウス移動のみで回転）
rotationController.SetRequireMouseClick(false);

// 回転方向を反転
rotationController.SetInvertRotation(true);
```

## デモの使用方法

### マウス操作
- **左クリック+ドラッグ**: オブジェクト回転

### キーボードショートカット
- **1キー**: X軸回転に変更
- **2キー**: Y軸回転に変更
- **3キー**: Z軸回転に変更
- **Rキー**: 回転方向を反転
- **Spaceキー**: クリック要求を切り替え

## 技術的な詳細

### Input System の実装
- `<Mouse>/position` で マウス位置を取得
- `<Mouse>/leftButton` で クリック状態を監視
- デルタ値の計算で滑らかな回転を実現

### 回転の計算
- マウスのX軸移動を回転量に変換
- `Time.deltaTime` を使用してフレームレート非依存
- 指定軸での `transform.Rotate()` を使用

### メモリ管理
- Input Action の適切な有効化/無効化
- イベントハンドラーの購読解除でメモリリークを防止

## カスタマイズ例

### より複雑な回転パターン
```csharp
// Y軸とZ軸の同時回転
private void ApplyCustomRotation(Vector2 delta)
{
    float yRotation = delta.x * rotationSensitivity * Time.deltaTime;
    float zRotation = delta.y * rotationSensitivity * Time.deltaTime;
    
    transform.Rotate(0, yRotation, zRotation, Space.Self);
}
```

### 回転の制限
```csharp
// 回転角度を制限
private void ClampRotation()
{
    Vector3 eulerAngles = transform.eulerAngles;
    eulerAngles.y = Mathf.Clamp(eulerAngles.y, -90f, 90f);
    transform.eulerAngles = eulerAngles;
}
```

## トラブルシューティング

### 回転が機能しない場合
1. Input System パッケージがインストールされているか確認
2. Project Settings で Input System が有効になっているか確認
3. コンソールでエラーメッセージを確認

### 回転が滑らかでない場合
1. `rotationSensitivity` の値を調整
2. フレームレートが安定しているか確認
3. `Time.deltaTime` が正しく適用されているか確認

### マウスクリックが検出されない場合
1. 他のコンポーネントがマウス入力を妨害していないか確認
2. UI要素がマウス入力をブロックしていないか確認

## ライセンス

このコードは自由に使用・改変していただけます。

---
作成日: 2025-08-28
Unity Version: 2022.3+
Input System Version: 1.14.0+