using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System; // do funkcji Math.Max, Math.Min

public class KontrolerGry1vs1 : MonoBehaviour
{
    public int czyjRuch; // x=0, o=1
    public int licznikRuchow;
    public GameObject[] tablicaPodswietlenCzyjRuch;
    public Sprite[] obiektyXO; //x.png, o.png

    public Button[] polaPlanszy;
    public char[,] oznaczonePola = new char[3, 3]; // tablica charów z zajętymi polami, typy:  _ , x , o 
    public int[] zajetePola; // tablica intów z zajętymi polami, typy: -100, 1, 2

    public int wynikX;
    public int wynikY;

    public Text wynikGraczaX;
    public Text wynikGraczaO;


    public Sprite przyciskRewanzu;
    public Sprite przyciskNowejGry;
    public Sprite przyciskWyjscia;
    public Button[] przyciskiRewanz_NowaGra_Wyjscie;

    public Sprite pustyButton;

    public GameObject[] wygranePodświetloneButtony;



    char player = 'x', opponent = 'o';

    // -------------------------------------------------------------------------------- START
    void Start()
    {
        UruchomGre(0);
    }

    // -------------------------------------------------------------------------------- URUCHOM GRE
    void UruchomGre(int kogoRuch)
    {
        czyjRuch = kogoRuch;
        licznikRuchow = 0;

        if (czyjRuch == 0)
        {
            tablicaPodswietlenCzyjRuch[0].SetActive(true);
            tablicaPodswietlenCzyjRuch[1].SetActive(false);
        }
        else if (czyjRuch == 1)
        {
            tablicaPodswietlenCzyjRuch[0].SetActive(false);
            tablicaPodswietlenCzyjRuch[1].SetActive(true);
        }

        przyciskiRewanz_NowaGra_Wyjscie[0].gameObject.SetActive(false);
        przyciskiRewanz_NowaGra_Wyjscie[1].gameObject.SetActive(false);
        przyciskiRewanz_NowaGra_Wyjscie[2].gameObject.SetActive(false);
        przyciskiRewanz_NowaGra_Wyjscie[0].GetComponent<Image>().sprite = null;
        przyciskiRewanz_NowaGra_Wyjscie[1].GetComponent<Image>().sprite = null;
        przyciskiRewanz_NowaGra_Wyjscie[2].GetComponent<Image>().sprite = null;


        for (int i = 0; i < polaPlanszy.Length; i++)
        {
            polaPlanszy[i].interactable = true;
            polaPlanszy[i].GetComponent<Image>().sprite = null;
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                oznaczonePola[i, j] = '_';
            }
        }

        for(int i=0; i<zajetePola.Length; i++)
        {
            zajetePola[i] = -100;
        }

