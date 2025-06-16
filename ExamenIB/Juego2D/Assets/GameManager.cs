using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float velocidad = 2;
    public Renderer bg;
    public GameObject col1;
    public GameObject piedra1;
    public GameObject piedra2;
    public GameObject platform; // Prefab de la plataforma

    public bool start = false;
    public bool gameOver = false;

    public GameObject menuInicio;
    public GameObject menuGameOver;
    public List<GameObject> suelo;
    public List<GameObject> obstaculos;
    public List<GameObject> plataformas = new List<GameObject>(); // Lista de plataformas

    private void Start()
    {
        // Crear Mapa
        for (int i = 0; i < 21; i++)
        {
            suelo.Add(Instantiate(col1, new Vector2(-10 + i, -3), Quaternion.identity));
        }

        // Crear Obstáculos
        obstaculos.Add(Instantiate(piedra1, new Vector2(15, -2), Quaternion.identity));
        obstaculos.Add(Instantiate(piedra2, new Vector2(20, -2), Quaternion.identity));

        // Crear bloques de plataformas accesibles
        float plataformaX = 5f; // posición inicial
        int bloques = 5;

        for (int b = 0; b < bloques; b++)
        {
            int cantidad = Random.Range(2, 4); // 2 o 3 plataformas seguidas
            float altura = Random.Range(-1f, 1.5f); // altura alcanzable

            for (int i = 0; i < cantidad; i++)
            {
                Vector2 pos = new Vector2(plataformaX, altura);
                GameObject nueva = Instantiate(platform, pos, Quaternion.identity);
                plataformas.Add(nueva);
                plataformaX += 0.8f; // espacio entre plataformas en un mismo bloque
            }

            // espacio entre bloques
            plataformaX += Random.Range(3f, 6f);
        }
    }

    private void Update()
    {
        if (!start && !gameOver)
        {
            menuInicio.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                start = true;
            }
        }
        else if (gameOver)
        {
            menuGameOver.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            menuInicio.SetActive(false);
            menuGameOver.SetActive(false);

            // Mover fondo
            bg.material.mainTextureOffset += new Vector2(0.015f, 0) * velocidad * Time.deltaTime;

            // Mover suelo
            for (int i = 0; i < suelo.Count; i++)
            {
                if (suelo[i].transform.position.x <= -10)
                {
                    suelo[i].transform.position = new Vector3(10f, -3, 0);
                }
                suelo[i].transform.position += new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
            }

            // Mover obstáculos
            for (int i = 0; i < obstaculos.Count; i++)
            {
                if (obstaculos[i].transform.position.x <= -10)
                {
                    float randomObs = Random.Range(10, 18);
                    obstaculos[i].transform.position = new Vector3(randomObs, -2, 0);
                }
                obstaculos[i].transform.position += new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
            }

            // Mover plataformas
            for (int i = 0; i < plataformas.Count; i++)
            {
                if (plataformas[i].transform.position.x <= -12)
                {
                    float maxX = GetMaxPlatformX();
                    float altura;
                    float nuevaX;
                    bool posicionValida = false;
                    int intentos = 0;

                    do
                    {
                        nuevaX = maxX + Random.Range(3f, 6f) + i * 1.5f;
                        altura = Random.Range(-1f, 1.5f); // Limita la altura máxima aquí

                        // Verifica que no haya obstáculos cerca en X
                        posicionValida = true;
                        foreach (var obs in obstaculos)
                        {
                            if (Mathf.Abs(obs.transform.position.x - nuevaX) < 5f)
                            {
                                posicionValida = false;
                                break;
                            }
                        }
                        intentos++;
                    }
                    while (!posicionValida && intentos < 10);

                    plataformas[i].transform.position = new Vector3(nuevaX, altura, 0);
                }
                plataformas[i].transform.position += new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
            }
        }
    }

    // Encuentra la plataforma más adelantada para colocar nuevas después
    private float GetMaxPlatformX()
    {
        float max = float.MinValue;
        foreach (var plat in plataformas)
        {
            if (plat.transform.position.x > max)
                max = plat.transform.position.x;
        }
        return max;
    }
}
