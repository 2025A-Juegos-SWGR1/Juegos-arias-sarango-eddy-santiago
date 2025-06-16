using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public GameManager gameManager;
    public float fuerzaSalto;

    private Rigidbody2D rb;
    private Animator anim;
    private int saltosRestantes = 2; // Permite hasta doble salto
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.start)
        {
            if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
            {
                anim.SetBool("estaSaltando", true);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Usar linearVelocity
                rb.AddForce(new Vector2(0, fuerzaSalto));
                saltosRestantes--;
            }
        }

        if (gameManager.gameOver)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Suelo")
        {
            anim.SetBool("estaSaltando", false);
            saltosRestantes = 2; // Reinicia los saltos al tocar el suelo
        }

        if (collision.gameObject.tag == "Obstaculo")
        {
            gameManager.gameOver = true;
        }
    }
}
