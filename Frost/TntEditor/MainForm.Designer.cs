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
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addNodeListToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.moveUpToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.moveDownToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.searchToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.searchToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.treeView = new System.Windows.Forms.TreeView();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.nodeInfoPanel = new Frost.TntEditor.NodeInfo();
			this.addNodeToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.menuStrip.SuspendLayout();
			this.nodeContextMenuStrip.SuspendLayout();
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
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(762, 24);
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
			this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.document_node;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.folder_horizontal_open;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.disk_black;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAstoolStripMenuItem
			// 
			this.saveAstoolStripMenuItem.Name = "saveAstoolStripMenuItem";
			this.saveAstoolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveAstoolStripMenuItem.Text = "Save &As...";
			this.saveAstoolStripMenuItem.Click += new System.EventHandler(this.saveAstoolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDown = this.nodeContextMenuStrip;
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// nodeContextMenuStrip
			// 
			this.nodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
			this.nodeContextMenuStrip.Name = "nodeContextMenuStrip";
			this.nodeContextMenuStrip.OwnerItem = this.editToolStripMenuItem;
			this.nodeContextMenuStrip.Size = new System.Drawing.Size(204, 136);
			// 
			// addToolStripMenuItem
			// 
			this.addToolStripMenuItem.DropDown = this.newNodeContextMenuStrip;
			this.addToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.node_design;
			this.addToolStripMenuItem.Name = "addToolStripMenuItem";
			this.addToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Insert;
			this.addToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.addToolStripMenuItem.Text = "&Add Node";
			this.addToolStripMenuItem.ToolTipText = "Add a new node below the selected node";
			// 
			// newNodeContextMenuStrip
			// 
			this.newNodeContextMenuStrip.Name = "newNodeContextMenuStrip";
			this.newNodeContextMenuStrip.OwnerItem = this.addToolStripMenuItem;
			this.newNodeContextMenuStrip.ShowImageMargin = false;
			this.newNodeContextMenuStrip.Size = new System.Drawing.Size(128, 26);
			// 
			// addNodeListToolStripButton
			// 
			this.addNodeListToolStripButton.DropDown = this.newNodeContextMenuStrip;
			this.addNodeListToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_design;
			this.addNodeListToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addNodeListToolStripButton.Name = "addNodeListToolStripButton";
			this.addNodeListToolStripButton.Size = new System.Drawing.Size(90, 22);
			this.addNodeListToolStripButton.Text = "Add Node";
			this.addNodeListToolStripButton.ToolTipText = "Add a node to the structure";
			// 
			// moveUpToolStripMenuItem
			// 
			this.moveUpToolStripMenuItem.Enabled = false;
			this.moveUpToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.node_select_previous;
			this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
			this.moveUpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
			this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.moveUpToolStripMenuItem.Text = "Move &Up";
			this.moveUpToolStripMenuItem.ToolTipText = "Move the node up one slot in its container. This does not have an effect on compl" +
    "ex nodes, as they are unordered.";
			// 
			// moveDownToolStripMenuItem
			// 
			this.moveDownToolStripMenuItem.Enabled = false;
			this.moveDownToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.node_select_next;
			this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
			this.moveDownToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
			this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.moveDownToolStripMenuItem.Text = "Move &Down";
			this.moveDownToolStripMenuItem.ToolTipText = "Move the node down one slot in its container. This does not have an effect on com" +
    "plex nodes, as they are unordered.";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Enabled = false;
			this.copyToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.node_insert_child;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.copyToolStripMenuItem.Text = "&Copy Node";
			this.copyToolStripMenuItem.ToolTipText = "Copy the selected node to the clipboard";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Enabled = false;
			this.pasteToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.node_insert_next;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.pasteToolStripMenuItem.Text = "&Paste Node";
			this.pasteToolStripMenuItem.ToolTipText = "Pastes a node from the clipboard below the currently selected node";
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Enabled = false;
			this.deleteToolStripMenuItem.Image = global::Frost.TntEditor.Properties.Resources.node_delete_next;
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.deleteToolStripMenuItem.Text = "Dele&te Node";
			this.deleteToolStripMenuItem.ToolTipText = "Deleted the selected node";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripButton,
            this.addNodeListToolStripButton,
            this.moveUpToolStripButton,
            this.moveDownToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.deleteToolStripButton,
            this.searchToolStripButton,
            this.searchToolStripTextBox});
			this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip.Location = new System.Drawing.Point(0, 24);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(762, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip";
			// 
			// moveUpToolStripButton
			// 
			this.moveUpToolStripButton.Enabled = false;
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
			this.moveDownToolStripButton.Enabled = false;
			this.moveDownToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_select_next;
			this.moveDownToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.moveDownToolStripButton.Name = "moveDownToolStripButton";
			this.moveDownToolStripButton.Size = new System.Drawing.Size(91, 22);
			this.moveDownToolStripButton.Text = "Move Down";
			this.moveDownToolStripButton.ToolTipText = "Move the node down one slot in its container. This does not have an effect on com" +
    "plex nodes, as they are unordered.";
			// 
			// copyToolStripButton
			// 
			this.copyToolStripButton.Enabled = false;
			this.copyToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_insert_child;
			this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripButton.Name = "copyToolStripButton";
			this.copyToolStripButton.Size = new System.Drawing.Size(87, 22);
			this.copyToolStripButton.Text = "Copy Node";
			this.copyToolStripButton.ToolTipText = "Copies the selected node to the clipboard";
			// 
			// pasteToolStripButton
			// 
			this.pasteToolStripButton.Enabled = false;
			this.pasteToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_insert_next;
			this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripButton.Name = "pasteToolStripButton";
			this.pasteToolStripButton.Size = new System.Drawing.Size(87, 22);
			this.pasteToolStripButton.Text = "Paste Node";
			this.pasteToolStripButton.ToolTipText = "Pastes a node from the clipboard below the currently selected node";
			// 
			// deleteToolStripButton
			// 
			this.deleteToolStripButton.Enabled = false;
			this.deleteToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_delete_next;
			this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.deleteToolStripButton.Name = "deleteToolStripButton";
			this.deleteToolStripButton.Size = new System.Drawing.Size(92, 22);
			this.deleteToolStripButton.Text = "Delete Node";
			this.deleteToolStripButton.ToolTipText = "Deleted the selected node";
			this.deleteToolStripButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// searchToolStripButton
			// 
			this.searchToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.searchToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.searchToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_magnifier;
			this.searchToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.searchToolStripButton.Name = "searchToolStripButton";
			this.searchToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.searchToolStripButton.Text = "Search";
			this.searchToolStripButton.Click += new System.EventHandler(this.searchToolStripButton_Click);
			// 
			// searchToolStripTextBox
			// 
			this.searchToolStripTextBox.AcceptsReturn = true;
			this.searchToolStripTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.searchToolStripTextBox.ForeColor = System.Drawing.SystemColors.GrayText;
			this.searchToolStripTextBox.Name = "searchToolStripTextBox";
			this.searchToolStripTextBox.Size = new System.Drawing.Size(100, 25);
			this.searchToolStripTextBox.Text = "Search";
			this.searchToolStripTextBox.Leave += new System.EventHandler(this.searchToolStripTextBox_Leave);
			this.searchToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchToolStripTextBox_KeyDown);
			this.searchToolStripTextBox.Click += new System.EventHandler(this.searchToolStripTextBox_Click);
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
			this.splitContainer.Size = new System.Drawing.Size(762, 348);
			this.splitContainer.SplitterDistance = 252;
			this.splitContainer.TabIndex = 2;
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(506, 348);
			this.treeView.TabIndex = 0;
			this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "tnt";
			this.openFileDialog.Filter = "TNT Files|*.*|Compressed TNT Files|*.*";
			this.openFileDialog.Title = "Load File";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "tnt";
			this.saveFileDialog.Filter = "TNT Files|*.*|Compressed TNT Files|*.*";
			this.saveFileDialog.Title = "Save File";
			// 
			// nodeInfoPanel
			// 
			this.nodeInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeInfoPanel.Location = new System.Drawing.Point(0, 0);
			this.nodeInfoPanel.Name = "nodeInfoPanel";
			this.nodeInfoPanel.Size = new System.Drawing.Size(252, 348);
			this.nodeInfoPanel.TabIndex = 0;
			// 
			// addNodeToolStripButton
			// 
			this.addNodeToolStripButton.Image = global::Frost.TntEditor.Properties.Resources.node_design;
			this.addNodeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addNodeToolStripButton.Name = "addNodeToolStripButton";
			this.addNodeToolStripButton.Size = new System.Drawing.Size(81, 22);
			this.addNodeToolStripButton.Text = "Add Node";
			this.addNodeToolStripButton.ToolTipText = "Add a new node to the currently selected node";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(762, 397);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "TNT Editor";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.nodeContextMenuStrip.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripDropDownButton addNodeListToolStripButton;
		private System.Windows.Forms.ToolStripButton moveUpToolStripButton;
		private System.Windows.Forms.ToolStripButton moveDownToolStripButton;
		private System.Windows.Forms.ToolStripButton deleteToolStripButton;
		private System.Windows.Forms.ContextMenuStrip newNodeContextMenuStrip;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
		private System.Windows.Forms.ToolStripButton pasteToolStripButton;
		private System.Windows.Forms.ToolStripTextBox searchToolStripTextBox;
		private System.Windows.Forms.ToolStripButton searchToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton addNodeToolStripButton;
	}
}

