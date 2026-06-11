using System;
using System.Drawing;
using System.Windows.Forms;

namespace PotapanjeBrodova
{
    // Glavna forma tokom igre - prikazuje dve table i omogucava gadanje
    public partial class FormIgra : Form
    {
        private const int VELICINA_CELIJE = 38;
        private const int OFFSET_X_LEVO = 20;
        private const int OFFSET_X_DESNO = 440;
        private const int OFFSET_Y = 90;

        private Tabla tablaIgraca;
        private Tabla tablaRacunara;
        private RacunarLogika racunar;
        private int[] velicine;

        private bool jeIgracNaPotezu = true;
        private int potezBrojac = 0;

        private Panel panelIgrac;
        private Panel panelRacunar;
        private Label lblStatus;
        private Label lblBrojPoteza;
        private Label lblBrodovi;

        public FormIgra(Tabla tablaIgraca, int[] velicine)
        {
            this.tablaIgraca = tablaIgraca;
            this.velicine = velicine;

            // Racunar postavlja brodove automatski
            tablaRacunara = new Tabla();
            racunar = new RacunarLogika();
            racunar.AutoPostavljanje(tablaRacunara, velicine);

            InitializeComponent();
            InicijalizujUI();
            AzurirajPoruke();
        }

        private void InicijalizujUI()
        {
            this.Text = "Potapanje brodova - Igra";
            this.Size = new Size(850, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 40, 70);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Naslov
            Label lblNaslov = new Label();
            lblNaslov.Text = "BITKA NA MORU";
            lblNaslov.Font = new Font("Arial", 16, FontStyle.Bold);
            lblNaslov.ForeColor = Color.FromArgb(100, 200, 255);
            lblNaslov.Location = new Point(320, 10);
            lblNaslov.AutoSize = true;
            this.Controls.Add(lblNaslov);

            // Status poruka
            lblStatus = new Label();
            lblStatus.Font = new Font("Arial", 10, FontStyle.Bold);
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.Location = new Point(20, 40);
            lblStatus.Size = new Size(800, 20);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblStatus);

            // Broj poteza
            lblBrojPoteza = new Label();
            lblBrojPoteza.Font = new Font("Arial", 9);
            lblBrojPoteza.ForeColor = Color.LightGray;
            lblBrojPoteza.Location = new Point(20, 62);
            lblBrojPoteza.Size = new Size(400, 20);
            this.Controls.Add(lblBrojPoteza);

            // Info o brodovima
            lblBrodovi = new Label();
            lblBrodovi.Font = new Font("Arial", 9);
            lblBrodovi.ForeColor = Color.LightGray;
            lblBrodovi.Location = new Point(430, 62);
            lblBrodovi.Size = new Size(400, 20);
            this.Controls.Add(lblBrodovi);

            // Oznake za table
            Label lblOznIgrac = new Label();
            lblOznIgrac.Text = "TVOJA TABLA";
            lblOznIgrac.Font = new Font("Arial", 10, FontStyle.Bold);
            lblOznIgrac.ForeColor = Color.FromArgb(100, 200, 100);
            lblOznIgrac.Location = new Point(OFFSET_X_LEVO + 80, OFFSET_Y - 22);
            lblOznIgrac.AutoSize = true;
            this.Controls.Add(lblOznIgrac);

            Label lblOznRac = new Label();
            lblOznRac.Text = "TABLA RACUNARA (gađaj!)";
            lblOznRac.Font = new Font("Arial", 10, FontStyle.Bold);
            lblOznRac.ForeColor = Color.FromArgb(255, 120, 120);
            lblOznRac.Location = new Point(OFFSET_X_DESNO + 20, OFFSET_Y - 22);
            lblOznRac.AutoSize = true;
            this.Controls.Add(lblOznRac);

            // Panel za tablu igraca
            panelIgrac = new Panel();
            panelIgrac.Location = new Point(OFFSET_X_LEVO, OFFSET_Y);
            panelIgrac.Size = new Size(Tabla.VELICINA * VELICINA_CELIJE + 1, Tabla.VELICINA * VELICINA_CELIJE + 1);
            panelIgrac.Paint += PanelIgrac_Paint;
            this.Controls.Add(panelIgrac);

