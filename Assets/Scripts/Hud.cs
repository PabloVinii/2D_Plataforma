using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Hud : MonoBehaviour
{

    public Sprite[] sprites;
    public Image life_bar;
    public static Hud instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(instance);
        }
    }

    public void RefreshLife(int player_life){
        life_bar.sprite = sprites[player_life];
    }
}
