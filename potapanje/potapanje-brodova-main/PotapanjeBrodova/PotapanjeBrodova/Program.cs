using System;
using System.Windows.Forms;

namespace PotapanjeBrodova
{
    static class Program
    {
        private static string _korisnickoIme = "Igrac";
        public static string KorisnickoIme
        {
            get { return _korisnickoIme; }
            private set { _korisnickoIme = value; }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (FormUnosImena formIme = new FormUnosImena())
            {
                if (formIme.ShowDialog() == DialogResult.OK)
                    KorisnickoIme = formIme.KorisnickoIme;
                else
                    return;
            }

            Application.Run(new Form1());
        }
    }
}