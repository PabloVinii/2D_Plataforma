 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private bool facing_right = true;
    private SpriteRenderer sprite; // sprite é um tipo SpriteRenderer
    private Rigidbody2D rig_body; // rig_body é um tipo Rigidbody2D
    private bool touched_wall = false;

    public int life;
    public float speed;
    public Transform wall_check;

    void Start()
    {
        rig_body = GetComponent<Rigidbody2D> ();
        sprite = GetComponent<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
        touched_wall = Physics2D.Linecast(transform.position, wall_check.position, 1 << LayerMask.NameToLayer("Ground"));
        if (touched_wall){
            Flip();
        }
    }
    void FixedUpdate(){
        rig_body.velocity = new Vector2(speed, rig_body.velocity.y);
    }

    void Flip(){
        facing_right = !facing_right;
        //tranform é um dos poucos componentes que não precisa ser inicializado
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Attack")){
            DamageEnemy();
        }
    }

    // faz o objeto piscar em vermelho, perder vida e receber impulso
    IEnumerator DamageEffect(){
        float actual_speed = speed;
        speed = speed* -1;
        sprite.color = Color.red;
        rig_body.AddForce(new Vector2(0f, 200f));
        yield return new WaitForSeconds(0.1f);
        speed = actual_speed;
        sprite.color = Color.white;
    }

    //aplica dano a cobra
    void DamageEnemy(){
        life--;
        StartCoroutine(DamageEffect());
        if (life < 1){
            Destroy(gameObject);
        }
    }
}
