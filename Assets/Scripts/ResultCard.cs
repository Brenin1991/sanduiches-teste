using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultCard : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private Image img;

    [SerializeField]
    private AudioClip[] sounds;

    void Start()
    {
        StartCoroutine(DestroyCard());
    }

    // Update is called once per frame
    public void SetUp(bool result)
    {
        if(result){
            text.text = "ACERTOU";
            img.color = new Color32(34,180,0,255);
            GetComponent<AudioSource>().clip = sounds[0];
            GetComponent<AudioSource>().Play();
        } else {
            img.color = new Color32(180,20,0,255);
            text.text = "ERROU";
            GetComponent<AudioSource>().clip = sounds[1];
            GetComponent<AudioSource>().Play();
        }
    }
    IEnumerator DestroyCard()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
