using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sanduiche sanduicheMontar, sanduicheCardapio;
    public Ingrediente[] ingredientes;
    public Sanduiche[] sanduiches;
    private int posAtualIngrediente = 0, acertos = 0, erros = 0;
    private bool cardapioMontado = false, resultado = false, finalizouMontagem = false, canAddPoints = true;

    [SerializeField]
    private TMP_Text resultadoText, acertosText, errosText, sanduicheNameText, sanduicheDescText;
    
    [SerializeField]
    private Transform ingredientsSpawn, targetButtonsPanel, resultCardPanel;
    
    public Transform[] ingredientsTargetButtons;

    public GameObject buttonPrefab;
    public GameObject resultCardPrefab;

    public Image iconSanduiche;

    bool lastBread = false;

    [SerializeField]
    private VoiceController voiceController;

    [SerializeField]
    private VideoController videoController;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject canvaGame;

    [SerializeField]
    private CinemachineVirtualCamera endCamera;

    [SerializeField]
    private TMP_Text[] resultadosText;

    [SerializeField]
    private float time;

    public bool stopTime = false;

    [SerializeField]
    private Image timerIcon;

    [SerializeField]
    private AudioSource clock;



    void Start(){
        SetUpControls();
        
        finalizouMontagem = false;
        cardapioMontado = false;
        posAtualIngrediente = 1;
        canAddPoints = true;
        clock.Play();
        Limpar();

        GetComponent<Timer>().startTime = true;
    }

    void Awake(){
        videoController.PlayVideo();
    }

    // Update is called once per frame
    void Update(){
        if(GetComponent<Timer>().endGame == false){
            Count();

            if(time <= 0f){
                stopTime = true;
                resultado = VerificarMontagem();
                AddPontuacao(resultado);
                StartCoroutine(Restart());
            }

            if(!cardapioMontado)
                MontarCardapio();

            if(posAtualIngrediente >= 4)
                finalizouMontagem = true;

            UpdateUI();

            if(finalizouMontagem){
                if(lastBread){
                    sanduicheMontar.ingredientes[4] = ingredientes[0];
                    lastBread = false;
                    StartCoroutine(SpawnLastBread());
                }
                

                resultado = VerificarMontagem();

                AddPontuacao(resultado);
                StartCoroutine(Restart());
            }
        } else {
            clock.Stop();
            canvaGame.SetActive(false);
            endCamera.gameObject.SetActive(true);
            endCamera.Priority = 1000;
            ResultadoFinal();
        }
    }

    public void MontarCardapio(){
        int index = Random.Range(0, 4);
        sanduicheCardapio = sanduiches[index];
        
        cardapioMontado = true;
    }

    public void AdicionaIngrediente(int id){
        if(!stopTime){
            GameObject ingredient;
            sanduicheMontar.ingredientes[posAtualIngrediente] = ingredientes[id];
            ingredient = Instantiate(ingredientes[id].model, ingredientsSpawn.position, Quaternion.identity);
            ingredient.transform.SetParent(ingredientsSpawn);
            posAtualIngrediente++;
            if(posAtualIngrediente >= 4){
                lastBread = true;
            }
        }        
    }

    private bool VerificarMontagem(){
        for(int i = 0; i < 5; i++){
            if(sanduicheCardapio.ingredientes[i] == sanduicheMontar.ingredientes[i]){
            } else {
                return false;
            }
            
        }
        return true;
    }

    private void Limpar(){
        for(int i = 0; i < 5; i++){
            //sanduicheCardapio.ingredientes[i] = null;
            sanduicheMontar.ingredientes[i] = null;
        }

        foreach (Transform child in ingredientsSpawn) {
            GameObject.Destroy(child.gameObject);
        }

        sanduicheMontar.ingredientes[0] = ingredientes[0];
        GameObject ingredient;
        ingredient = Instantiate(ingredientes[0].model, ingredientsSpawn.position, Quaternion.identity);
        ingredient.transform.SetParent(ingredientsSpawn);
        
    }

    private void AddPontuacao(bool resultado){
        if(canAddPoints){
            if(resultado){
                acertos++;
                anim.SetFloat("speed", 2);
                StartCoroutine(ResetAnim());
            } else {
                erros++;
                anim.SetFloat("speed", 0);
                StartCoroutine(ResetAnim());
            }
            GameObject obj;
            obj = Instantiate(resultCardPrefab, ingredientsSpawn.position, Quaternion.identity);
            obj.transform.SetParent(resultCardPanel);
            obj.GetComponent<ResultCard>().SetUp(resultado);
            voiceController.SetUp(resultado);
            canAddPoints = false;
        }
    }

    IEnumerator Restart()
    {
        clock.Stop();
        GetComponent<CameraManager>().canChange = true;
        yield return new WaitForSeconds(0.5f);

        GetComponent<CameraManager>().ChangeCamera();
        finalizouMontagem = false;
        cardapioMontado = false;
        posAtualIngrediente = 1;
        canAddPoints = true;
        stopTime = false;
        time = 10f;
        clock.Play();
        Limpar();
    }

    IEnumerator ResetAnim()
    {
        yield return new WaitForSeconds(2f);
        anim.SetFloat("speed", 0.5f);
    }

    private void UpdateUI(){
        acertosText.text = "" + acertos;
        errosText.text = "" + erros;
        sanduicheNameText.text = "" + sanduicheCardapio.nome;
        iconSanduiche.GetComponent<Image>().sprite = sanduicheCardapio.icone;
        sanduicheDescText.text = "- " + sanduicheCardapio.ingredientes[1].nome + "\n" + "- " + sanduicheCardapio.ingredientes[2].nome + "\n" + "- " + sanduicheCardapio.ingredientes[3].nome + "\n";

    }

    private void SetUpControls(){
        for(int i = 0; i < 5; i++){
            GameObject obj;
            Ingrediente ingrediente = ingredientes[i];
            obj = Instantiate(buttonPrefab, targetButtonsPanel.position, Quaternion.identity);
            obj.transform.SetParent(targetButtonsPanel);

            obj.GetComponent<Marker>().target = ingredientsTargetButtons[i];
            obj.transform.GetChild(0).GetComponent<Image>().sprite = ingrediente.icone;

            obj.GetComponent<Button>().onClick.AddListener(delegate{
                AdicionaIngrediente(ingrediente.id);
            });
        }
    }

    IEnumerator SpawnLastBread()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject ingredient;
        ingredient = Instantiate(ingredientes[5].model, ingredientsSpawn.position, Quaternion.identity);
        ingredient.transform.SetParent(ingredientsSpawn);

    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    void ResultadoFinal(){
        resultadosText[0].text = "" + acertos;
        resultadosText[1].text = "" + erros;
        resultadosText[2].text = "Final: " + (acertos - erros);
    }

    private void Count(){
        if(!stopTime){
            time = time - Time.deltaTime;
            timerIcon.fillAmount = time / 10f;

            if(time < 6){
                clock.pitch = 1f;
            } else {
                clock.pitch = 0.5f;
            }
        }    
    } 

}
