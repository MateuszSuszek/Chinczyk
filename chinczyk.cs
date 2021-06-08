using System; 

namespace Chinczyk
{
    class Gra{
        public static void Main()
        {
            StanGry chinczyk = new StanGry();
            chinczyk.Reset();

            while(!chinczyk.koniec)
            {
                chinczyk.WypiszStanGry();
                chinczyk.WykonajRuch();
            }
            
        }

    }

    class Gracz
    {
        public string kolor;
        public ConsoleColor kolorKonsoli;
        public PoleStartowe poleStartowe;
        public PoleKoncowe poleKoncowe;

        public Gracz(string k, ConsoleColor kk)
        {
            kolor = k;
            kolorKonsoli = kk;
            poleStartowe = new PoleStartowe(this);
            poleKoncowe = new PoleKoncowe(this);
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
        public ConsoleColor kolorPola;
        public Pole()
        {
            nastepnePole = null;
            pionek = null;
            kolorPola = ConsoleColor.Gray;
        }
	} 

    class PoleStartowe : Pole
    {
        public int pionkiNaStarcie;
        public Pionek[] pionki;

        public PoleStartowe(Gracz g)
        {
            pionkiNaStarcie = 4;
            nastepnePole = null;
            pionek = null;
            kolorPola = g.kolorKonsoli;
            Pionek[] p = {new Pionek(g, 1), new Pionek(g, 2), new Pionek(g, 3), new Pionek(g, 4)};
            pionki = p;
        }
    }

    class PoleKoncowe : Pole
    {
        public int pionkiNaKoncu;
        public Pionek[] pionki;

        public PoleKoncowe(Gracz g)
        {
            pionkiNaKoncu = 0;
            nastepnePole = null;
            pionek =  null;
            kolorPola = g.kolorKonsoli;
            Pionek[] p = {null, null, null, null};
            pionki = p;
        }
    }

    class Plansza
    {
        public Pole polePoczatkowe;

