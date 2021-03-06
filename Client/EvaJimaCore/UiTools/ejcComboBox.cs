﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EveJimaCore.WhlControls
{
    public partial class ejcComboBox : UserControl
    {
        private bool _autoSize;

        public event Action<string> OnElementChanged;

        public ejcComboBox()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int width = 0;

            foreach (ejcComboboxItem comboBox1Item in comboBox1.Items)
            {
                lblSizeOwner.Text = comboBox1Item.Text;
                lblSizeOwner.AutoSize = true;
                if (lblSizeOwner.Width > width) width = lblSizeOwner.Width;
            }

            comboBox1.Width = width + 30;
            
            comboBox1.Visible = true;
            comboBox1.DroppedDown = true;
        }


        private void comboBox1_SelectedValueChanged_1(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            cmdPathfinder.Text = comboBox1.Text;

            if (OnElementChanged != null)
            {
                var element = ((ejcComboboxItem)comboBox1.SelectedItem).Value.ToString();
                OnElementChanged(element);
            }
        }

        private void cmdPathfinder_Click(object sender, EventArgs e)
        {
            if (OnElementChanged != null)
            {
                var element = ((ejcComboboxItem)comboBox1.SelectedItem).Value.ToString();
                OnElementChanged(element);
            }
        }


        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        public override string Text
        {
            get
            {
                return cmdPathfinder.Text;
            }

            set
            {
                cmdPathfinder.Text = value;
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        public string Value
        {
            get
            {
                return (comboBox1.SelectedItem as ejcComboboxItem).Value.ToString();
            }

        }

        

        [Category("Appearance")]
        public override bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
                // causes control to be redrawn
            }
        }

        public void ResetSize()
        {
            int width = 0;

            foreach (ejcComboboxItem comboBox1Item in comboBox1.Items)
            {
                lblSizeOwner.Text = comboBox1Item.Text;
                lblSizeOwner.AutoSize = true;
                if (lblSizeOwner.Width > width) width = lblSizeOwner.Width;
            }

            if (_autoSize)
            {
                cmdPathfinder.AutoSize = true;
                Width = cmdPathfinder.Left + 2 + cmdPathfinder.Width;
            }

            Invalidate();
        }

        public void AddItem(ejcComboboxItem item)
        {
            comboBox1.Items.Add(item);

            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty( comboBox1.SelectedText) )
            {

                comboBox1.Visible = false;
                cmdPathfinder.Text = comboBox1.Text;

                if(OnElementChanged != null)
                {
                    var element = ((ejcComboboxItem)comboBox1.SelectedItem).Value.ToString();
                    OnElementChanged(element);
                }
            }
        }

        public bool IsContaintItem(string item)
        {

            foreach(var comboBox1Item in comboBox1.Items)
            {
                var element = (ejcComboboxItem)comboBox1Item;

                if(element.Value.ToString().Trim() == item.Trim())
                {
                    return true;
                }
            }

            return false;
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var backgroundBrush = new SolidBrush(Color.FromArgb(15, 15, 15));

            try
            {

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {

                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

                    e.Graphics.DrawString("   " + comboBox1.Items[e.Index], comboBox1.Font, Brushes.Peru, new Rectangle(e.Bounds.X, e.Bounds.Y + 6, e.Bounds.Width, e.Bounds.Height));
                }
                else
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

                    e.Graphics.DrawString("   " + comboBox1.Items[e.Index], comboBox1.Font, Brushes.White, new Rectangle(e.Bounds.X, e.Bounds.Y + 6, e.Bounds.Width, e.Bounds.Height));
                }
            }
            catch(Exception exception)
            {
                
            }

        }

        private void ejcComboBox_ForeColorChanged(object sender, EventArgs e)
        {
            cmdPathfinder.ForeColor = ForeColor;
        }

        public void ActivateItem(string panelName)
        {
            foreach (ejcComboboxItem comboBox1Item in comboBox1.Items)
            {
                if(comboBox1Item.Value == panelName)
                {
                    cmdPathfinder.Text = comboBox1Item.Text;
                    comboBox1.SelectedItem = comboBox1Item;
                }
            }
        }
    }

    public class ejcComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
