using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public InputAction fly;
    [SerializeField]private float jumphorce = 5f;
    Rigidbody2D rb;
    [SerializeField] private float flyspeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        fly.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (fly.WasPressedThisFrame())
        {
            flys();
        }
    }
    void flys()
    {
        
        
            Vector3 velocity = rb.linearVelocity;
             velocity.y = flyspeed;
             rb.linearVelocity = velocity;
    
        }
            
}
