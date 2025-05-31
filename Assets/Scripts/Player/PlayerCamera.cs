using UnityEngine;
using UniRx;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _distance;
    private float _posZ;
    private void Start()
    {
        PlayerController.Instance.PosZ.Subscribe(x => 
        {
            _posZ = x;
        }).AddTo(this);
    }
    void Update()
    {
        transform.position = new(transform.position.x, transform.position.y, _posZ - _distance);
    }
}
