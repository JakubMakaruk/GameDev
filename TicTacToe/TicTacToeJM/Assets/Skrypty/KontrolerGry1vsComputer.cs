using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System; // do funkcji Math.Max, Math.Min

public class KontrolerGry1vsComputer : MonoBehaviour
{
    public int czyjRuch; // x=0, o=1
    public int licznikRuchow;
    public GameObject[] tablicaPodswietlenCzyjRuch;
    public Sprite[] obiektyXO; //x.png, o.png

    public Button[] polaPlanszy;
    public char[,] oznaczonePola = new char[3,3];
    public int[] zajetePola; // tablica intów z zajętymi polami, typy: -100, 1, 2

    public int wynikX; // zmienna używana w kompilatorze, później używana ToString
    public int wynikY; // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^  to samo co wyżej

    public Text wynikGraczaX; //obiekt w Unity, WynikX
    public Text wynikKomputeraY; // obiekt w Unity, wynikO

    public Sprite przyciskRewanzu; // przyciskRewanzu.png
    public Sprite przyciskNowejGry; // przyciskNowejGry.png
    public Sprite przyciskWyjscia; // przyciskWyjscia.png
    public Button[] przyciskiRewanz_NowaGra_Wyjscie; // tablica Buttonów(z Unity) Rewanżu, NowejGry, Wyjścia

    public Sprite pustyButton; // niewidzialny obrazek

    public GameObject[] wygranePodświetloneButtony; // tablica z wygranymi polami


    // -------------------------------------------------------------------------------- STRUKTURA RUCHU
    struct Ruch
    {
        public int row, col;
    };

    char player = 'x', opponent = 'o';

