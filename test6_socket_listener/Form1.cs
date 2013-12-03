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
        public SynchronousSocketListener listener = new SynchronousSocketListener();
        public Thread listen_thread;

        public Form1()
        {
            InitializeComponent();

            // We pass a closure to Thread so that we can start the method with a parameter.
            // We pass a reference to the Thread's parent in this case, so that we have access to the output text box.
            listen_thread = new Thread(() => { listener.StartListening(this); });
            // This will allow the thread to die when the program does.
            this.listen_thread.IsBackground = true;
            this.listen_thread.Start();
            this.listen_thread.Suspend();
            
        }

        private void start_stop_Click(object sender, EventArgs e)
        {
            if ((listen_thread.ThreadState & (ThreadState.Suspended)) == ThreadState.Running)
            {
                Console.WriteLine("Running detected. Attempting to suspend");
                this.listen_thread.Suspend();
                sendNullToListener();
            }
            else
            {
                Console.WriteLine("Suspended detected. Attempting to run");
                this.listen_thread.Resume();
            }
        }

        /**
         * This method is redundant because we have a matching delegate in the listener. The delagate allows us to 
         * indirectly, and legaly access the text box property in the middle of a loop.
         */
        public void updateDataBox(string msg)
        {
            tb_data.Text = msg;
        }



        /**
         * We track the state of the thread itself, and provide some feedback as to its state.
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if ((listen_thread.ThreadState & (ThreadState.Suspended)) == 0)
                {
                    state_button.BackColor = Color.Lime;
                }
                else
                {
                    state_button.BackColor = Color.Red;
                }
            }
            catch (Exception thread_ex)
            {
                // The thread was probably not started.
            }
        }

        /**
         * In order to force the Accept of the listener to move on and allow the thread to suspend, we send it null.
         */
        public void sendNullToListener()
        {
            SynchronousSocketClient sender = new SynchronousSocketClient();
            sender.StartClient();
        }
    }
}
