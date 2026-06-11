using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PotapanjeBrodova
{
    // Forma koja prikazuje top 10 rezultata
    public class FormLeaderboard : Form
    {
        public FormLeaderboard()
        {
            this.Text = "Leaderboard - Top 10";
            this.Size = new Size(460, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 40, 70);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblNaslov = new Label();
            lblNaslov.Text = "LEADERBOARD - TOP 10";
            lblNaslov.Font = new Font("Arial", 14, FontStyle.Bold);
            lblNaslov.ForeColor = Color.FromArgb(255, 215, 0);
            lblNaslov.Location = new Point(100, 15);
            lblNaslov.AutoSize = true;
            this.Controls.Add(lblNaslov);

            Label lblOpis = new Label();
            lblOpis.Text = "Pobednici sortirani po broju poteza (manje = bolje)";
            lblOpis.Font = new Font("Arial", 8);
            lblOpis.ForeColor = Color.LightGray;
            lblOpis.Location = new Point(70, 45);
            lblOpis.AutoSize = true;
            this.Controls.Add(lblOpis);

            // Zaglavlje tabele
            Panel header = new Panel();
            header.Location = new Point(20, 65);
            header.Size = new Size(410, 28);
            header.BackColor = Color.FromArgb(30, 80, 140);
            this.Controls.Add(header);

            Label hRank = new Label(); hRank.Text = "#"; hRank.ForeColor = Color.White;
            hRank.Font = new Font("Arial", 9, FontStyle.Bold);
            hRank.Location = new Point(5, 5); hRank.AutoSize = true;
            header.Controls.Add(hRank);

            Label hIme = new Label(); hIme.Text = "Korisnik"; hIme.ForeColor = Color.White;
            hIme.Font = new Font("Arial", 9, FontStyle.Bold);
            hIme.Location = new Point(40, 5); hIme.AutoSize = true;
            header.Controls.Add(hIme);

            Label hPotezi = new Label(); hPotezi.Text = "Potezi"; hPotezi.ForeColor = Color.White;
            hPotezi.Font = new Font("Arial", 9, FontStyle.Bold);
            hPotezi.Location = new Point(210, 5); hPotezi.AutoSize = true;
            header.Controls.Add(hPotezi);

            Label hDatum = new Label(); hDatum.Text = "Datum"; hDatum.ForeColor = Color.White;
            hDatum.Font = new Font("Arial", 9, FontStyle.Bold);
            hDatum.Location = new Point(275, 5); hDatum.AutoSize = true;
            header.Controls.Add(hDatum);

            // Ucitaj rezultate
            List<LeaderboardZapis> lista = LeaderboardManager.UcitajSve();

            if (lista.Count == 0)
            {
                Label lblPrazno = new Label();
                lblPrazno.Text = "Jos nema rezultata. Igrajte i pobedite!";
                lblPrazno.Font = new Font("Arial", 10);
                lblPrazno.ForeColor = Color.LightGray;
                lblPrazno.Location = new Point(60, 130);
                lblPrazno.AutoSize = true;
                this.Controls.Add(lblPrazno);
            }
            else
            {
                // Boje za top 3
                Color[] boje = {
                    Color.FromArgb(255, 215, 0),   // Gold
                    Color.FromArgb(192, 192, 192), // Silver
                    Color.FromArgb(205, 127, 50),  // Bronze
                };

                for (int i = 0; i < lista.Count; i++)
                {
                    int y = 95 + i * 26;
                    Color bojaReda = (i % 2 == 0)
                        ? Color.FromArgb(20, 55, 95)
                        : Color.FromArgb(18, 48, 82);

                    Panel red = new Panel();
                    red.Location = new Point(20, y);
                    red.Size = new Size(410, 24);
                    red.BackColor = bojaReda;
                    this.Controls.Add(red);

                    Color bojaFonta = (i < 3) ? boje[i] : Color.LightGray;
                    FontStyle stil = (i < 3) ? FontStyle.Bold : FontStyle.Regular;

                    Label lRank = new Label();
                    lRank.Text = (i + 1).ToString() + ".";
                    lRank.ForeColor = bojaFonta;
                    lRank.Font = new Font("Arial", 9, stil);
                    lRank.Location = new Point(5, 4);
                    lRank.Size = new Size(30, 16);
                    red.Controls.Add(lRank);

                    Label lIme = new Label();
                    lIme.Text = lista[i].Korisnik;
                    lIme.ForeColor = bojaFonta;
                    lIme.Font = new Font("Arial", 9, stil);
                    lIme.Location = new Point(40, 4);
                    lIme.Size = new Size(165, 16);
                    red.Controls.Add(lIme);

                    Label lPotezi = new Label();
                    lPotezi.Text = lista[i].BrojPoteza.ToString();
                    lPotezi.ForeColor = bojaFonta;
                    lPotezi.Font = new Font("Arial", 9, stil);
                    lPotezi.Location = new Point(210, 4);
                    lPotezi.Size = new Size(55, 16);
                    red.Controls.Add(lPotezi);

                    Label lDatum = new Label();
                    lDatum.Text = lista[i].Datum;
                    lDatum.ForeColor = Color.Gray;
                    lDatum.Font = new Font("Arial", 8);
                    lDatum.Location = new Point(275, 5);
                    lDatum.Size = new Size(125, 16);
                    red.Controls.Add(lDatum);
                }
            }

            Button btnZatvori = new Button();
            btnZatvori.Text = "ZATVORI";
            btnZatvori.Font = new Font("Arial", 10, FontStyle.Bold);
            btnZatvori.Size = new Size(130, 36);
            btnZatvori.Location = new Point(160, 360);
            btnZatvori.BackColor = Color.FromArgb(30, 80, 140);
            btnZatvori.ForeColor = Color.White;
            btnZatvori.FlatStyle = FlatStyle.Flat;
            btnZatvori.Click += delegate { this.Close(); };
            this.Controls.Add(btnZatvori);
        }
    }
}