    // -------------------------------------------------------------------------------- CZY W LEWO
    bool czyWLewo(char[,] board)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == '_')
                    return true;
        return false;
    }

    // -------------------------------------------------------------------------------- SZACOWANIE
    int ocenianie(char[,] b)
    {
        for (int row = 0; row < 3; row++)
            if (b[row, 0] == b[row, 1] && b[row, 1] == b[row, 2])
            {
                if (b[row, 0] == 'x') return 1;
                else if (b[row, 0] == 'o') return -1;
            }

        for (int col = 0; col < 3; col++)
            if (b[0, col] == b[1, col] && b[1, col] == b[2, col])
            {
                if (b[0, col] == 'x') return 1;
                else if (b[0, col] == 'o') return -1;
            }

        if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == 'x') return 1;
            else if (b[0, 0] == 'o') return -1;
        }

        if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
            if (b[0, 2] == 'x') return 1;
            else if (b[0, 2] == 'o') return -1;
        }

        return 0;
    }

    // -------------------------------------------------------------------------------- MINMAX
    public int minimax(char[,] board, int depth, bool isMax)
    {
        int score = ocenianie(board);
        if (score == 1) return score;
        if (score == -1) return score;
        if (czyWLewo(board) == false) return 0;

        int best = isMax ? -1000 : 1000;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == '_')
                {
                    board[i, j] = isMax ? player : opponent;
                    best = isMax ? Math.Max(best, minimax(board, depth + 1, !isMax)) : Math.Min(best, minimax(board, depth + 1, !isMax));
                    board[i, j] = '_';
                }
        return best;
    }

    // -------------------------------------------------------------------------------- NAJLEPSZY RUCH
    Ruch najlepszyRuch(char[,] board)
    {
        int bestVal = -1000;
        Ruch bestMove;
        bestMove.row = -1;
        bestMove.col = -1;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == '_')
                {
                    board[i, j] = player;
                    int moveVal = minimax(board, 0, false);

                    board[i, j] = '_';

                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    }
                }
        return bestMove;
    }

    // -------------------------------------------------------------------------------- START
    void Start()
    {
        UruchomGre(0);
    }

    // -------------------------------------------------------------------------------- URUCHOM GRE
    void UruchomGre(int ruch)
    {
        czyjRuch = ruch;
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

        for (int i=0; i < polaPlanszy.Length; i++)
        {
            polaPlanszy[i].interactable = true;
            polaPlanszy[i].GetComponent<Image>().sprite = null;
        }

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                oznaczonePola[i, j] = '_';
            }
        }

        for (int i = 0; i < zajetePola.Length; i++)
        {
            zajetePola[i] = -100;
        }

        for (int i = 0; i < wygranePodświetloneButtony.Length; i++)
        {
            wygranePodświetloneButtony[i].SetActive(false);
        }

        if(czyjRuch == 1)
        {
            OperacjaPrzycisku(0);
        }
    }

    // -------------------------------------------------------------------------------- OPERACJA PRZYCISKU
    public void OperacjaPrzycisku(int numer)
    {
        if (czyjRuch == 0)
        {
            polaPlanszy[numer].image.sprite = obiektyXO[czyjRuch];
            polaPlanszy[numer].interactable = false;

            zajetePola[numer] = czyjRuch + 1;

            if (numer >= 0 && numer < 3)
            {
                oznaczonePola[0, numer] = player;
            }
            else if (numer >= 3 && numer < 6)
            {
                numer = numer - 3;
                oznaczonePola[1, numer] = player;
            }
            else if (numer >= 6 && numer < 9)
            {
                numer = numer - 6;
                oznaczonePola[2, numer] = player;
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
                    return;
                }
            }

            czyjRuch = 1;
            tablicaPodswietlenCzyjRuch[0].SetActive(false);
            tablicaPodswietlenCzyjRuch[1].SetActive(true);


            if (licznikRuchow < 9)
            {
                Ruch wspolrzedne = najlepszyRuch(oznaczonePola);

                oznaczonePola[wspolrzedne.row, wspolrzedne.col] = opponent;

                if (wspolrzedne.row == 0 && wspolrzedne.col == 0)
                    numer = 0;
                if (wspolrzedne.row == 0 && wspolrzedne.col == 1)
                    numer = 1;
                if (wspolrzedne.row == 0 && wspolrzedne.col == 2)
                    numer = 2;
                if (wspolrzedne.row == 1 && wspolrzedne.col == 0)
                    numer = 3;
                if (wspolrzedne.row == 1 && wspolrzedne.col == 1)
                    numer = 4;
                if (wspolrzedne.row == 1 && wspolrzedne.col == 2)
                    numer = 5;
                if (wspolrzedne.row == 2 && wspolrzedne.col == 0)
                    numer = 6;
                if (wspolrzedne.row == 2 && wspolrzedne.col == 1)
                    numer = 7;
                if (wspolrzedne.row == 2 && wspolrzedne.col == 2)
                    numer = 8;

                polaPlanszy[numer].image.sprite = obiektyXO[czyjRuch];
                polaPlanszy[numer].interactable = false;

                zajetePola[numer] = czyjRuch + 1;
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
                }

                czyjRuch = 0;
                tablicaPodswietlenCzyjRuch[0].SetActive(true);
                tablicaPodswietlenCzyjRuch[1].SetActive(false);
            }
        }

        else if (czyjRuch == 1)
        {
            Ruch wspolrzedne = najlepszyRuch(oznaczonePola);

            oznaczonePola[wspolrzedne.row, wspolrzedne.col] = opponent;

            if (wspolrzedne.row == 0 && wspolrzedne.col == 0)
                numer = 0;
            if (wspolrzedne.row == 0 && wspolrzedne.col == 1)
                numer = 1;
            if (wspolrzedne.row == 0 && wspolrzedne.col == 2)
                numer = 2;
            if (wspolrzedne.row == 1 && wspolrzedne.col == 0)
                numer = 3;
            if (wspolrzedne.row == 1 && wspolrzedne.col == 1)
                numer = 4;
            if (wspolrzedne.row == 1 && wspolrzedne.col == 2)
                numer = 5;
            if (wspolrzedne.row == 2 && wspolrzedne.col == 0)
                numer = 6;
            if (wspolrzedne.row == 2 && wspolrzedne.col == 1)
                numer = 7;
            if (wspolrzedne.row == 2 && wspolrzedne.col == 2)
                numer = 8;

            polaPlanszy[numer].image.sprite = obiektyXO[czyjRuch];
            polaPlanszy[numer].interactable = false;

            zajetePola[numer] = czyjRuch + 1;
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
            }

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
                switch (i)
                {
                    case 0:
                        wyswietlenieZwyciezcy(0, 1, 2);  // wywołanie funkcji wyświetlenia zwycięzcy, jeśli takowego znaleźliśmy
                        return true;
                    case 1:
                        wyswietlenieZwyciezcy(3, 4, 5);
                        return true;
                    case 2:
                        wyswietlenieZwyciezcy(6, 7, 8);
                        return true;
                    case 3:
                        wyswietlenieZwyciezcy(0, 3, 6);
                        return true;
                    case 4:
                        wyswietlenieZwyciezcy(1, 4, 7);
                        return true;
                    case 5:
                        wyswietlenieZwyciezcy(2, 5, 8);
                        return true;
                    case 6:
                        wyswietlenieZwyciezcy(0, 4, 8);
                        return true;
                    case 7:
                        wyswietlenieZwyciezcy(2, 4, 6);
                        return true;
                }
            }
        }
        return false;
    }

    void wyswietlenieZwyciezcy(int a, int b, int c)
    {
        for(int i=0; i<polaPlanszy.Length; i++)
        {
            if(polaPlanszy[i].interactable == true)
            {
                polaPlanszy[i].interactable = false;
                polaPlanszy[i].GetComponent<Image>().sprite = pustyButton;
            }
        }

        if (czyjRuch == 0)
        {
            wynikX++;
            wynikGraczaX.text = wynikX.ToString();
            czyjRuch = 1;
        }
        else if (czyjRuch == 1)
        {
            wynikY++;
            wynikKomputeraY.text = wynikY.ToString();
            czyjRuch = 0;
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
        wynikKomputeraY.text = "0";
        wynikX = 0;
        wynikY = 0;
        UruchomGre(0);
    }
    
    public void wyjscie()
    {
        SceneManager.LoadScene(0);
    }
}
