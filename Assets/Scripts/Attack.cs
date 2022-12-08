using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public float speed;
    public float time_destroy;
    
    void Start()
    {
        //spawna e destroi após o time_destroy
        Destroy(gameObject, time_destroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
