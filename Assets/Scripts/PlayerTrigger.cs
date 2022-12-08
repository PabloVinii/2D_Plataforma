using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player player_script;

    public AudioClip fx_coin;
    // Start is called before the first frame update
    void Start()
    {   
        //encontrando o objeto player
        player_script = GameObject.Find("Player").GetComponent<Player>();
    }

    //quando colidir com Enemy, aplicar dano ao Player
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            player_script.DamagePlayer();
        }

        if(other.CompareTag("Water")){
            player_script.DamageByWater();
        }

        if(other.CompareTag("Coin")){
            Debug.Log("mueda");
            Destroy(other.gameObject);
            SoundManager.instance.PlaySound(fx_coin);
        }
    }
}
