using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GraczStany
{
    ruch,
    chodzenie,
    atak,
    odepchniecie
}

public class KontrolerGracza : Zycie
{ 
    public GraczStany obecnyStan;
    public float predkosc;
    private Rigidbody2D rigidb;
    private Vector3 zmiana;

    private Animator animator;
    //public NewBehaviourScript hp;
    public int hp;

    public GameObject[] serca;
    public Sprite serce;

    
    public Text wynikMonet;
    public int wynik;


    // Start is called before the first frame update
    void Start()
    {
        wynik = 0;
        obecnyStan = GraczStany.chodzenie;
        animator = GetComponent<Animator>();
        rigidb = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        zmiana = Vector2.zero;
        zmiana.x = Input.GetAxisRaw("Horizontal"); // Pobranie z osi współrzędnych wektora. Dzięki RAW mamy wartości 0/1. Bez byłyby "losowe". Wciśnięcie strzałki przypisywane do wektora.
        zmiana.y = Input.GetAxisRaw("Vertical");
        Debug.Log(zmiana);

        
        if (Input.GetButtonDown("Atak") && obecnyStan != GraczStany.atak && obecnyStan != GraczStany.odepchniecie)
        {
            StartCoroutine(AtakCO());
        }
        else if (obecnyStan == GraczStany.chodzenie || obecnyStan == GraczStany.ruch)
        {
            UpdateAnimacje_Ruchy();
        }
    }


    private IEnumerator AtakCO()
    {
        animator.SetBool("czyAtak", true);
        obecnyStan = GraczStany.atak;
        yield return null;
        animator.SetBool("czyAtak", false);
        yield return new WaitForSeconds(.3f);
        obecnyStan = GraczStany.chodzenie;
    }

    void UpdateAnimacje_Ruchy()
    {
        if (zmiana != Vector3.zero)
        {
            RuchGracza();
            animator.SetFloat("moveX", zmiana.x);
            animator.SetFloat("moveY", zmiana.y);

            animator.SetBool("czyChodzenie", true);
        }
        else
        {
            animator.SetBool("czyChodzenie", false);
        }
    }


    void RuchGracza()
    {
        zmiana.Normalize();
        rigidb.MovePosition(transform.position + zmiana * predkosc * Time.deltaTime); 
    }

    public void Uderzenie(float czasUderzenia, int dmg)
    {
        
        if (hp > 1)
        {
            StartCoroutine(KopniecieCO(czasUderzenia, dmg));
        }
        else
        {
            serca[0].SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KopniecieCO(float czasUderzenia, int dmg)
    {
        if (rigidb != null)
        {
            yield return new WaitForSeconds(czasUderzenia);
            rigidb.velocity = Vector2.zero;
            obecnyStan = GraczStany.ruch;
            rigidb.velocity = Vector2.zero;
            hp = hp - dmg;
            UpdateHP(hp);
        }
    }

    public void UpdateHP(int zycie)
    {
        serca[zycie].SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D mon)
    {
        if(mon.gameObject.CompareTag("Moneta"))
        {
            wynik++;
            wynikMonet.text = wynik.ToString();
            mon.GetComponent<KontrolerMonety>().Spawn();
        }
    }
}
