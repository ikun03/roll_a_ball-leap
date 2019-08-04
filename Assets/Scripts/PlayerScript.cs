using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;

    private int count;

    public bool isLeft;
    public bool isRight;
    public bool isForward;
    public bool isBackward;

    public Text score;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        UpdateScoreText();
    }

    private void FixedUpdate()
    {
        /*var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);*/

        if (isLeft)
        {
            rb.AddForce(Vector3.left * speed);
        }

        if (isRight)
        {
            rb.AddForce(Vector3.right * speed);
        }

        if (isForward)
        {
            rb.AddForce(Vector3.forward * speed);
        }

        if (isBackward)
        {
            rb.AddForce(Vector3.back * speed);
        }

        //rb.AddForce(Vector3.back * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("pickup")) return;
        count++;
        other.gameObject.SetActive(false);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        score.text = "Score: " + count.ToString();
    }
}