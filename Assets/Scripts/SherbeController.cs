using System;
using R3;
using UnityEngine;

public class SherbeController : MonoBehaviour
{
    private readonly ReactiveProperty<bool> _isActive = new(false);
    public ReadOnlyReactiveProperty<bool> IsActive => _isActive;

    public void Activate(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(
            UnityEngine.Random.Range(0f, 360f),
            UnityEngine.Random.Range(0f, 360f),
            UnityEngine.Random.Range(0f, 360f)
        );
        
        _isActive.Value = true;
        gameObject.SetActive(true);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("End")) Disable();
    }

    public void Disable()
    {
        _isActive.Value = false;
        gameObject.SetActive(false);
    }
}
