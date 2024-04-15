using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Transform camera;
    [SerializeField]
    private float moveSpeed = 5500;
    [SerializeField]
    private float rotationSpeed = 90;
    [SerializeField]
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float XForece = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float ZForece = moveSpeed * Time.deltaTime *Input.GetAxis("Vertical");
        float Rot = rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        float camRot = rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");
        rb.AddRelativeForce(XForece, 0, ZForece);
        transform.Rotate(0, Rot, 0);
        camera.Rotate(-camRot, 0, 0);
    }
}
