using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ScrollingLabel : UserControl
    {
        [Description("Set Text")]
        [Category("ScollingLabel")]
        [DefaultValue("")]
        public override String Text
        {
            get;
            set;
        }

        public ScrollingLabel()
        {
            InitializeComponent();
            MainLabel.Size = this.Size; // not sure
        }

        public new Size Size
        {
            get { return base.Size; }
            set
            {
                MainLabel.Size = value;
                this.Size = MainLabel.Size;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            //Font font = new Font("Vernada", (float)this.Height,

        }

        private void ScrollingLabel_Load(object sender, EventArgs e)
        {
            
        }
    }
}