            // Panel za tablu racunara
            panelRacunar = new Panel();
            panelRacunar.Location = new Point(OFFSET_X_DESNO, OFFSET_Y);
            panelRacunar.Size = new Size(Tabla.VELICINA * VELICINA_CELIJE + 1, Tabla.VELICINA * VELICINA_CELIJE + 1);
            panelRacunar.Paint += PanelRacunar_Paint;
            panelRacunar.MouseClick += PanelRacunar_MouseClick;
            panelRacunar.Cursor = Cursors.Cross;
            this.Controls.Add(panelRacunar);

            // Legenda
            DodajLegendum();
        }

        // Legenda boja
        private void DodajLegendum()
        {
            int y = OFFSET_Y + Tabla.VELICINA * VELICINA_CELIJE + 15;

            Panel pOcean = new Panel();
            pOcean.BackColor = Color.FromArgb(20, 60, 110);
            pOcean.Location = new Point(20, y);
            pOcean.Size = new Size(15, 15);
            this.Controls.Add(pOcean);
            Label lOcean = new Label(); lOcean.Text = "Okean"; lOcean.ForeColor = Color.LightGray;
            lOcean.Location = new Point(38, y - 2); lOcean.AutoSize = true;
            this.Controls.Add(lOcean);

            Panel pBrod = new Panel();
            pBrod.BackColor = Color.FromArgb(60, 120, 200);
            pBrod.Location = new Point(110, y);
            pBrod.Size = new Size(15, 15);
            this.Controls.Add(pBrod);
            Label lBrod = new Label(); lBrod.Text = "Brod"; lBrod.ForeColor = Color.LightGray;
            lBrod.Location = new Point(128, y - 2); lBrod.AutoSize = true;
            this.Controls.Add(lBrod);

            Panel pPog = new Panel();
            pPog.BackColor = Color.FromArgb(220, 60, 60);
            pPog.Location = new Point(190, y);
            pPog.Size = new Size(15, 15);
            this.Controls.Add(pPog);
            Label lPog = new Label(); lPog.Text = "Pogodak"; lPog.ForeColor = Color.LightGray;
            lPog.Location = new Point(208, y - 2); lPog.AutoSize = true;
            this.Controls.Add(lPog);

            Panel pProm = new Panel();
            pProm.BackColor = Color.FromArgb(180, 180, 200);
            pProm.Location = new Point(290, y);
            pProm.Size = new Size(15, 15);
            this.Controls.Add(pProm);
            Label lProm = new Label(); lProm.Text = "Promasaj"; lProm.ForeColor = Color.LightGray;
            lProm.Location = new Point(308, y - 2); lProm.AutoSize = true;
            this.Controls.Add(lProm);
        }

        // Crtanje table igraca (brodovi vidljivi)
        private void PanelIgrac_Paint(object sender, PaintEventArgs e)
        {
            CrtajTablu(e.Graphics, tablaIgraca, true);
        }

        // Crtanje table racunara (brodovi SKRIVENI)
        private void PanelRacunar_Paint(object sender, PaintEventArgs e)
        {
            CrtajTablu(e.Graphics, tablaRacunara, false);
        }

        private void CrtajTablu(Graphics g, Tabla tabla, bool prikaziBrodove)
        {
            for (int r = 0; r < Tabla.VELICINA; r++)
            {
                for (int k = 0; k < Tabla.VELICINA; k++)
                {
                    int x = k * VELICINA_CELIJE;
                    int y = r * VELICINA_CELIJE;
                    Color boja;

                    switch (tabla.Polja[r, k])
                    {
                        case StanjePolja.Pogodjeno:
                            boja = Color.FromArgb(220, 60, 60);
                            break;
                        case StanjePolja.Promaseno:
                            boja = Color.FromArgb(180, 180, 200);
                            break;
                        case StanjePolja.Brod:
                            boja = prikaziBrodove
                                ? Color.FromArgb(60, 120, 200)
                                : Color.FromArgb(20, 60, 110);
                            break;
                        default:
                            boja = Color.FromArgb(20, 60, 110);
                            break;
                    }

                    g.FillRectangle(new SolidBrush(boja), x, y, VELICINA_CELIJE, VELICINA_CELIJE);
                    g.DrawRectangle(new Pen(Color.FromArgb(40, 100, 160)), x, y, VELICINA_CELIJE, VELICINA_CELIJE);

                    // Nacrtaj pogodak (X) ili promasaj (tacka)
                    if (tabla.Polja[r, k] == StanjePolja.Pogodjeno)
                    {
                        Pen xPen = new Pen(Color.White, 2);
                        int padding = 8;
                        g.DrawLine(xPen, x + padding, y + padding, x + VELICINA_CELIJE - padding, y + VELICINA_CELIJE - padding);
                        g.DrawLine(xPen, x + VELICINA_CELIJE - padding, y + padding, x + padding, y + VELICINA_CELIJE - padding);
                    }
                    else if (tabla.Polja[r, k] == StanjePolja.Promaseno)
                    {
                        g.FillEllipse(new SolidBrush(Color.DarkGray),
                            x + VELICINA_CELIJE / 2 - 4, y + VELICINA_CELIJE / 2 - 4, 8, 8);
                    }
                }
            }
        }

