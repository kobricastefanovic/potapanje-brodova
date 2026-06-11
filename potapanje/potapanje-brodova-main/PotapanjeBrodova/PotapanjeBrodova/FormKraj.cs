using System;
using System.Drawing;
using System.Windows.Forms;

namespace PotapanjeBrodova
{
    // Forma koja se prikazuje na kraju igre
    public partial class FormKraj : Form
    {
        public FormKraj(bool pobeda, int brojPoteza)
        {
            InitializeComponent();

            this.Text = "Kraj igre";
            this.Size = new Size(420, 330);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 40, 70);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Ako je igrac pobedio, sacuvaj rezultat
            if (pobeda)
                LeaderboardManager.SacuvajRezultat(Program.KorisnickoIme, brojPoteza);

            // Veliki rezultat
            Label lblRezultat = new Label();
            lblRezultat.Font = new Font("Arial", 28, FontStyle.Bold);
            lblRezultat.TextAlign = ContentAlignment.MiddleCenter;
            lblRezultat.Size = new Size(400, 80);
            lblRezultat.Location = new Point(10, 20);

            if (pobeda)
            {
                lblRezultat.Text = "POBEDA!";
                lblRezultat.ForeColor = Color.FromArgb(100, 255, 100);
            }
            else
            {
                lblRezultat.Text = "PORAZ!";
                lblRezultat.ForeColor = Color.FromArgb(255, 100, 100);
            }
            this.Controls.Add(lblRezultat);

            // Ime igraca
            Label lblIme = new Label();
            lblIme.Font = new Font("Arial", 11);
            lblIme.ForeColor = Color.FromArgb(100, 200, 255);
            lblIme.TextAlign = ContentAlignment.MiddleCenter;
            lblIme.Size = new Size(400, 25);
            lblIme.Location = new Point(10, 100);
            lblIme.Text = string.Format("Igrac: {0}", Program.KorisnickoIme);
            this.Controls.Add(lblIme);

            // Opis
            Label lblOpis = new Label();
            lblOpis.Font = new Font("Arial", 11);
            lblOpis.ForeColor = Color.LightGray;
            lblOpis.TextAlign = ContentAlignment.MiddleCenter;
            lblOpis.Size = new Size(400, 30);
            lblOpis.Location = new Point(10, 128);
            lblOpis.Text = pobeda
                ? "Potopili ste svu flotu protivnika!"
                : "Racunar je potopao svu vasu flotu!";
            this.Controls.Add(lblOpis);

            // Broj poteza
            Label lblPotezi = new Label();
            lblPotezi.Font = new Font("Arial", 11, FontStyle.Bold);
            lblPotezi.ForeColor = Color.Yellow;
            lblPotezi.TextAlign = ContentAlignment.MiddleCenter;
            lblPotezi.Size = new Size(400, 25);
            lblPotezi.Location = new Point(10, 158);
            lblPotezi.Text = string.Format("Ukupan broj poteza: {0}", brojPoteza);
            this.Controls.Add(lblPotezi);

            // Poruka o leaderboard-u (samo ako je pobeda)
            if (pobeda)
            {
                Label lblLb = new Label();
                lblLb.Font = new Font("Arial", 9);
                lblLb.ForeColor = Color.FromArgb(255, 215, 0);
                lblLb.TextAlign = ContentAlignment.MiddleCenter;
                lblLb.Size = new Size(400, 20);
                lblLb.Location = new Point(10, 182);
                lblLb.Text = "Rezultat sacuvan u leaderboard!";
                this.Controls.Add(lblLb);
            }

            // Dugme nova igra
            Button btnNova = new Button();
            btnNova.Text = "NOVA IGRA";
            btnNova.Font = new Font("Arial", 10, FontStyle.Bold);
            btnNova.Size = new Size(120, 38);
            btnNova.Location = new Point(15, 250);
            btnNova.BackColor = Color.FromArgb(50, 140, 50);
            btnNova.ForeColor = Color.White;
            btnNova.FlatStyle = FlatStyle.Flat;
            btnNova.Click += delegate
            {
                Form1 novaForma = new Form1();
                novaForma.Show();
                this.Close();
            };
            this.Controls.Add(btnNova);
            Button btnLeader = new Button();
            btnLeader.Text = "LEADERBOARD";
            btnLeader.Font = new Font("Arial", 10, FontStyle.Bold);
            btnLeader.Size = new Size(140, 38);
            btnLeader.Location = new Point(145, 250);
            btnLeader.BackColor = Color.FromArgb(80, 60, 140);
            btnLeader.ForeColor = Color.White;
            btnLeader.FlatStyle = FlatStyle.Flat;
            btnLeader.Click += delegate
            {
                using (FormLeaderboard fl = new FormLeaderboard())
                    fl.ShowDialog();
            };
            this.Controls.Add(btnLeader);

            // Dugme izlaz
            Button btnIzlaz = new Button();
            btnIzlaz.Text = "IZLAZ";
            btnIzlaz.Font = new Font("Arial", 10, FontStyle.Bold);
            btnIzlaz.Size = new Size(100, 38);
            btnIzlaz.Location = new Point(295, 250);
            btnIzlaz.BackColor = Color.FromArgb(140, 50, 50);
            btnIzlaz.ForeColor = Color.White;
            btnIzlaz.FlatStyle = FlatStyle.Flat;
            btnIzlaz.Click += delegate { Application.Exit(); };
            this.Controls.Add(btnIzlaz);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}