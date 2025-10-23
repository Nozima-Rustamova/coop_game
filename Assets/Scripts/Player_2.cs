using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_2 : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("HorizontalP2");
        float verticalInput = Input.GetAxis("VerticalP2");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}