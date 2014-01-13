namespace Frost.TntEditor
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.treeView = new System.Windows.Forms.TreeView();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.nodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addNodeToolStripButton = new System.Windows.Forms.ToolStripSplitButton();
			this.decreaseDepthToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.moveUpToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.moveDownToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.nodeInfoPanel = new Frost.TntEditor.NodeInfo();
			this.menuStrip.SuspendLayout();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(633, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAstoolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// saveAstoolStripMenuItem
			// 
			this.saveAstoolStripMenuItem.Name = "saveAstoolStripMenuItem";
			this.saveAstoolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveAstoolStripMenuItem.Text = "Save &As...";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripButton,
            this.decreaseDepthToolStripButton,
            this.moveUpToolStripButton,
            this.moveDownToolStripButton,
            this.deleteToolStripButton});
			this.toolStrip.Location = new System.Drawing.Point(0, 24);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(633, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip";
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 49);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.nodeInfoPanel);
			this.splitContainer.Panel1MinSize = 175;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.treeView);
			this.splitContainer.Panel2MinSize = 100;
			this.splitContainer.Size = new System.Drawing.Size(633, 281);
			this.splitContainer.SplitterDistance = 210;
			this.splitContainer.TabIndex = 2;
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(419, 281);
			this.treeView.TabIndex = 0;
			this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = "openFileDialog1";
			// 
			// nodeContextMenuStrip
			// 
			this.nodeContextMenuStrip.Name = "nodeContextMenuStrip";
			this.nodeContextMenuStrip.Size = new System.Drawing.Size(61, 4);
			// 
			// addNodeToolStripButton
			// 
			this.addNodeToolStripButton.DropDown = this.newNodeContextMenuStrip;
			this.addNodeToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_design;
			this.addNodeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addNodeToolStripButton.Name = "addNodeToolStripButton";
			this.addNodeToolStripButton.Size = new System.Drawing.Size(93, 22);
			this.addNodeToolStripButton.Text = "Add Node";
			this.addNodeToolStripButton.ToolTipText = "Add a node to the structure";
			this.addNodeToolStripButton.Click += new System.EventHandler(this.addNodeToolStripButton_Click);
			// 
			// decreaseDepthToolStripButton
			// 
			this.decreaseDepthToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_insert_previous;
			this.decreaseDepthToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.decreaseDepthToolStripButton.Name = "decreaseDepthToolStripButton";
			this.decreaseDepthToolStripButton.Size = new System.Drawing.Size(109, 22);
			this.decreaseDepthToolStripButton.Text = "Decrease Depth";
			this.decreaseDepthToolStripButton.ToolTipText = "Move the node up to its parent";
			// 
			// moveUpToolStripButton
			// 
			this.moveUpToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_select_previous;
			this.moveUpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.moveUpToolStripButton.Name = "moveUpToolStripButton";
			this.moveUpToolStripButton.Size = new System.Drawing.Size(75, 22);
			this.moveUpToolStripButton.Text = "Move Up";
			this.moveUpToolStripButton.ToolTipText = "Move the node up one slot in its container. This does not have an effect on compl" +
    "ex nodes, as they are unordered.";
			// 
			// moveDownToolStripButton
			// 
			this.moveDownToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_select_next;
			this.moveDownToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.moveDownToolStripButton.Name = "moveDownToolStripButton";
			this.moveDownToolStripButton.Size = new System.Drawing.Size(91, 22);
			this.moveDownToolStripButton.Text = "Move Down";
			this.moveDownToolStripButton.ToolTipText = "Move the node down one slot in its container. This does not have an effect on com" +
    "plex nodes, as they are unordered.";
			// 
			// deleteToolStripButton
			// 
			this.deleteToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_delete_next;
			this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.deleteToolStripButton.Name = "deleteToolStripButton";
			this.deleteToolStripButton.Size = new System.Drawing.Size(92, 22);
			this.deleteToolStripButton.Text = "Delete Node";
			this.deleteToolStripButton.ToolTipText = "Deleted the selected node";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.document_node;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.newToolStripMenuItem.Text = "&New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.folder_horizontal_open;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "&Open";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.disk_black;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			// 
			// newNodeContextMenuStrip
			// 
			this.newNodeContextMenuStrip.Name = "newNodeContextMenuStrip";
			this.newNodeContextMenuStrip.OwnerItem = this.addNodeToolStripButton;
			this.newNodeContextMenuStrip.ShowImageMargin = false;
			this.newNodeContextMenuStrip.Size = new System.Drawing.Size(128, 26);
			// 
			// nodeInfoPanel
			// 
			this.nodeInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeInfoPanel.Location = new System.Drawing.Point(0, 0);
			this.nodeInfoPanel.Name = "nodeInfoPanel";
			this.nodeInfoPanel.Size = new System.Drawing.Size(210, 281);
			this.nodeInfoPanel.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 330);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "TNT Editor";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAstoolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ContextMenuStrip nodeContextMenuStrip;
		private NodeInfo nodeInfoPanel;
		private System.Windows.Forms.ToolStripSplitButton addNodeToolStripButton;
		private System.Windows.Forms.ToolStripButton decreaseDepthToolStripButton;
		private System.Windows.Forms.ToolStripButton moveUpToolStripButton;
		private System.Windows.Forms.ToolStripButton moveDownToolStripButton;
		private System.Windows.Forms.ToolStripButton deleteToolStripButton;
		private System.Windows.Forms.ContextMenuStrip newNodeContextMenuStrip;
	}
}

