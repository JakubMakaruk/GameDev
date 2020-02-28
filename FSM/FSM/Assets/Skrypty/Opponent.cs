using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : KontrolerEnemy
{
    private Rigidbody2D myRigidbody;
    public Transform cel;
    public Transform powrot;
    public float gonicPromien;
    public float atakPromien;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        obecnyStan = OpponentStany.ruch;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cel = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ZmierzDystans();
    }

    void ZmierzDystans()
    {
        if(Vector3.Distance(cel.position, transform.position) <= gonicPromien    &&     Vector3.Distance(cel.position, transform.position) > atakPromien)
        {
            if (obecnyStan == OpponentStany.ruch || obecnyStan == OpponentStany.chodzenie && obecnyStan != OpponentStany.odepchniecie)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, cel.position, predkosc * Time.deltaTime);
                ZmienAnimacje(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ZmienStan(OpponentStany.chodzenie);
                anim.SetBool("czyPobudzenie", true);
            }
        }
        else if (Vector3.Distance(cel.position, transform.position) > gonicPromien)
        {
            anim.SetBool("czyPobudzenie", false);
        }
    }

    private void ZmienStan(OpponentStany nowyStan)
    {
        if(obecnyStan != nowyStan)
        {
            obecnyStan = nowyStan;
        }
    }


    private void UstawFloata(Vector2 wektor)
    {
        anim.SetFloat("niedzwiedzX", wektor.x);
        anim.SetFloat("niedzwiedzY", wektor.y);
    }

    private void ZmienAnimacje(Vector2 wspolrzedne)
    {
        if(Mathf.Abs(wspolrzedne.x) > Mathf.Abs(wspolrzedne.y))
        {
            if(wspolrzedne.x > 0)
            {
                UstawFloata(Vector2.right);
            }
            else if(wspolrzedne.x < 0)
            {
                UstawFloata(Vector2.left);
            }
        }
        else if(Mathf.Abs(wspolrzedne.x) < Mathf.Abs(wspolrzedne.y))
        {
            if(wspolrzedne.y > 0)
            {
                UstawFloata(Vector2.up);
            }
            else if(wspolrzedne.y < 0)
            {
                UstawFloata(Vector2.down);
            }
        } 
    }

}
