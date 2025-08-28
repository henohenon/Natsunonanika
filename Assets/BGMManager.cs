using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static bool isLoad = false;// 自身がすでにロードされているかを判定するフラグ
    private void Awake() {
        if (isLoad) { // すでにロードされていたら
            Destroy(this.gameObject); // 自分自身を破棄して終了
            return;
        }
        isLoad = true; // ロードされていなかったら、フラグをロード済みに設定する
        DontDestroyOnLoad(this.gameObject); 
    }
}
