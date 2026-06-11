using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            for (int r = 0; r < VELICINA; r++)
                for (int k = 0; k < VELICINA; k++)
                    Polja[r, k] = StanjePolja.Prazno;
        }

        public bool MozeDaStane(int red, int kolona, int velicina, bool horizontalan)
        {
            for (int i = 0; i < velicina; i++)
            {
                int r = horizontalan ? red : red + i;
                int k = horizontalan ? kolona + i : kolona;

                if (r < 0 || r >= VELICINA || k < 0 || k >= VELICINA)
                    return false;

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

        public bool Gadaj(int red, int kolona)
        {
            if (Polja[red, kolona] == StanjePolja.Brod)
            {
                Polja[red, kolona] = StanjePolja.Pogodjeno;
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

        public bool SviBrodoPotopljeni()
        {
            foreach (Brod brod in Brodovi)
                if (!brod.JePotopljen())
                    return false;
            return true;
        }

        public bool JeVecGadano(int red, int kolona)
        {
            return Polja[red, kolona] == StanjePolja.Pogodjeno ||
                   Polja[red, kolona] == StanjePolja.Promaseno;
        }
    }

    // -------------------------------------------------------
    // Klasa za jedan zapis u leaderboard-u
    // -------------------------------------------------------
    public class LeaderboardZapis
    {
        public string Korisnik { get; set; }
        public int BrojPoteza { get; set; }
        public string Datum { get; set; }

        public LeaderboardZapis() { }

        public LeaderboardZapis(string korisnik, int brojPoteza)
        {
            Korisnik = korisnik;
            BrojPoteza = brojPoteza;
            Datum = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }
    }

    // -------------------------------------------------------
    // Pomocna klasa za citanje/pisanje leaderboard JSON fajla
    // Koristi samo System.IO i System.Text - nema eksternih biblioteka!
    // -------------------------------------------------------
    public static class LeaderboardManager
    {
        // Fajl se cuva pored .exe aplikacije
        private static readonly string PutanjaFajla = "leaderboard.json";

        public static List<LeaderboardZapis> UcitajSve()
        {
            try
            {
                if (!File.Exists(PutanjaFajla))
                    return new List<LeaderboardZapis>();

                string json = File.ReadAllText(PutanjaFajla, Encoding.UTF8);
                return ParsirajJson(json);
            }
            catch
            {
                return new List<LeaderboardZapis>();
            }
        }

        public static void SacuvajRezultat(string korisnik, int brojPoteza)
        {
            List<LeaderboardZapis> lista = UcitajSve();
            lista.Add(new LeaderboardZapis(korisnik, brojPoteza));

            // Sortiraj po broju poteza (manje poteza = bolje)
            lista.Sort((a, b) => a.BrojPoteza.CompareTo(b.BrojPoteza));

            // Cuva samo top 10
            if (lista.Count > 10)
                lista.RemoveRange(10, lista.Count - 10);

            string json = SerijaliserujJson(lista);
            File.WriteAllText(PutanjaFajla, json, Encoding.UTF8);
        }

        // Rucni mini JSON parser - bez eksternih biblioteka
        private static List<LeaderboardZapis> ParsirajJson(string json)
        {
            var lista = new List<LeaderboardZapis>();
            json = json.Trim();
            if (json.Length < 2) return lista;

            // Ocekuje format: [ {...}, {...} ]
            int i = json.IndexOf('{');
            while (i >= 0)
            {
                int kraj = json.IndexOf('}', i);
                if (kraj < 0) break;
                string obj = json.Substring(i, kraj - i + 1);
                var zapis = new LeaderboardZapis();
                zapis.Korisnik = IzvuciStringPolje(obj, "Korisnik");
                zapis.Datum = IzvuciStringPolje(obj, "Datum");
                string potezi = IzvuciStringPolje(obj, "BrojPoteza");
                int bp;
                int.TryParse(potezi, out bp);
                zapis.BrojPoteza = bp;
                lista.Add(zapis);
                i = json.IndexOf('{', kraj + 1);
            }
            return lista;
        }

        private static string IzvuciStringPolje(string obj, string kljuc)
        {
            // Trazi "Kljuc":"vrednost" ili "Kljuc":broj
            string token = "\"" + kljuc + "\":";
            int idx = obj.IndexOf(token);
            if (idx < 0) return "";
            int pocetak = idx + token.Length;
            if (pocetak >= obj.Length) return "";

            char prvi = obj[pocetak];
            if (prvi == '"')
            {
                int kraj = obj.IndexOf('"', pocetak + 1);
                if (kraj < 0) return "";
                return obj.Substring(pocetak + 1, kraj - pocetak - 1);
            }
            else
            {
                // Broj ili bool
                int kraj = pocetak;
                while (kraj < obj.Length && obj[kraj] != ',' && obj[kraj] != '}')
                    kraj++;
                return obj.Substring(pocetak, kraj - pocetak).Trim();
            }
        }

        private static string SerijaliserujJson(List<LeaderboardZapis> lista)
        {
            var sb = new StringBuilder();
            sb.Append("[\n");
            for (int i = 0; i < lista.Count; i++)
            {
                var z = lista[i];
                sb.Append("  {");
                sb.AppendFormat("\"Korisnik\":\"{0}\",", EskejpujJson(z.Korisnik));
                sb.AppendFormat("\"BrojPoteza\":{0},", z.BrojPoteza);
                sb.AppendFormat("\"Datum\":\"{0}\"", EskejpujJson(z.Datum));
                sb.Append("}");
                if (i < lista.Count - 1) sb.Append(",");
                sb.Append("\n");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static string EskejpujJson(string s)
        {
            if (s == null) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }

    // -------------------------------------------------------
    // Klasa za automatsko postavljanje brodova (racunar AI)
    // -------------------------------------------------------
    public class RacunarLogika
    {
        private Random rnd = new Random();

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

        private List<int[]> prioritetniPotezi = new List<int[]>();

        public int[] SledeciPotez(Tabla tablaProtivnika)
        {
            while (prioritetniPotezi.Count > 0)
            {
                int idx = rnd.Next(prioritetniPotezi.Count);
                int[] potez = prioritetniPotezi[idx];
                prioritetniPotezi.RemoveAt(idx);

                if (!tablaProtivnika.JeVecGadano(potez[0], potez[1]))
                    return potez;
            }

            int r, k;
            do
            {
                r = rnd.Next(Tabla.VELICINA);
                k = rnd.Next(Tabla.VELICINA);
            } while (tablaProtivnika.JeVecGadano(r, k));

            return new int[] { r, k };
        }

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