using System;
using System.Collections.Generic;

namespace PotapanjeBrodova
{
    // Moguca stanja jednog polja na tabli
    public enum StanjePolja
    {
        Prazno,       // Nije gadano, nema broda
        Brod,         // Tu je brod, nije gadano
        Pogodjeno,    // Gadano i pogodjen brod
        Promaseno     // Gadano, ali nije brod
    }

    // Jedan brod sa pozicijom i velicinom
    public class Brod
    {
        public int Red { get; set; }
        public int Kolona { get; set; }
        public int Velicina { get; set; }
        public bool JeHorizontalan { get; set; }
        public int BrojPogodaka { get; set; }

        public Brod(int red, int kolona, int velicina, bool horizontalan)
        {
            Red = red;
            Kolona = kolona;
            Velicina = velicina;
            JeHorizontalan = horizontalan;
            BrojPogodaka = 0;
        }

        // Da li je brod potopljen
        public bool JePotopljen()
        {
            return BrojPogodaka >= Velicina;
        }
    }

    // Tabla 10x10 sa svim brodovima i stanjima polja
    public class Tabla
    {
        public const int VELICINA = 10;
        public StanjePolja[,] Polja { get; private set; }
        public List<Brod> Brodovi { get; private set; }

        public Tabla()
        {
            Polja = new StanjePolja[VELICINA, VELICINA];
            Brodovi = new List<Brod>();

            // Inicijalizacija svih polja na prazno
            for (int r = 0; r < VELICINA; r++)
                for (int k = 0; k < VELICINA; k++)
                    Polja[r, k] = StanjePolja.Prazno;
        }

        // Proverava da li brod moze biti postavljen na datoj poziciji
        public bool MozeDaStane(int red, int kolona, int velicina, bool horizontalan)
        {
            for (int i = 0; i < velicina; i++)
            {
                int r = horizontalan ? red : red + i;
                int k = horizontalan ? kolona + i : kolona;

                if (r < 0 || r >= VELICINA || k < 0 || k >= VELICINA)
                    return false;

                // Proverava i susedna polja da brodovi ne bi dodirivali
                for (int dr = -1; dr <= 1; dr++)
                    for (int dk = -1; dk <= 1; dk++)
                    {
                        int nr = r + dr;
                        int nk = k + dk;
                        if (nr >= 0 && nr < VELICINA && nk >= 0 && nk < VELICINA)
                            if (Polja[nr, nk] == StanjePolja.Brod)
                                return false;
                    }
            }
            return true;
        }

        // Postavlja brod na tablu
        public bool PostaviBrod(int red, int kolona, int velicina, bool horizontalan)
        {
            if (!MozeDaStane(red, kolona, velicina, horizontalan))
                return false;

            Brod noviBrod = new Brod(red, kolona, velicina, horizontalan);
            Brodovi.Add(noviBrod);

            for (int i = 0; i < velicina; i++)
            {
                int r = horizontalan ? red : red + i;
                int k = horizontalan ? kolona + i : kolona;
                Polja[r, k] = StanjePolja.Brod;
            }
            return true;
        }

        // Gadanje polja - vraca true ako je pogodjen brod
        public bool Gadaj(int red, int kolona)
        {
            if (Polja[red, kolona] == StanjePolja.Brod)
            {
                Polja[red, kolona] = StanjePolja.Pogodjeno;

                // Pronadji koji brod je pogodjen i povecaj broj pogodaka
                foreach (Brod brod in Brodovi)
                {
                    for (int i = 0; i < brod.Velicina; i++)
                    {
                        int r = brod.JeHorizontalan ? brod.Red : brod.Red + i;
                        int k = brod.JeHorizontalan ? brod.Kolona + i : brod.Kolona;
                        if (r == red && k == kolona)
                        {
                            brod.BrojPogodaka++;
                            return true;
                        }
                    }
                }
            }
            else if (Polja[red, kolona] == StanjePolja.Prazno)
            {
                Polja[red, kolona] = StanjePolja.Promaseno;
            }
            return false;
        }

        // Proverava da li su svi brodovi potopljeni
        public bool SviBrodoPotopljeni()
        {
            foreach (Brod brod in Brodovi)
                if (!brod.JePotopljen())
                    return false;
            return true;
        }

        // Da li je polje vec gadano
        public bool JeVecGadano(int red, int kolona)
        {
            return Polja[red, kolona] == StanjePolja.Pogodjeno ||
                   Polja[red, kolona] == StanjePolja.Promaseno;
        }
    }

    // Klasa za automatsko postavljanje brodova (racunar)
    public class RacunarLogika
    {
        private Random rnd = new Random();

        // Automatski postavi sve brodove nasumicno
        public void AutoPostavljanje(Tabla tabla, int[] velicine)
        {
            foreach (int vel in velicine)
            {
                bool postavljen = false;
                while (!postavljen)
                {
                    int red = rnd.Next(Tabla.VELICINA);
                    int kolona = rnd.Next(Tabla.VELICINA);
                    bool horizontalan = rnd.Next(2) == 0;
                    postavljen = tabla.PostaviBrod(red, kolona, vel, horizontalan);
                }
            }
        }

        // Racunar odabire polje za gadanje (pametan AI)
        private List<int[]> listaPoteza = new List<int[]>();
        private List<int[]> prioritetniPotezi = new List<int[]>();

        public int[] SledeciPotez(Tabla tablaProtivnika)
        {
            // Ako ima prioritetnih polja (oko pogodaka), gada njih
            while (prioritetniPotezi.Count > 0)
            {
                int idx = rnd.Next(prioritetniPotezi.Count);
                int[] potez = prioritetniPotezi[idx];
                prioritetniPotezi.RemoveAt(idx);

                if (!tablaProtivnika.JeVecGadano(potez[0], potez[1]))
                    return potez;
            }

            // Inace gada nasumicno
            int r, k;
            do
            {
                r = rnd.Next(Tabla.VELICINA);
                k = rnd.Next(Tabla.VELICINA);
            } while (tablaProtivnika.JeVecGadano(r, k));

            return new int[] { r, k };
        }

        // Nakon pogotka, dodaj susedna polja kao prioritet
        public void DodajPrioritete(int red, int kolona)
        {
            int[][] susedi = new int[][]
            {
                new int[]{red-1, kolona},
                new int[]{red+1, kolona},
                new int[]{red, kolona-1},
                new int[]{red, kolona+1}
            };

            foreach (int[] s in susedi)
            {
                if (s[0] >= 0 && s[0] < Tabla.VELICINA &&
                    s[1] >= 0 && s[1] < Tabla.VELICINA)
                    prioritetniPotezi.Add(s);
            }
        }
    }
}
