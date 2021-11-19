using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private TestPlayerStatus playerStatus;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }
    // TODO: change speed
    void PlayerMove()
    {
        float xMove = 0, yMove = 0, zMove = 0;

        if (Input.GetKey(KeyCode.W))
        {
            // Up
            zMove += 5 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Down
            zMove -= 5 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Left
            xMove -= 5 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Right
            xMove += 5 * Time.deltaTime;
        }

        player.Translate(new Vector3(xMove, yMove, zMove));
    }
}
