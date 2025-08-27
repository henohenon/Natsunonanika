using UnityEngine;

public class MachineController : MonoBehaviour
{
    [SerializeField]
    private Transform ice;
    [SerializeField]
    private Transform screw;
    
    private float _rate = 1f;
    [Range(0, 1)]
    public float rate 
    { 
        get => _rate;
        set
        {
            _rate = value;
            if (ice != null)
                ice.localScale = new Vector3(ice.localScale.x, value, ice.localScale.z);
            if (screw != null)
            {
                var pos = screw.localPosition;
                screw.localPosition = new Vector3(pos.x, value * -0.4f, pos.z);
                
                var rot = screw.localRotation.eulerAngles;
                screw.localRotation = Quaternion.Euler(rot.x, (value * 360 * 5) % 360, rot.z);
            }
        }
    }
}
