using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "gameObjects/Ingrediente")]
public class Ingrediente : ScriptableObject
{
    public int id;
    public string nome;
    public Sprite icone;
    public GameObject model;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
