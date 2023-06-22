using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class VoiceController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] positiveVoices, negativeVoices;

    [SerializeField]
    private string[] positiveTexts, negativeTexts;

    [SerializeField]
    private TMP_Text legenda;


    public void SetUp(bool result)
    {
        if(result){
            int index = Random.Range(0, positiveVoices.Length);
            GetComponent<AudioSource>().clip = positiveVoices[index];
            GetComponent<AudioSource>().Play();
            legenda.text = positiveTexts[index];
            StartCoroutine(LimparLegenda());
        } else {
            int index = Random.Range(0, negativeVoices.Length);
            GetComponent<AudioSource>().clip = negativeVoices[index];
            GetComponent<AudioSource>().Play();
            legenda.text = negativeTexts[index];
            StartCoroutine(LimparLegenda());
        }
    }

    IEnumerator LimparLegenda()
    {
        yield return new WaitForSeconds(4f);
        
        legenda.text = "";
    }
}
