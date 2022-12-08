using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector2 velocity;
    private Transform player;
    private float shake_timer;
    private float shake_intensity;

    public float smooth_time_X;
    public float smooth_time_Y;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (player != null){
            float pos_x = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smooth_time_X);
            float pos_y = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smooth_time_Y);

            transform.position = new Vector3(pos_x, pos_y, transform.position.z);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (shake_timer >= 0f)
        {
            Vector2 shake_pos = Random.insideUnitCircle * shake_intensity;
            transform.position = new Vector3(transform.position.x + shake_pos.x, transform.position.y + shake_pos.y, transform.position.z);
            shake_timer -= Time.deltaTime;
        }
    }

    public void ShakeCamera(float Timer, float Intensity){
        shake_timer = Timer;
        shake_intensity = Intensity;
    }
}
