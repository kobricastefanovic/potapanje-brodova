using System;
using System.Drawing;
using System.Windows.Forms;

namespace PotapanjeBrodova
{
    // Forma za unos korisnickog imena pre pocetka igre
    public class FormUnosImena : Form
    {
        private TextBox txtIme;
        private string _korisnickoIme = "Igrac";
        public string KorisnickoIme
        {
            get { return _korisnickoIme; }
            private set { _korisnickoIme = value; }
        }

        public FormUnosImena()
        {
            this.Text = "Potapanje brodova";
            this.Size = new Size(380, 220);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 40, 70);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblNaslov = new Label();
            lblNaslov.Text = "POTAPANJE BRODOVA";
            lblNaslov.Font = new Font("Arial", 16, FontStyle.Bold);
            lblNaslov.ForeColor = Color.FromArgb(100, 200, 255);
            lblNaslov.Location = new Point(50, 20);
            lblNaslov.AutoSize = true;
            this.Controls.Add(lblNaslov);

            Label lblPitanje = new Label();
            lblPitanje.Text = "Unesite vase korisnicko ime:";
            lblPitanje.Font = new Font("Arial", 10);
            lblPitanje.ForeColor = Color.LightGray;
            lblPitanje.Location = new Point(30, 70);
            lblPitanje.AutoSize = true;
            this.Controls.Add(lblPitanje);

            txtIme = new TextBox();
            txtIme.Font = new Font("Arial", 12);
            txtIme.Location = new Point(30, 95);
            txtIme.Size = new Size(300, 30);
            txtIme.BackColor = Color.FromArgb(30, 70, 120);
            txtIme.ForeColor = Color.White;
            txtIme.BorderStyle = BorderStyle.FixedSingle;
            txtIme.MaxLength = 20;
            txtIme.KeyDown += TxtIme_KeyDown;
            this.Controls.Add(txtIme);

            Button btnOK = new Button();
            btnOK.Text = "IGRAJ";
            btnOK.Font = new Font("Arial", 11, FontStyle.Bold);
            btnOK.Size = new Size(130, 38);
            btnOK.Location = new Point(30, 140);
            btnOK.BackColor = Color.FromArgb(50, 140, 50);
            btnOK.ForeColor = Color.White;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            Button btnLeaderboard = new Button();
            btnLeaderboard.Text = "LEADERBOARD";
            btnLeaderboard.Font = new Font("Arial", 11, FontStyle.Bold);
            btnLeaderboard.Size = new Size(150, 38);
            btnLeaderboard.Location = new Point(180, 140);
            btnLeaderboard.BackColor = Color.FromArgb(80, 60, 140);
            btnLeaderboard.ForeColor = Color.White;
            btnLeaderboard.FlatStyle = FlatStyle.Flat;
            btnLeaderboard.Click += BtnLeaderboard_Click;
            this.Controls.Add(btnLeaderboard);

            this.AcceptButton = btnOK;
            txtIme.Focus();
        }

        private void TxtIme_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PotvrdiIme();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            PotvrdiIme();
        }

        private void PotvrdiIme()
        {
            string ime = txtIme.Text.Trim();
            if (string.IsNullOrEmpty(ime))
            {
                MessageBox.Show("Unesite korisnicko ime!", "Upozorenje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            KorisnickoIme = ime;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnLeaderboard_Click(object sender, EventArgs e)
        {
            using (FormLeaderboard fl = new FormLeaderboard())
                fl.ShowDialog();
        }
    }
}