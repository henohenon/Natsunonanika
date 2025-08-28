using System;
using R3;
using UnityEngine;

public class SharveController : MonoBehaviour
{
    private readonly ReactiveProperty<bool> _isActive = new();
    public Observable<bool> IsActive => _isActive;

    public void Activate(Vector3 position)
    {
        transform.position = position;
        _isActive.Value = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("End"))
        {
            _isActive.Value = false;
        }
    }
}
