using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public int jump_force;
    public int life;
    public Transform ground_check;

    private bool invunerable = false;
    private bool grounded = false;
    private bool jumping = false;
    private bool facing_right = true;

    // declara o tipo da variavel 
    private SpriteRenderer sprite; // sprite é um tipo SpriteRenderer
    private Rigidbody2D rig_body; // rig_body é um tipo Rigidbody2D
    private Animator anim;

    //atack
    public float fire_rate;
    public Transform spawn_attack;
    public GameObject attack_prefab;
    public GameObject crown;
    private float next_attack = 0f;

    //camera
    private Camera camera_script;
    public float timer_shake;
    public float intensity_timer;
    
    //audio
    public AudioClip fx_hurt;
    public AudioClip fx_jump;
    public AudioClip fx_attack;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer> ();
        rig_body = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        camera_script = GameObject.Find("Main Camera").GetComponent<Camera>();
        // declara que cada variavel recebe o componente do proprio objeto onde está atribuida
        // Objeto player possui um Rigidbody2D, e o script está dentro de player
        // GetComponent<>() está atruibuindo a variavel rig_body o componente do próprio player
    }

    // Update is called once per frame
    void Update()
    {
        // cria uma linha imaginaria pra checar se o ground_check esta colidindo com a camada do chão
        grounded = Physics2D.Linecast(transform.position, ground_check.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown("Jump") && grounded){
            jumping = true;
            SoundManager.instance.PlaySound(fx_jump);
        }

        SetAnimations();
        if (Input.GetButton("Fire1") && grounded && Time.time > next_attack){
            SoundManager.instance.PlaySound(fx_attack);
            attack();
        }
    }

    // feita pra trabalhar com RigidBody
    void FixedUpdate() {
        // pega os input das configurações do projeto
        float move = Input.GetAxis("Horizontal");
        // Vector2 recebe 2 parametros: X e Y
        rig_body.velocity = new Vector2(move * speed, rig_body.velocity.y);

        if ((move < 0f && facing_right) || (move > 0f && !facing_right))
        {
            Flip();
        }
         
        if (jumping){
            rig_body.AddForce(new Vector2(0f, jump_force));
            jumping = false;
        }

    }

    void Flip(){
        facing_right = !facing_right;
        //tranform é um dos poucos componentes que não precisa ser inicializado
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void SetAnimations(){
        anim.SetFloat("vel_y", rig_body.velocity.y);
        anim.SetBool("jump_fall", !grounded);
        anim.SetBool("walking", rig_body.velocity.x != 0f && grounded);
    }

    void attack(){
        anim.SetTrigger("punch");
        next_attack = Time.time + fire_rate;

        GameObject clone_attack = Instantiate(attack_prefab, spawn_attack.position, spawn_attack.rotation);

        if(!facing_right){
            clone_attack.transform.eulerAngles = new Vector3 (180, 0, 180);
        }
    }

    IEnumerator DamageEffect(){
        //criar efeito na camera
        camera_script.ShakeCamera(timer_shake, intensity_timer);

        // faz o player piscar varias vezes até 1 segundo
        for (float i = 0f; i < 1f; i +=0.1f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        
        invunerable = false;
    }

    public void DamagePlayer(){
        if(!invunerable){
        invunerable = true;
        life--;
        StartCoroutine(DamageEffect());

        SoundManager.instance.PlaySound(fx_hurt);
        Hud.instance.RefreshLife(life);

        if(life < 1){
            Debug.Log("faliceu");
            kingDeath();
            Invoke("ReloadLevel", 3f);
            gameObject.SetActive(false);
            }
        }
    }

    public void DamageByWater(){
        life = 0;
        Hud.instance.RefreshLife(life);
        kingDeath();
        Invoke("ReloadLevel", 3f);
        gameObject.SetActive(false);
    }
    void kingDeath(){
        GameObject clone_crown = Instantiate(crown, transform.position, Quaternion.identity);
        Rigidbody2D rg_crown = clone_crown.GetComponent<Rigidbody2D>();
        rg_crown.AddForce(Vector3.up * 500);
    }

    void ReloadLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

}
