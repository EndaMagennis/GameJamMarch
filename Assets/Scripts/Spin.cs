using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public MeshRenderer Renderer;

    public float timeScale;
    public Material material;
    public Color randomColor;
    public float randomRed;
    public float randomGreen;
    public float randomBlue;
    public float randomAlpha;
    public float randomScale;


    public AudioClip bling;
    public AudioSource coinSound;


    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(10.0f * Time.deltaTime, 0.0f, 0.0f);
        ChangeColor();
    }

    void ChangeColor()
    {
        randomRed = Random.Range(0f, 1.0f);
        randomGreen = Random.Range(0f, 1.0f);
        randomBlue = Random.Range(0f, 1.0f);
        randomAlpha = Random.Range(0f, 1.0f);

        if (timeScale <= Time.deltaTime)
        {
            material = Renderer.material;
            material.color = randomColor;

            randomColor = new Color(randomRed, randomGreen, randomBlue, randomAlpha);
            timeScale = 1.0f;
        }
        else
        {

            material.color = Color.Lerp(material.color, randomColor, Time.deltaTime / timeScale);
            timeScale -= Time.deltaTime;
        }
    }
}
