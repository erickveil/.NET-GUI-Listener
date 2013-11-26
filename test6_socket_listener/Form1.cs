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
        // Run the listener on its own thread so we can still use the GUI
        public AsynchListener listener = new AsynchListener();
        public Thread listen_thread;

        public Form1()
        {
            InitializeComponent();

            listen_thread = new Thread(listener.StartListening);
            // This will allow the thread to die when the program does.
            this.listen_thread.IsBackground = true;
            this.listen_thread.Start();
        }

        private void start_stop_Click(object sender, EventArgs e)
        {
            if (!this.ison)
            {
                this.ison = true;
                test6_socket_listener.Properties.Settings.Default.active = true;
                state_button.BackColor = Color.Lime;
            }
            else
            {
                this.ison = false;
                test6_socket_listener.Properties.Settings.Default.active = false;
                state_button.BackColor = Color.Red;
            }
        }
    }
}
