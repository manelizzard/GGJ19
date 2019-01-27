using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntensity : MonoBehaviour
{


    [FMODUnity.EventRef]
    public string NAME_EVENT = "event:/Music";
    public FMOD.Studio.EventInstance AUDIO_EVENT;
    public FMOD.Studio.ParameterInstance PARAMETER_EVENT;

    public float intensity;
    float lastV;

    // Start is called before the first frame update
    void Start()
    {
        AUDIO_EVENT = FMODUnity.RuntimeManager.CreateInstance(NAME_EVENT);
        AUDIO_EVENT.getParameter("music_intensity", out PARAMETER_EVENT);
        AUDIO_EVENT.start();
        PARAMETER_EVENT.setValue(0);
        InvokeRepeating("CheckMusicIntensity", 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckMusicIntensity()
    {

        int nplayers = 0;
        GameManager.instance.players.ForEach((p) => { nplayers += p.shipCount; });

        int maxp = 200;
        float v = (float)nplayers / (float)maxp;
        Debug.Log(v);



        v += (v - lastV)/10;
        if (Player.playersTargetedToCurrentPlanet > 1)
        {
            v = 1;
        }
        PARAMETER_EVENT.setValue(v);


    }
}