        for(int i=0; i<wygranePodświetloneButtony.Length; i++)
        {
            wygranePodświetloneButtony[i].SetActive(false);
        }
    }

    // -------------------------------------------------------------------------------- OPERACJA PRZYCISKU
    public void OperacjaPrzycisku(int numer)
    {
        polaPlanszy[numer].image.sprite = obiektyXO[czyjRuch];
        polaPlanszy[numer].interactable = false;

        zajetePola[numer] = czyjRuch + 1;

        char kto = '_';
        if (czyjRuch == 0)
            kto = player;
        else if (czyjRuch == 1)
            kto = opponent;

        // oznaczenie zajętej komórki przez konkretnego gracza
        if (numer >= 0 && numer < 3)
        {
            oznaczonePola[0, numer] = kto;
        }
        else if (numer >= 3 && numer < 6)
        {
            numer = numer - 3;
            oznaczonePola[1, numer] = kto;
        }
        else if (numer >= 6 && numer < 9)
        {
            numer = numer - 6;
            oznaczonePola[2, numer] = kto;
        }
        licznikRuchow++;


        if (licznikRuchow > 4) // od 5 ruchów dopiero będzie możliwe zwycięstwo
        {
            bool czyJestWygrana = sprawdzenieZwyciezcy();
            if (licznikRuchow == 9 && czyJestWygrana == false)
            {
                przyciskiRewanz_NowaGra_Wyjscie[0].gameObject.SetActive(true);
                przyciskiRewanz_NowaGra_Wyjscie[1].gameObject.SetActive(true);
                przyciskiRewanz_NowaGra_Wyjscie[2].gameObject.SetActive(true);
                przyciskiRewanz_NowaGra_Wyjscie[0].image.sprite = przyciskRewanzu;
                przyciskiRewanz_NowaGra_Wyjscie[1].image.sprite = przyciskNowejGry;
                przyciskiRewanz_NowaGra_Wyjscie[2].image.sprite = przyciskWyjscia;
            }
            if (czyJestWygrana == true)
            {
                for (int i = 0; i < polaPlanszy.Length; i++)
                {
                    if (polaPlanszy[i].interactable == true)
                    {
                        polaPlanszy[i].interactable = false;
                        polaPlanszy[i].GetComponent<Image>().sprite = pustyButton;
                    }
                }
            }
        }

        if (czyjRuch == 0)
        {
            czyjRuch = 1;
            tablicaPodswietlenCzyjRuch[0].SetActive(false);
            tablicaPodswietlenCzyjRuch[1].SetActive(true);
        }
        else
        {
            czyjRuch = 0;
            tablicaPodswietlenCzyjRuch[0].SetActive(true);
            tablicaPodswietlenCzyjRuch[1].SetActive(false);
        }
    }


    public bool sprawdzenieZwyciezcy()
    {
        int typ1 = zajetePola[0] + zajetePola[1] + zajetePola[2];  // poziom-góra
        int typ2 = zajetePola[3] + zajetePola[4] + zajetePola[5];  // poziom-środek
        int typ3 = zajetePola[6] + zajetePola[7] + zajetePola[8];  // poziom-dół

        int typ4 = zajetePola[0] + zajetePola[3] + zajetePola[6];  // pion-lewo
        int typ5 = zajetePola[1] + zajetePola[4] + zajetePola[7];  // pion-środek
        int typ6 = zajetePola[2] + zajetePola[5] + zajetePola[8];  // pion-prawo

        int typ7 = zajetePola[0] + zajetePola[4] + zajetePola[8];  // przekątna-lewo->prawo
        int typ8 = zajetePola[2] + zajetePola[4] + zajetePola[6];  // przekątna-lewo<-prawo

        var typyZwyciestwa = new int[] { typ1, typ2, typ3, typ4, typ5, typ6, typ7, typ8 };  // tablica zawierająca wszystkie możliwe opcje zwycięstwa

        for (int i = 0; i < typyZwyciestwa.Length; i++)
        {
            if (typyZwyciestwa[i] == 3 * (czyjRuch + 1))  // jeżeli zwycięży X(0), to wynik=3 | jeżeli zwycięży O(1), to wynik=6
            {
                switch(i)
                {
                    case 0:
                        wyswietlenieZwyciezcy(0, 1, 2);  // wywołanie funkcji wyświetlenia zwycięzcy, jeśli takowego znaleźliśmy
                        break;
                    case 1:
                        wyswietlenieZwyciezcy(3, 4, 5);
                        break;
                    case 2:
                        wyswietlenieZwyciezcy(6, 7, 8);
                        break;
                    case 3:
                        wyswietlenieZwyciezcy(0, 3, 6);
                        break;
                    case 4:
                        wyswietlenieZwyciezcy(1, 4, 7);
                        break;
                    case 5:
                        wyswietlenieZwyciezcy(2, 5, 8);
                        break;
                    case 6:
                        wyswietlenieZwyciezcy(0, 4, 8);
                        break;
                    case 7:
                        wyswietlenieZwyciezcy(2, 4, 6);
                        break;
                }
                return true;
            }
        }
        return false;
    }

    void wyswietlenieZwyciezcy(int a, int b, int c)
    {
        if(czyjRuch == 0)
        {
            wynikX++;
            wynikGraczaX.text = wynikX.ToString();
        }
        else if(czyjRuch == 1)
        {
            wynikY++;
            wynikGraczaO.text = wynikY.ToString();
        }

        przyciskiRewanz_NowaGra_Wyjscie[0].gameObject.SetActive(true);
        przyciskiRewanz_NowaGra_Wyjscie[1].gameObject.SetActive(true);
        przyciskiRewanz_NowaGra_Wyjscie[2].gameObject.SetActive(true);
        przyciskiRewanz_NowaGra_Wyjscie[0].GetComponent<Image>().sprite = przyciskRewanzu;
        przyciskiRewanz_NowaGra_Wyjscie[1].GetComponent<Image>().sprite = przyciskNowejGry;
        przyciskiRewanz_NowaGra_Wyjscie[2].GetComponent<Image>().sprite = przyciskWyjscia;

        wygranePodświetloneButtony[a].SetActive(true);
        wygranePodświetloneButtony[b].SetActive(true);
        wygranePodświetloneButtony[c].SetActive(true);
    }

    public void rewanz()
    {
        UruchomGre(czyjRuch);
    }

    public void nowaGra()
    {
        wynikGraczaX.text = "0";
        wynikGraczaO.text = "0";
        wynikX = 0;
        wynikY = 0;
        UruchomGre(0);
    }

    public void wyjscie()
    {
        SceneManager.LoadScene(0);
    }
}
