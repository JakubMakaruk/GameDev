using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolerMonety : MonoBehaviour
{

    private float randX;
    private float randY;
    public GameObject moneta;
    public Vector2 pozycja;

    public int wynikMonet;

    // Start is called before the first frame update
    void Start()
    {
        wynikMonet = 0;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D cos)
    {
        if(cos.CompareTag("Player"))
        {
            if(transform.position == cos.transform.position)
            {
                Spawn();
            }
        }
    }

    public void Spawn()
    {
        randX = Random.Range(-5.5f, 5.5f);
        randY = Random.Range(-3.9f, 3.9f);

        pozycja = new Vector2(randX, randY);

        transform.position = pozycja;
    }
}
