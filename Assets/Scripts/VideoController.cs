using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoController : MonoBehaviour
{
    
    public VideoPlayer videoPlayer;

    [SerializeField]
    private Timer timer;

    private float time;

    void Start(){
        videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"bubble.mp4"); 
    }


    public void PlayVideo(){
        videoPlayer.Play();
    }

    void Update(){
        Count();
        
        if(time >= 50f){
            timer.ShakeScreen();
            
            time = 0f;
        }
    }

    private void Count(){
        time = time + Time.deltaTime;       
    } 

}
