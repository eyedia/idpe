namespace Eyedia.Core.Windows.Control
{
    partial class IntillisenseEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IntillisenseEditor));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("BigParser");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Stuff1", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("TreeChopper");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Stuff2", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("AddFile");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("DeleteFile");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("RenameFile");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("FileCreater", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("AddThing");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("GraphicsEngine", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Widgets", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("EvenMoreStuff", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("CodeProject", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4,
            treeNode12});
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBoxTooltip = new System.Windows.Forms.TextBox();
            this.treeViewItems = new System.Windows.Forms.TreeView();
            this.listBoxAutoComplete = new Eyedia.Core.Windows.Control.GListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(230, 107);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";            
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            this.richTextBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Lime;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            // 
            // textBoxTooltip
            // 
            this.textBoxTooltip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(225)))));
            this.textBoxTooltip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTooltip.Location = new System.Drawing.Point(121, 32);
            this.textBoxTooltip.Multiline = true;
            this.textBoxTooltip.Name = "textBoxTooltip";
            this.textBoxTooltip.ReadOnly = true;
            this.textBoxTooltip.Size = new System.Drawing.Size(100, 20);
            this.textBoxTooltip.TabIndex = 6;
            this.textBoxTooltip.Visible = false;
            this.textBoxTooltip.Enter += new System.EventHandler(this.textBoxTooltip_Enter);
            // 
            // treeViewItems
            // 
            this.treeViewItems.FullRowSelect = true;
            this.treeViewItems.Location = new System.Drawing.Point(13, 12);
            this.treeViewItems.Name = "treeViewItems";
            treeNode1.Name = "";
            treeNode1.Text = "BigParser";
            treeNode2.Name = "";
            treeNode2.Text = "Stuff1";
            treeNode3.Name = "";
            treeNode3.Text = "TreeChopper";
            treeNode4.Name = "";
            treeNode4.Text = "Stuff2";
            treeNode5.Name = "";
            treeNode5.Text = "AddFile";
            treeNode6.Name = "";
            treeNode6.Text = "DeleteFile";
            treeNode7.Name = "";
            treeNode7.Text = "RenameFile";
            treeNode8.Name = "";
            treeNode8.Text = "FileCreater";
            treeNode9.Name = "";
            treeNode9.Text = "AddThing";
            treeNode10.Name = "";
            treeNode10.Text = "GraphicsEngine";
            treeNode11.Name = "";
            treeNode11.Text = "Widgets";
            treeNode12.Name = "";
            treeNode12.Text = "EvenMoreStuff";
            treeNode13.Name = "";
            treeNode13.Text = "CodeProject";
            this.treeViewItems.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode13});
            this.treeViewItems.PathSeparator = ".";
            this.treeViewItems.Size = new System.Drawing.Size(170, 40);
            this.treeViewItems.TabIndex = 7;
            this.treeViewItems.Visible = false;
            // 
            // listBoxAutoComplete
            // 
            this.listBoxAutoComplete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxAutoComplete.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxAutoComplete.ImageList = this.imageList1;
            this.listBoxAutoComplete.Location = new System.Drawing.Point(13, 58);
            this.listBoxAutoComplete.Name = "listBoxAutoComplete";
            this.listBoxAutoComplete.Size = new System.Drawing.Size(208, 132);
            this.listBoxAutoComplete.TabIndex = 4;
            this.listBoxAutoComplete.Visible = false;
            this.listBoxAutoComplete.SelectedValueChanged += new System.EventHandler(this.listBoxAutoComplete_SelectedIndexChanged);
            this.listBoxAutoComplete.DoubleClick += new System.EventHandler(this.listBoxAutoComplete_DoubleClick);
            this.listBoxAutoComplete.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxAutoComplete_KeyDown);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // IntillisenseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewItems);
            this.Controls.Add(this.textBoxTooltip);
            this.Controls.Add(this.listBoxAutoComplete);
            this.Controls.Add(this.richTextBox1);
            this.Name = "IntillisenseEditor";
            this.Size = new System.Drawing.Size(230, 107);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox1;
        private GListBox listBoxAutoComplete;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBoxTooltip;
        private System.Windows.Forms.TreeView treeViewItems;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