        // Igrac klikce na tablu racunara
        private void PanelRacunar_MouseClick(object sender, MouseEventArgs e)
        {
            if (!jeIgracNaPotezu) return;

            int kolona = e.X / VELICINA_CELIJE;
            int red = e.Y / VELICINA_CELIJE;

            if (red < 0 || red >= Tabla.VELICINA || kolona < 0 || kolona >= Tabla.VELICINA)
                return;

            if (tablaRacunara.JeVecGadano(red, kolona))
            {
                lblStatus.Text = "To polje si vec gadao! Odaberi drugo.";
                return;
            }

            // Igrac gada
            bool pogodak = tablaRacunara.Gadaj(red, kolona);
            potezBrojac++;
            panelRacunar.Invalidate();

            if (tablaRacunara.SviBrodoPotopljeni())
            {
                ZavrsiIgru(true);
                return;
            }

            jeIgracNaPotezu = false;
            string poruka = pogodak ? "POGODAK! Racunar je na potezu..." : "Promasaj. Racunar je na potezu...";
            lblStatus.Text = poruka;
            AzurirajBrojac();

            // Timer za potez racunara (kratka pauza da se vidi animacija)
            Timer timer = new Timer();
            timer.Interval = 700;
            timer.Tick += delegate
            {
                timer.Stop();
                PotezRacunara();
            };
            timer.Start();
        }

        private void PotezRacunara()
        {
            int[] potez = racunar.SledeciPotez(tablaIgraca);
            bool pogodak = tablaIgraca.Gadaj(potez[0], potez[1]);
            potezBrojac++;

            if (pogodak)
                racunar.DodajPrioritete(potez[0], potez[1]);

            panelIgrac.Invalidate();

            if (tablaIgraca.SviBrodoPotopljeni())
            {
                ZavrsiIgru(false);
                return;
            }

            jeIgracNaPotezu = true;
            string poruka = pogodak ? "Racunar je pogodio tebe! Tvoj potez..." : "Racunar je promasio. Tvoj potez!";
            lblStatus.Text = poruka;
            AzurirajBrojac();
        }

        private void ZavrsiIgru(bool igracPobedio)
        {
            panelRacunar.Enabled = false;
            FormKraj kraj = new FormKraj(igracPobedio, potezBrojac);
            kraj.FormClosed += delegate
            {
                this.Close();
            };
            kraj.Show();
        }

        private void AzurirajPoruke()
        {
            lblStatus.Text = "Tvoj potez! Klikni na tablu racunara da gadjas.";
            AzurirajBrojac();
        }

        private void AzurirajBrojac()
        {
            lblBrojPoteza.Text = string.Format("Broj poteza: {0}", potezBrojac);

            int tvoji = BrojPotopljenih(tablaRacunara);
            int racunarovi = BrojPotopljenih(tablaIgraca);
            lblBrodovi.Text = string.Format("Potopljeni brodovi - Ti: {0}/{1}  |  Racunar: {2}/{3}",
                tvoji, velicine.Length, racunarovi, velicine.Length);
        }

        private int BrojPotopljenih(Tabla tabla)
        {
            int br = 0;
            foreach (Brod b in tabla.Brodovi)
                if (b.JePotopljen()) br++;
            return br;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
