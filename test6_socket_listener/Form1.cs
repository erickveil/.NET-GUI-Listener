using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace test6_socket_listener
{
    public partial class Form1 : Form
    {
        public bool ison = false;
        public Thread listen_thread;
        public AsynchListener listener=new AsynchListener();

        public Form1()
        {
            InitializeComponent();
        }

        private void start_stop_Click(object sender, EventArgs e)
        {
            if (!this.ison)
            {
                this.ison = true;
                state_button.BackColor = Color.Lime;

                // Run the listener on its own thread so we can still use the GUI
                this.listen_thread = new Thread(this.listener.StartListening);
                // This will allow the thread to die when the program does.
                this.listen_thread.IsBackground = true;
                this.listen_thread.Start();
            }
            else
            {
                this.ison = false;
                state_button.BackColor = Color.Red;
                this.listen_thread.Abort();
            }
        }
    }
}