        public Plansza(Pole pp)
        {
            polePoczatkowe = pp;
        }
        public void WypiszPlansze()
        {
            Pole aktualnePole = polePoczatkowe;

            for(int i = 0; i < 40; i++)
            {
                Console.ForegroundColor = aktualnePole.kolorPola;
                System.Console.Write("X ");
                Console.ResetColor();

                aktualnePole = aktualnePole.nastepnePole;
            }

            System.Console.WriteLine();

            aktualnePole = polePoczatkowe;

            for(int i = 0; i < 40; i++)
            {
                if(aktualnePole.pionek != null)
                {
                    Console.ForegroundColor = aktualnePole.pionek.gracz.kolorKonsoli;
                    System.Console.Write(aktualnePole.pionek.numerPionka);
                    System.Console.Write(" ");
                    Console.ResetColor();
                }
                else
                {
                    System.Console.Write("O ");
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
        private int numerRuchu; // który raz gracz wykonuje kolejny ruch po wyrzuceniu 6, maksymalnie 3 razy
        public bool koniec;

        private int RzutKostka()
        {
            Random k = new Random();
            return k.Next(1,7);
        }

        public void WypiszStanGry()
        {
            plansza.WypiszPlansze();

            System.Console.WriteLine();
            
            for(int i = 0; i < 4; i++)
            {
                Console.ForegroundColor = gracze[i].kolorKonsoli;
                System.Console.WriteLine("Gracz {0}, {1}", i + 1, gracze[i].kolor);
                System.Console.Write("Pionki na starcie: ");
                for(int j = 0; j < 4; j++)
                {
                    if(gracze[i].poleStartowe.pionki[j] != null)
                    {
                        System.Console.Write("{0} ", j+1);
                    }
                }
                System.Console.WriteLine();
                System.Console.WriteLine("Liczba pionków na końcu: {0}", gracze[i].poleKoncowe.pionkiNaKoncu);
                System.Console.WriteLine();
                Console.ResetColor();
            }
        }
        public void WykonajRuch()
        {

            Console.WriteLine("Ruch gracza {0}", indeksGracza + 1);
            
            int rzut = RzutKostka();

            

            Console.WriteLine("Wyrzucono {0}", rzut);

            string command = "";

            Gracz akg = gracze[indeksGracza];
            PoleStartowe ps = akg.poleStartowe;
            PoleKoncowe pk = akg.poleKoncowe;

            bool done = false;

            while(!done)
            {
                while(command != "1" && command != "2" && command != "3" && command != "4")
                {
                    command = Console.ReadLine();

                    if(command == "exit" || command == "quit" || command == "q")
                    {
                        koniec = true;
                        return;
                    }

                    if(command == "reset" || command == "r")
                    {
                        Reset();
                        return;
                    }
                }

                int numer = Convert.ToInt32(command);

                if((ps.pionkiNaStarcie + pk.pionkiNaKoncu) == 4 && (rzut != 6 || ps.pionek != null))
                {
                    // Brak dostępnego ruchu
                    done = true;
                }
                else if(ps.pionki[numer-1] != null && pk.pionki[numer-1] == null && rzut == 6 && ps.pionek == null)
                {
                    ps.pionkiNaStarcie--;
                    ps.pionek = ps.pionki[numer-1];
                    ps.pionki[numer-1] = null;
                    done = true;
                }
                else if(ps.pionki[numer-1] == null)
                {
                    // Znajduję pole, na którym znajduje się przesuwany pionek
                    Pole akp = plansza.polePoczatkowe;
                    while(akp.pionek == null || akp.pionek.gracz != akg || akp.pionek.numerPionka != numer)
                    {
                        akp = akp.nastepnePole;
                    }

                    Pionek p = akp.pionek;
                    Pole polePionka = akp;

                    // Przesunięcie

                    int dystans = rzut;

                    while(dystans != 0 && akp != pk)
                    {
                        akp = akp.nastepnePole;
                        dystans--;
                    }

                    if(akp.pionek != null && akp.pionek.gracz != akg) // Zbicie pionka
                    {
                        akp.pionek.gracz.poleStartowe.pionki[akp.pionek.numerPionka-1] = akp.pionek;
                        akp.pionek.gracz.poleStartowe.pionkiNaStarcie++;
                        akp.pionek = p;
                        polePionka.pionek = null;
                        done = true;
                    }
                    else if(akp.pionek != null && akp.pionek.gracz == akg)
                    {
                        // Nie przesuwaj, ruch nie jest dozwolony
                    }
                    else
                    {
                        akp.pionek = p;
                        polePionka.pionek = null;
                        done = true;
                    }

                    if(akp == pk)
                    {
                        pk.pionkiNaKoncu++;
                        pk.pionki[numer-1] = p;
                        akp.pionek = null;
                    }
                }

                command = "";
            }

            if(pk.pionkiNaKoncu == 4)
            {
                koniec = true;
            }

            if(rzut == 6 && numerRuchu != 3)
            {
                numerRuchu++;
            }else
            {
                numerRuchu = 1;
                indeksGracza = (indeksGracza + 1) % 4;
            }

        }
        public void Reset()
        {

            //tbd: Wczytaj liczbę graczy
            
            Gracz[] g = {new Gracz("niebieski", ConsoleColor.Blue), 
                         new Gracz("czerwony", ConsoleColor.Red), 
                         new Gracz("zielony", ConsoleColor.Green),
                         new Gracz("żółty", ConsoleColor.Yellow)};
            gracze = g;

            indeksGracza = 0;

            numerRuchu = 1;

            koniec = false;

            Pole aktualnePole = gracze[0].poleStartowe;

            for(int i = 0; i < 40; i++)
            {
                if(i == 8)
                {
                    aktualnePole.nastepnePole = gracze[1].poleKoncowe;
                }
                else if(i == 9)
                {
                    aktualnePole.nastepnePole = gracze[1].poleStartowe;    
                }
                else if(i == 18)
                {
                    aktualnePole.nastepnePole = gracze[2].poleKoncowe;
                }
                else if(i == 19)
                {
                    aktualnePole.nastepnePole = gracze[2].poleStartowe;
                }
                else if(i == 28)
                {
                    aktualnePole.nastepnePole = gracze[3].poleKoncowe;
                }
                else if(i == 29)
                {
                    aktualnePole.nastepnePole = gracze[3].poleStartowe;
                }
                else if(i == 38)
                {
                    aktualnePole.nastepnePole = gracze[0].poleKoncowe;
                }
                else if(i == 39)
                {
                    aktualnePole.nastepnePole = gracze[0].poleStartowe;
                }
                else
                {
                    aktualnePole.nastepnePole = new Pole();
                }

                aktualnePole.nastepnePole.pionek = null;
                aktualnePole = aktualnePole.nastepnePole;
            }

            plansza = new Plansza(gracze[0].poleStartowe);

        }
    }
} 