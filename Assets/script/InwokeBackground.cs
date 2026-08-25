using Unity.VisualScripting;
using UnityEngine;

public class InwokeBackground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private float PX;
    
    [SerializeField]private float SX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < PX)
        {
            transform.position = new Vector3(SX, transform.position.y, transform.position.z);
        }
    }
}
