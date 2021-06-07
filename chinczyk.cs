using System; 

namespace Chinczyk
{
    class Gra{
        public static void Main()
        {
            StanGry chinczyk = new StanGry();
            chinczyk.Reset();
            chinczyk.plansza.Wypisz();
        }
    }
    class Gracz
    {
        public string kolor;
        public PoleStartowe poleStartowe;
        public PoleKoncowe poleKoncowe;

        public Gracz(string k)
        {
            kolor = k;
            poleStartowe = new PoleStartowe();
            poleKoncowe = new PoleKoncowe();
        }

    }
    class Pionek
    {
        public Gracz gracz;
        public int numerPionka;

        public Pionek(Gracz g, int n)
        {
            gracz = g;
            numerPionka = n;
        }
    }
	 
	class Pole
    {  
        public Pole nastepnePole;
        public Pionek pionek;
        public Pole()
        {
            nastepnePole = null;
            pionek = null;
        }
	} 

    class PoleStartowe : Pole
    {
        private int pionkiNaStarcie;

        public PoleStartowe()
        {
            pionkiNaStarcie = 4;
            nastepnePole = null;
            pionek = null;
        }
    }

    class PoleKoncowe : Pole
    {
        private int pionkiNaKoncu;

        public PoleKoncowe()
        {
            pionkiNaKoncu = 0;
            nastepnePole = null;
            pionek = null;
        }
    }

    class Plansza
    {
        private Pole polePoczatkowe;

        public Plansza(Pole pp)
        {
            polePoczatkowe = pp;
        }
        public void Wypisz()
        {
            Pole aktualnePole = polePoczatkowe;

            for(int i = 0; i <= 40; i++)
            {
                if(aktualnePole.pionek != null)
                {
                    System.Console.Write(aktualnePole.pionek.numerPionka);
                    System.Console.Write(" ");
                }
                else
                {
                    System.Console.Write("X ");
                }

                aktualnePole = aktualnePole.nastepnePole;
            }

            System.Console.WriteLine();
        }
    }

    class StanGry
    {
        public Plansza plansza;
        private Gracz[] gracze;
        private int indeksGracza;

        private int RzutKostka()
        {
            Random k = new Random();
            return k.Next(1,6);
        }
        public void WykonajRuch()
        {
            
        }
        public void Reset()
        {

            //tbd: Wczytaj liczbę graczy
            
            Gracz[] g = {new Gracz("niebieski"), 
                         new Gracz("czerwony"), 
                         new Gracz("zielony"),
                         new Gracz("żółty")};
            gracze = g;

            indeksGracza = 0;

            Pole aktualnePole = gracze[0].poleStartowe;


            for(int i = 0; i < 40; i++)
            {
                if(i == 8)
                {
                    aktualnePole.nastepnePole = gracze[1].poleKoncowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[1], 1);
                }
                else if(i == 9)
                {
                    aktualnePole.nastepnePole = gracze[1].poleStartowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[1], 2);    
                }
                else if(i == 18)
                {
                    aktualnePole.nastepnePole = gracze[2].poleKoncowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[2], 1);
                }
                else if(i == 19)
                {
                    aktualnePole.nastepnePole = gracze[2].poleStartowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[2], 2);
                }
                else if(i == 28)
                {
                    aktualnePole.nastepnePole = gracze[3].poleKoncowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[3], 1);
                }
                else if(i == 29)
                {
                    aktualnePole.nastepnePole = gracze[3].poleStartowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[3], 2);
                }
                else if(i == 38)
                {
                    aktualnePole.nastepnePole = gracze[0].poleKoncowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[0], 1);
                }
                else if(i == 39)
                {
                    aktualnePole.nastepnePole = gracze[0].poleStartowe;
                    aktualnePole.nastepnePole.pionek = new Pionek(gracze[0], 2);
                }
                else
                {
                    aktualnePole.nastepnePole = new Pole();
                    aktualnePole.nastepnePole.pionek = null;
                }

                aktualnePole = aktualnePole.nastepnePole;
            }

            plansza = new Plansza(gracze[0].poleStartowe);

        }
    }
} 