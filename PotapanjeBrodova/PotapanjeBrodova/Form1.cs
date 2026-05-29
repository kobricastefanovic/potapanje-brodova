using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PotapanjeBrodova
{
    // Prva forma - igrac postavlja brodove na svoju tablu
    public partial class Form1 : Form
    {
        private const int VELICINA_CELIJE = 40;
        private const int OFFSET_X = 30;
        private const int OFFSET_Y = 80;

        // Velicine brodova koje treba postaviti: 1x nosac aviona(5), 1x bojni brod(4),
        // 1x razarac(3), 1x podmornica(3), 1x patrolni(2)
        private int[] velicine = { 5, 4, 3, 3, 2 };
        private string[] imenaBrodova = { "Nosac aviona (5)", "Bojni brod (4)", "Razarac (3)", "Podmornica (3)", "Patrolni (2)" };

        private Tabla tablaIgraca;
        private int trenutniBrod = 0;
        private bool jeHorizontalan = true;

        private Panel panelTabla;
        private Label lblUputstvo;
        private Label lblTrenutni;
        private Button btnOkret;
        private Button btnObrisi;
        private Button btnAutoPostavi;
        private Button btnZapocni;

        public Form1()
        {
            tablaIgraca = new Tabla();
            InitializeComponent();
            InicijalizujUI();
            AzurirajPoruke();
        }

        private void InicijalizujUI()
        {
            this.Text = "Potapanje brodova - Postavljanje";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 40, 70);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Naslov
            Label lblNaslov = new Label();
            lblNaslov.Text = "POTAPANJE BRODOVA";
            lblNaslov.Font = new Font("Arial", 18, FontStyle.Bold);
            lblNaslov.ForeColor = Color.FromArgb(100, 200, 255);
            lblNaslov.Location = new Point(OFFSET_X, 10);
            lblNaslov.AutoSize = true;
            this.Controls.Add(lblNaslov);

            // Uputstvo
            lblUputstvo = new Label();
            lblUputstvo.Font = new Font("Arial", 9);
            lblUputstvo.ForeColor = Color.LightGray;
            lblUputstvo.Location = new Point(OFFSET_X, 40);
            lblUputstvo.Size = new Size(500, 20);
            this.Controls.Add(lblUputstvo);

            // Trenutni brod
            lblTrenutni = new Label();
            lblTrenutni.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTrenutni.ForeColor = Color.Yellow;
            lblTrenutni.Location = new Point(OFFSET_X, 58);
            lblTrenutni.Size = new Size(500, 20);
            this.Controls.Add(lblTrenutni);

            // Panel za tablu (iscrtava se u OnPaint)
            panelTabla = new Panel();
            panelTabla.Location = new Point(OFFSET_X, OFFSET_Y);
            panelTabla.Size = new Size(Tabla.VELICINA * VELICINA_CELIJE + 1, Tabla.VELICINA * VELICINA_CELIJE + 1);
            panelTabla.Paint += new PaintEventHandler(PanelTabla_Paint);
            panelTabla.MouseClick += new MouseEventHandler(PanelTabla_MouseClick);
            this.Controls.Add(panelTabla);

            int btnY = OFFSET_Y + Tabla.VELICINA * VELICINA_CELIJE + 15;

            // Dugme okret
            btnOkret = new Button();
            btnOkret.Text = "Okret (H)";
            btnOkret.Location = new Point(OFFSET_X, btnY);
            btnOkret.Size = new Size(100, 35);
            btnOkret.BackColor = Color.FromArgb(30, 80, 140);
            btnOkret.ForeColor = Color.White;
            btnOkret.FlatStyle = FlatStyle.Flat;
            btnOkret.Click += btnOkret_Click;
            this.Controls.Add(btnOkret);

            // Dugme obrisi
            btnObrisi = new Button();
            btnObrisi.Text = "Obrisi sve";
            btnObrisi.Location = new Point(OFFSET_X + 110, btnY);
            btnObrisi.Size = new Size(100, 35);
            btnObrisi.BackColor = Color.FromArgb(140, 50, 50);
            btnObrisi.ForeColor = Color.White;
            btnObrisi.FlatStyle = FlatStyle.Flat;
            btnObrisi.Click += btnObrisi_Click;
            this.Controls.Add(btnObrisi);

            // Dugme auto postavi
            btnAutoPostavi = new Button();
            btnAutoPostavi.Text = "Auto postavi";
            btnAutoPostavi.Location = new Point(OFFSET_X + 220, btnY);
            btnAutoPostavi.Size = new Size(110, 35);
            btnAutoPostavi.BackColor = Color.FromArgb(50, 120, 50);
            btnAutoPostavi.ForeColor = Color.White;
            btnAutoPostavi.FlatStyle = FlatStyle.Flat;
            btnAutoPostavi.Click += btnAutoPostavi_Click;
            this.Controls.Add(btnAutoPostavi);

            // Dugme zapocni igru
            btnZapocni = new Button();
            btnZapocni.Text = "ZAPOCNI IGRU";
            btnZapocni.Location = new Point(OFFSET_X + 340, btnY);
            btnZapocni.Size = new Size(160, 35);
            btnZapocni.BackColor = Color.FromArgb(200, 140, 0);
            btnZapocni.ForeColor = Color.White;
            btnZapocni.FlatStyle = FlatStyle.Flat;
            btnZapocni.Font = new Font("Arial", 10, FontStyle.Bold);
            btnZapocni.Enabled = false;
            btnZapocni.Click += btnZapocni_Click;
            this.Controls.Add(btnZapocni);

            // Osluskuje tastaturu
            this.KeyDown += Form1_KeyDown;
            this.KeyPreview = true;
        }

        // Crtanje table
        private void PanelTabla_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < Tabla.VELICINA; r++)
            {
                for (int k = 0; k < Tabla.VELICINA; k++)
                {
                    int x = k * VELICINA_CELIJE;
                    int y = r * VELICINA_CELIJE;

                    // Boja pozadine celije
                    Color boja;
                    switch (tablaIgraca.Polja[r, k])
                    {
                        case StanjePolja.Brod:
                            boja = Color.FromArgb(60, 120, 200);
                            break;
                        default:
                            boja = Color.FromArgb(20, 60, 110);
                            break;
                    }

                    g.FillRectangle(new SolidBrush(boja), x, y, VELICINA_CELIJE, VELICINA_CELIJE);
                    g.DrawRectangle(new Pen(Color.FromArgb(40, 100, 160)), x, y, VELICINA_CELIJE, VELICINA_CELIJE);
                }
            }
        }

        // Klik na tablu - postavljanje broda
        private void PanelTabla_MouseClick(object sender, MouseEventArgs e)
        {
            if (trenutniBrod >= velicine.Length) return;

            int kolona = e.X / VELICINA_CELIJE;
            int red = e.Y / VELICINA_CELIJE;

            bool uspesno = tablaIgraca.PostaviBrod(red, kolona, velicine[trenutniBrod], jeHorizontalan);

            if (uspesno)
            {
                trenutniBrod++;
                AzurirajPoruke();
                panelTabla.Invalidate();

                if (trenutniBrod >= velicine.Length)
                {
                    lblTrenutni.Text = "Svi brodovi postavljeni! Mozete zapoceti igru.";
                    btnZapocni.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Ne moze se postaviti ovde! Probajte drugu poziciju.", "Greska",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOkret_Click(object sender, EventArgs e)
        {
            jeHorizontalan = !jeHorizontalan;
            AzurirajPoruke();
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            tablaIgraca = new Tabla();
            trenutniBrod = 0;
            jeHorizontalan = true;
            btnZapocni.Enabled = false;
            AzurirajPoruke();
            panelTabla.Invalidate();
        }

        private void btnAutoPostavi_Click(object sender, EventArgs e)
        {
            tablaIgraca = new Tabla();
            trenutniBrod = 0;
            RacunarLogika ai = new RacunarLogika();
            ai.AutoPostavljanje(tablaIgraca, velicine);
            trenutniBrod = velicine.Length;
            lblTrenutni.Text = "Brodovi automatski postavljeni!";
            btnZapocni.Enabled = true;
            panelTabla.Invalidate();
        }

        private void btnZapocni_Click(object sender, EventArgs e)
        {
            FormIgra igra = new FormIgra(tablaIgraca, velicine);
            igra.Show();
            this.Hide();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.H || e.KeyCode == Keys.R)
            {
                jeHorizontalan = !jeHorizontalan;
                AzurirajPoruke();
            }
        }

        private void AzurirajPoruke()
        {
            lblUputstvo.Text = "Kliknite na tablu da postavite brod | H = promeni orijentaciju";
            if (trenutniBrod < velicine.Length)
            {
                string orijentacija = jeHorizontalan ? "HORIZONTALNO" : "VERTIKALNO";
                lblTrenutni.Text = string.Format("Postavljate: {0}  |  Orijentacija: {1}",
                    imenaBrodova[trenutniBrod], orijentacija);
            }
        }
    }
}
