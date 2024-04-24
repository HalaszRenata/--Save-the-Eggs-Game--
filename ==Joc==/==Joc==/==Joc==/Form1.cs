using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace __Joc__
{
    //In Save The Eggs Game, jucătorul trebuie să salveze cât mai multe ouă.
    //Un contor reține numărul de ouă salvate, iar un alt contor reține numărul de ouă sparte
    //Ideea acestui joc este ca jucătorul să prindă obiectele care apar din partea de sus a ecranului
    //Jucătorul va putea să se deplaseze la stânga și la dreapta,prin butoane, iar
    //jucătorul va putea, să prindă ouăle care cad sau se sparg
    //dacă acestea ajung în partea de jos a ecranului sub formă de ouă sparte.
    //De asemenea, după un număr de obiecte salvate,in cod dupa 10 de elemente, viteza apariției obiectelor
    //crește, iar picăturile vor deveni mai dese, ceea ce va face jocul mai dificil.
    public partial class Form1 : Form
    {
        bool goleft; //deplasare la stanga
        bool goright; //deplasare la dreapta
        int speed = 5;//viteza cu care cad ouale initial
        int score = 0;//valoarea initiala a scorului
        int missed = 0;//valoarea initiala pentru ouale pierdute
        Random rndy = new Random();//valoarea oarecare pentru locatia Y
        Random rndx = new Random();//valoarea oarecare pentru locatia X
        PictureBox splash = new PictureBox();//creaza dinamic noi stropi 
        public Form1()
        {
            InitializeComponent();
            reset();//resetarea jocului
        }
    

        private void MainGameTimeEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Eggs Score: " + score; //se afiseaza scorul oualelor salvate
            txtMissed.Text = "Eggs Missed: " + missed;//se afiseaza scorul oualelor pierdute
            if (goleft == true && player.Left > 0)
            {
                // Daca se acceseaza sageata stanga se deplaseaza gaina cu 12 pixeli
                player.Left -= 12;
                //se schimba imaginea gainii cu fata catre stanga
                player.Image = Properties.Resources.chicken_normal2;
            }
            if (goright == true && player.Left + player.Width < this.ClientSize.Width)
            {
                // Daca se acceseaza sageata dreapta se deplaseaza gaina catre dreapta cu 12 pixeli
                player.Left += 12;
                //se schimba imaginea gainii cu fata catre stanga
                player.Image = Properties.Resources.chicken_normal;
            }
            //verificam daca gaina nu a depasit suprafata ferestrei
            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && X.Tag == "egg")
                {
                    //daca se coboara un ou si a fost prins creste viteza
                    X.Top += speed;
                    //daca oul nu a fost prins apare
                    // Oul s-a spart, se schimba imaginea
                    if (X.Top + X.Height > this.ClientSize.Height)
                    {
                        splash.Image = Properties.Resources.splash;
                        //setam locatia oualui care se va sparge
                        splash.Location = X.Location;
                        //setează înălțimea imaginii de stropire(splash) la 59 de pixeli. 
                        splash.Height = 59;
                        //setam culoarea transparenta a imaginii oualui pierdut
                        splash.BackColor = System.Drawing.Color.Transparent;
                        //adaugam imaginea picaturii pe forma
                        this.Controls.Add(splash);
                        //pozitionam imaginea picaturii
                        X.Top = rndy.Next(80, 300);
                        X.Left = rndx.Next(5, this.ClientSize.Width - X.Width);
                        //creste contorul oualelor sparte
                        missed++;
                        // imaginea vizibila
                        player.Image = Properties.Resources.chicken_hurt;
                    }
                    //daca oul a fost atins de gaina,ambele imagini se intersecteaza
                    if (X.Bounds.IntersectsWith(player.Bounds))
                    {
                        //repozitionam oul aleator
                        X.Top = rndy.Next(100, 300) * -1;
                        X.Left = rndx.Next(5, this.ClientSize.Width - X.Width);
                        //creste scorul oualelor salvate
                        score++;
                    }
                    if (score >= 10)
                        //daca scorul depaseste 10 puncte creste viteza de coborare a oualelor
                        speed = 7;
                    if (score >= 20)
                        //daca scorul depaseste 20 puncte creste viteza de coborare a oualelor
                        speed = 8;
                    if (score >= 30)
                        //daca scorul depaseste 30 puncte creste viteza de coborare a oualelor
                        speed = 9;
                    if (score >= 40)
                        //daca scorul depaseste 40 puncte creste viteza de coborare a oualelor
                        speed = 10;
                    if (score >= 50)
                        //daca scorul depaseste 50 puncte creste viteza de coborare a oualelor
                        speed = 11;
                    //Se castiga doar daca gaina prinde 100 de oua, fara a scapa mai mult de 5 oua
                    if (score > 100 && missed <= 5)
                    {
                        GameTimer.Stop();
                        MessageBox.Show("Felicitări! Ai câștigat jocul! Ai prins toate cele 100 de ouă fără să pierzi mai mult de 5.");
                        reset();
                    }
                }
                
                if (missed > 5)
                {
                    GameTimer.Stop();
                    //se afiseaza mesajul
                    MessageBox.Show("Joc terminat.Ai pierdut!" + "\r\n" + "Click pe Ok pentru a incepe jocul de la inceput");
                    //daca se apasa ok se restarteaza jocul
                    reset();
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                //daca se apasa tasta sageata stanga jucatorul se deplaseaza spre stanga
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                //daca se apasa tasta dreapta stanga jucatorul se deplaseaza spre dreapta
                goright = true;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // În momentul în care tasta stânga sau dreapta este eliberată,
            // se setează variabila corespunzătoare ca false.
            if (e.KeyCode == Keys.Left)
            {
                // Dacă se eliberează tasta săgeată stânga, se oprește deplasarea către stânga.
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                // Dacă se eliberează tasta săgeată dreapta, se oprește deplasarea către dreapta.
                goright = false;
            }

        }
        private void reset()
        {
            //verificam toate elementele jocului
            foreach (Control X in this.Controls)
            {
                if (X is PictureBox && (string)X.Tag == "egg")
                {
                    X.Top = rndy.Next(100, 300) * -1;
                    X.Left = rndx.Next(5, this.ClientSize.Width - X.Width);
                }
            }
            player.Left = this.ClientSize.Width / 2;
            player.Image = Properties.Resources.chicken_normal2;
            score = 0;
            missed = 0;
            speed = 5;
            goleft = false;
            goright = false;
            GameTimer.Start();
        }
    }
}