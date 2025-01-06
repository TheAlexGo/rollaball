using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private int pickUpCount;
    
    private float movementX;
    private float movementY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        pickUpCount = GameObject.FindGameObjectsWithTag("PickUp").Length;
    }

    // This function is called when a move input is detected.
    private void OnMove(InputValue movementValue)
    {
        var movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void SetCountText()
    {
        countText.text = $"Count: {count}";

        if (count == pickUpCount)
        {
            winTextObject.SetActive(true);
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        var movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PickUp"))
        {
            return;
        }
        other.gameObject.SetActive(false);
        count++;
        SetCountText();
    }
}
