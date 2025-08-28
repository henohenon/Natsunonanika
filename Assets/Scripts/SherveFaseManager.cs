using System;
using R3;
using UnityEngine;

public class SherveFaseManager : MonoBehaviour
{
    [SerializeField] private AudioSource _sherveSound;

    [SerializeField] private MouseAngleCheck mouseAngleCheck;
    [SerializeField] private SherbetManager sherbetManager;
    [SerializeField] private Transform handleObj; 
    [SerializeField] private MachineController machineController;
    [SerializeField] private float maxIce = 100;
    
    private float _currentIce;
    private IDisposable _angleSubscription;
    private float _accumulatedAngle = 0f;
    private Vector3 _initRotation;
    
    private readonly ReactiveProperty<bool> _shavedFirst = new ();
    public ReadOnlyReactiveProperty<bool> ShavedFirst => _shavedFirst;


    private void Start()
    {
        // MouseAngleCheckとSherbetManagerの参照を確認
        if (mouseAngleCheck == null)
        {
            Debug.LogError("MouseAngleCheck not found! Please assign it in the inspector or ensure it exists in the scene.");
            return;
        }
        
        if (sherbetManager == null)
        {
            Debug.LogError("SherbetManager not found! Please assign it in the inspector or ensure it exists in the scene.");
            return;
        }
        
        _initRotation = handleObj.localEulerAngles;
        
        // MouseAngleCheckのAngleOffsetを購読
        _angleSubscription = mouseAngleCheck.AngleOffset.Subscribe(OnAngleChanged);

        ReStart();
    }
    public void ReStart()
    {
        _currentIce  = maxIce;
        _shavedFirst.Value = true;
        machineController.rate = 1;
        handleObj.eulerAngles = _initRotation;
        sherbetManager.ReStart();
    }
    
    private void OnAngleChanged(float angleChange)
    {
        if (!enabled) return;
        
        handleObj.Rotate(Vector3.right, angleChange);
        // angleChangeの値を累積
        _accumulatedAngle += angleChange;
        
        // 累積された角度が1.0以上の場合、その整数部分だけGenerateSherbetを実行
        int sherbetCount = Mathf.FloorToInt(_accumulatedAngle);
        
        if (sherbetCount > 0)
        {
            _sherveSound.Play();
            // sherbetCount回GenerateSherbetを実行
            for (int i = 0; i < sherbetCount; i++)
            {
                sherbetManager.GenerateSherbet();
            }
            
            // 実行した分を累積値から減算（小数部分は保持）
            _accumulatedAngle -= sherbetCount;
            _currentIce -= sherbetCount;

            machineController.rate = _currentIce / maxIce;

            _shavedFirst.Value = maxIce - _currentIce < 10;
                
            Debug.Log($"Generated {sherbetCount} sherbet(s). Remaining accumulated angle: {_accumulatedAngle}");
        }
    }
    
    private void OnDestroy()
    {
        // 購読を解除してメモリリークを防ぐ
        _angleSubscription?.Dispose();
    }

    public int GetScore()
    {
        return sherbetManager.GetCurrentActiveSherbetCount();
    }
}