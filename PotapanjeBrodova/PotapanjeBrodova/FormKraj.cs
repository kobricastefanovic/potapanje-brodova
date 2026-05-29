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
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 40, 70);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Veliki rezultat
            Label lblRezultat = new Label();
            lblRezultat.Font = new Font("Arial", 28, FontStyle.Bold);
            lblRezultat.TextAlign = ContentAlignment.MiddleCenter;
            lblRezultat.Size = new Size(380, 80);
            lblRezultat.Location = new Point(10, 30);

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

            // Opis
            Label lblOpis = new Label();
            lblOpis.Font = new Font("Arial", 12);
            lblOpis.ForeColor = Color.LightGray;
            lblOpis.TextAlign = ContentAlignment.MiddleCenter;
            lblOpis.Size = new Size(380, 40);
            lblOpis.Location = new Point(10, 115);
            lblOpis.Text = pobeda
                ? "Potopili ste svu flotu protivnika!"
                : "Racunar je potopao svu vasu flotu!";
            this.Controls.Add(lblOpis);

            // Broj poteza
            Label lblPotezi = new Label();
            lblPotezi.Font = new Font("Arial", 11, FontStyle.Bold);
            lblPotezi.ForeColor = Color.Yellow;
            lblPotezi.TextAlign = ContentAlignment.MiddleCenter;
            lblPotezi.Size = new Size(380, 30);
            lblPotezi.Location = new Point(10, 155);
            lblPotezi.Text = string.Format("Ukupan broj poteza: {0}", brojPoteza);
            this.Controls.Add(lblPotezi);

            // Dugme nova igra
            Button btnNova = new Button();
            btnNova.Text = "NOVA IGRA";
            btnNova.Font = new Font("Arial", 11, FontStyle.Bold);
            btnNova.Size = new Size(160, 40);
            btnNova.Location = new Point(30, 210);
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

            // Dugme izlaz
            Button btnIzlaz = new Button();
            btnIzlaz.Text = "IZLAZ";
            btnIzlaz.Font = new Font("Arial", 11, FontStyle.Bold);
            btnIzlaz.Size = new Size(160, 40);
            btnIzlaz.Location = new Point(210, 210);
            btnIzlaz.BackColor = Color.FromArgb(140, 50, 50);
            btnIzlaz.ForeColor = Color.White;
            btnIzlaz.FlatStyle = FlatStyle.Flat;
            btnIzlaz.Click += delegate
            {
                Application.Exit();
            };
            this.Controls.Add(btnIzlaz);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
