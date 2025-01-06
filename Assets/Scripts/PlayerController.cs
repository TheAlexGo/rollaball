using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private const string PICK_UP_TAG = "PickUp";
    private const string ENEMY_TAG = "Enemy";

    private const string COUNT_TEXT = "Очки";
    private const string YOU_WIN_TEXT = "Вы выиграли!";
    private const string YOU_LOSE_TEXT = "Вы проиграли!";

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
        winTextObject.SetActive(false);
        pickUpCount = GameObject.FindGameObjectsWithTag(PICK_UP_TAG).Length;
        SetCountText();
    }

    // This function is called when a move input is detected.
    private void OnMove(InputValue movementValue)
    {
        var movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(ENEMY_TAG))
        {
            return;
        }
        Destroy(gameObject);
        winTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = YOU_LOSE_TEXT;
        winTextObject.gameObject.SetActive(true);
    }

    private void SetCountText()
    {
        countText.text = $"{COUNT_TEXT}: {count}";

        if (count < pickUpCount)
        {
            return;
        }
        winTextObject.gameObject.GetComponent<TextMeshProUGUI>().text = YOU_WIN_TEXT;
        winTextObject.SetActive(true);
        Destroy(GameObject.FindGameObjectWithTag(ENEMY_TAG));
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        var movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(PICK_UP_TAG))
        {
            return;
        }
        other.gameObject.SetActive(false);
        count++;
        SetCountText();
    }
}
