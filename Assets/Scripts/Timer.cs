using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time;

    [SerializeField]
    private float firstTime;

    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private AudioSource endAlam;

    private bool isPlayEndAlam = false;

    public bool stopTime = false;
    public bool startTime = false;
    public bool endGame = false;


    [SerializeField]
    private VideoController videoController;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Count();

        if(time <= 0f){
            startTime = false;
            stopTime = true;
        }

        if(time <= 11f && !endGame) {
            ShakeScreen();
            timerText.color = new Color32(255, 35, 35, 255);
        } else {
            timerText.color = new Color32(255, 255, 255, 255);
        }
    }

    private void Count(){
        if(startTime && !stopTime){
            time = time - Time.deltaTime;
            timerText.text = time.ToString("0");
            endGame = false;
        } else {
            timerText.text = "PAROU";
            endGame = true;
        }
    } 

    public void ShakeScreen(){
        StartCoroutine(ShakeTime());      
    }

    public IEnumerator ShakeTime()
    {
        endAlam.Play();
        videoController.videoPlayer.Play();
        videoController.videoPlayer.isLooping = true;

        for(int i = 0; i < GetComponent<CameraManager>().cameras.Length; i++){
            GetComponent<CameraManager>().cameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 50;
        }

        
        yield return new WaitForSeconds(10f);

        
        videoController.videoPlayer.isLooping = false;
        endAlam.Stop();

        for(int i = 0; i < GetComponent<CameraManager>().cameras.Length; i++){
            GetComponent<CameraManager>().cameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
        }
    }
}
