using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Windows.Input;
using ClonesEngine;

namespace MultiplayerPaint
{

    public partial class frmPaint : Form
    {
        GestionnaireDePacket GP;
        //UDPConnecter ConnectionUDP;
        bool update = true;
        bool mouse = true;
        Random RNG;
        //byte ID;
        public frmPaint()
        {
            InitializeComponent();
            BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            RNG = new Random();
            GP = new GestionnaireDePacket();
            this.DoubleBuffered = true;
            Thread.Sleep(200);
            timer1.Start();
            update = false;
        }

        private void frmPaint_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*if (mouse)
            {
                mouse = false;
            }
            else
            {
                mouse = true;
                this.Refresh();
            }*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                Graphics g = this.CreateGraphics();
                for (byte i = 1; i <= GP.PlayerCount; i++)////////////////////////////////////+ 1
                {
                    int X = 0, Y = 0;
                    X = GP.PlayerList[i].Position.X;
                    Y = GP.PlayerList[i].Position.Y;



                    g.FillEllipse(new SolidBrush(Color.FromArgb(GP.PlayerList[i].Couleur)), X - 10, Y - 10, 20, 20);

                    /*rtb1.Text += a.ToString() + "x:" + c.ToString() + "y\n";
                    rtb1.SelectionStart = rtb1.Text.Length;
                    rtb1.ScrollToCaret();*/

                    if (X == 0 && Y == 0)
                    {
                        GP.PlayerList[i].Position = new Point(1, 1);
                        GP.Send(TramePreGen.InfoJoueur(GP.PlayerList[i], i, GP.PacketID));
                        this.Refresh();
                    }
                }
                g.DrawString(GP.ID.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), 10, 10);
                if (MouseButtons == MouseButtons.Left)//mouse)
                {
                    
                    GP.PlayerList[GP.ID].Position = this.PointToClient(Cursor.Position);
                    GP.Send(TramePreGen.InfoJoueur(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));




                }

                if (update)
                {
                    this.Invalidate();
                }
            }));
        }

        private void frmPaint_Paint(object sender, PaintEventArgs e)
        {
            if (true)
            {
                Invoke(new Action(() =>
                {
                    for (int i = 1; i <= GP.PlayerCount; i++)
                    {
                        int X, Y;
                        X = GP.PlayerList[i].Position.X;
                        Y = GP.PlayerList[i].Position.Y;

                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(GP.PlayerList[i].Couleur)), X - 10, Y - 10, 20, 20);
                        /*rtb1.Text += a.ToString() + "x:" + c.ToString() + "y\n";
                        rtb1.SelectionStart = rtb1.Text.Length;
                        rtb1.ScrollToCaret();*/
                    }
                    if (mouse)
                    {
                        Point Light = this.PointToClient(Cursor.Position);
                        if (GP.PlayerCount > 0)
                        {
                            
                            GP.PlayerList[GP.ID].Position = Light;
                        }


                    }
                    if (update)
                    {
                        this.Invalidate();
                    }
                }));


            }
        }

        private void frmPaint_Click(object sender, EventArgs e)
        {
            if (update)
            {
                update = false;
                timer1.Start();
            }
            else
            {
                //update = true;
                //timer1.Stop();
                //this.Refresh();
            }
        }

        private void frmPaint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                byte[] b = new byte[2];
                b[0] = 5;
                b[1] = GP.ID;
                GP.Send(b);
            }
            else
            {
                if (e.KeyData == Keys.Space)
                {
                    using (ColorDialog ColorPicker = new ColorDialog())
                    {
                        if (ColorPicker.ShowDialog() == DialogResult.OK)
                        {
                            GP.PlayerList[GP.ID].Couleur = ColorPicker.Color.ToArgb();
                            GP.Send(TramePreGen.InfoJoueur(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));
                        }
                    }
                }
                else
                {
                    GP.PlayerList[GP.ID].Position = new Point(0, 0);
                    GP.Send(TramePreGen.InfoJoueur(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));
                }
            }
        }

        private void frmPaint_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
    }
}
