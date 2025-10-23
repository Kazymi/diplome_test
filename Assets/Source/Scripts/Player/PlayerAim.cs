using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Joystick joystick; 
    [SerializeField] private Transform colliderTransform; 
    
    void Update()
    {
        if (joystick.Direction.magnitude > 0.1f) 
        {
            float angle = Mathf.Atan2(joystick.Direction.y, joystick.Direction.x) * Mathf.Rad2Deg;
            colliderTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
