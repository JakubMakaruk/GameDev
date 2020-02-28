using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolerOpponenta : MonoBehaviour
{
    public float predkosc;
    private Rigidbody2D rigidb;
    private Vector3 zmiana;

    private Animator animator;


    public Transform cel;
    public Transform powrot;
    public float miarka;
    public float mozliwyAtak;



    // Start is called before the first frame update
    void Start()
    {
        //predkosc = 3;
        //cel = GameObject.FindWithTag("Gracz").transform;
        animator = GetComponent<Animator>();
        rigidb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        zmiana = Vector2.zero;
        zmiana.x = Input.GetAxisRaw("Horizontal"); // Pobranie z osi współrzędnych wektora. Dzięki RAW mamy wartości 0/1. Bez byłyby "losowe". Wciśnięcie strzałki przypisywane do wektora.
        zmiana.y = Input.GetAxisRaw("Vertical");
        Debug.Log(zmiana);
        UpdateAnimacje_Ruchy();
    }

    
    void UpdateAnimacje_Ruchy()
    {
        if (zmiana != Vector3.zero)
        {
            RuchGracza();
            animator.SetFloat("niedzwiedzX", zmiana.x);
            animator.SetFloat("niedzwiedzY", zmiana.y);

            animator.SetBool("czyChodzenieNiedzwiedz", true);
        }
        else
        {
            animator.SetBool("czyChodzenieNiedzwiedz", false);
        }
    }


    void RuchGracza()
    {
        rigidb.MovePosition(transform.position + zmiana * predkosc * Time.deltaTime);
    }

    /*void ZmierzDystans()
    {
        if(Vector3.Distance(cel.position, transform.position) <= miarka)
        {
            transform.position = Vector3.MoveTowards(transform.position, cel.position, mozliwyAtak);
        }
    }*/

    
}
