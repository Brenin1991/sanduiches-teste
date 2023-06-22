using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    public bool canChange = true;
    int camIndex = 0;
    public CinemachineVirtualCamera[] cameras;

    public void ChangeCamera(){
        
        if(canChange){
            camIndex++;
            if(camIndex >= cameras.Length)
                camIndex = 0;           
            for(int i = 0; i < cameras.Length; i++){
                if(i == camIndex){
                    cameras[i].Priority = 20;
                } else {
                    cameras[i].Priority = 0;
                }
            }
            canChange = false;
        }        
    }
}
