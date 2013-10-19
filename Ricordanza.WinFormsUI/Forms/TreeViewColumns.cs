using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using System.ComponentModel;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// This projects shows a TreeView user control having columns.
    /// </summary>
    /// <example>
    /// �I�[�o�[���C�p�̃A�C�R���̒ǉ�<br />
    /// <code>
    /// ��.Designer.cs
    /// 
    /// private TreeViewColumns treeViewColumns;
    /// 
    /// private void InitializeComponent()
    /// {
    /// 	this.treeViewColumns = new Ricordanza.WinFormsUI.Forms.TreeViewColumns();
    /// 	this.SuspendLayout();
    /// 	// 
    /// 	// treeViewColumns1
    /// 	// 
    /// 	this.treeViewColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
    /// 	this.treeViewColumns.Location = new System.Drawing.Point(8, 8);
    /// 	this.treeViewColumns.Name = "treeViewColumns1";
    /// 	this.treeViewColumns.Padding = new System.Windows.Forms.Padding(1);
    /// 	this.treeViewColumns.Size = new System.Drawing.Size(438, 184);
    /// 	this.treeViewColumns.TabIndex = 0;
    /// 	// 
    /// 	// Form1
    /// 	// 
    /// 	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
    /// 	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    /// 	this.ClientSize = new System.Drawing.Size(462, 205);
    /// 	this.Controls.Add(this.treeViewColumns);
    /// 	this.Name = "Form1";
    /// 	this.Text = "Example TreeViewColumns (lite)";
    /// 	this.ResumeLayout(false);
    /// }
    /// 
    /// ��.cs
    /// public Form1()
    /// {
    /// 	InitializeComponent();
    /// 
    /// 	TreeNode treeNode = new TreeNode("test");
    /// 	treeNode.Tag = new string[] { "col1", "col2" };
    /// 
    /// 	// Some random node
    /// 	this.treeViewColumns.TreeView.Nodes[0].Nodes[0].Nodes.Add(treeNode);
    /// 
    /// 	this.treeViewColumns.TreeView.SelectedNode = treeNode;
    /// }
    /// </code>
    /// </example>
	public partial class TreeViewColumns : UserControl
	{
		public TreeViewColumns()
		{
			InitializeComponent();

			this.BackColor = VisualStyleInformation.TextControlBorder;
			this.Padding = new Padding(1);
		}

		[Description("TreeView associated with the control"), Category("Behavior")]
		public TreeView TreeView
		{
			get
			{
				return this.treeView1;
			}
		}

		[Description("Columns associated with the control"), Category("Behavior")]
		public ListView.ColumnHeaderCollection Columns
		{
			get
			{
				return this.listView1.Columns;
			}
		}

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			this.treeView1.Focus();
		}

		private void treeView1_Click(object sender, EventArgs e)
		{
			Point p = this.treeView1.PointToClient(Control.MousePosition);
			TreeNode tn = this.treeView1.GetNodeAt(p);
			if (tn != null)
				this.treeView1.SelectedNode = tn;
		}

		private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			this.treeView1.Focus();
			this.treeView1.Invalidate();
		}

		private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			this.treeView1.Focus();
			this.treeView1.Invalidate();
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			e.DrawDefault = true;

			Rectangle rect = e.Bounds;

			if ((e.State & TreeNodeStates.Selected) != 0)
			{
				if ((e.State & TreeNodeStates.Focused) != 0)
					e.Graphics.FillRectangle(SystemBrushes.Highlight, rect);
				else
					e.Graphics.FillRectangle(SystemBrushes.Control, rect);
			}
			else
				e.Graphics.FillRectangle(Brushes.White, rect);

			e.Graphics.DrawRectangle(SystemPens.Control, rect);

			for (int intColumn = 1; intColumn < this.listView1.Columns.Count; intColumn++)
			{
				rect.Offset(this.listView1.Columns[intColumn - 1].Width, 0);
				rect.Width = this.listView1.Columns[intColumn].Width;

				e.Graphics.DrawRectangle(SystemPens.Control, rect);

				string strColumnText;
				string[] list = e.Node.Tag as string[];
				if (list != null && intColumn<=list.Length)
					strColumnText = list[intColumn - 1];
				else
					strColumnText = intColumn + " " + e.Node.Text; // dummy

				TextFormatFlags flags = TextFormatFlags.EndEllipsis;
				switch(this.listView1.Columns[intColumn].TextAlign)
				{
					case HorizontalAlignment.Center:
						flags |= TextFormatFlags.HorizontalCenter;
						break;
					case HorizontalAlignment.Left:
						flags |= TextFormatFlags.Left;
						break;
					case HorizontalAlignment.Right:
						flags |= TextFormatFlags.Right;
						break;
					default:
						break;
				}

				rect.Y++;
				if ((e.State & TreeNodeStates.Selected) != 0 &&
					(e.State & TreeNodeStates.Focused) != 0)
					TextRenderer.DrawText(e.Graphics, strColumnText, e.Node.NodeFont, rect, SystemColors.HighlightText, flags);
				else
					TextRenderer.DrawText(e.Graphics, strColumnText, e.Node.NodeFont, rect, e.Node.ForeColor, e.Node.BackColor, flags);
				rect.Y--;
			}
		}

	}
}