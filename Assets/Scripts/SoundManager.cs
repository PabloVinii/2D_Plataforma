using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource fx_source;
    public AudioSource music_source;

    // static permite que outros scripts possam chamar seus metodos
    public static SoundManager instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void PlaySound(AudioClip clip){
        fx_source.clip = clip;
        fx_source.Play();
    }
}
