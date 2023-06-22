using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class InitialScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject timeLine, initialCanva;

    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(StartGame());

        }
    }

    

    IEnumerator StartGame()
    {
        initialCanva.SetActive(false);

        yield return new WaitForSeconds(4f);

        timeLine.SetActive(true);
    }
}