﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Foreman
{	
	public partial class MainForm : Form
	{

		public MainForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			DataCache.LoadRecipes();

			rateOptionsDropDown.SelectedIndex = 0;

			ItemListBox.Items.Clear();
			ItemListBox.Items.AddRange(DataCache.Items.Keys.ToArray());
			ItemListBox.Sorted = true;
		}

		private void ItemListForm_KeyDown(object sender, KeyEventArgs e)
		{
#if DEBUG
			if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
#endif
		}

		private void AddItemButton_Click(object sender, EventArgs e)
		{
			List<Item> selectedItems = new List<Item>();

			foreach (String itemName in ItemListBox.SelectedItems)
			{
				selectedItems.Add(DataCache.Items[itemName]);
			}
			GraphViewer.AddDemands(selectedItems);
		}

		private void RemoveNodeButton_Click(object sender, EventArgs e)
		{
			GraphViewer.DeleteNode(GraphViewer.SelectedNode);
		}

		private void rateButton_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked)
			{
				this.GraphViewer.graph.SelectedAmountType = AmountType.Rate;
				rateOptionsDropDown.Enabled = true;
			}
			else
			{
				rateOptionsDropDown.Enabled = false;
			}
			GraphViewer.Invalidate();
		}

		private void fixedAmountButton_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked)
			{
				this.GraphViewer.graph.SelectedAmountType = AmountType.FixedAmount;
			}
			GraphViewer.Invalidate();
		}

		private void rateOptionsDropDown_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch ((sender as ComboBox).SelectedIndex)
			{
				case 0:
					GraphViewer.graph.SelectedUnit = RateUnit.PerSecond;
					GraphViewer.Invalidate();
					break;
				case 1:
					GraphViewer.graph.SelectedUnit = RateUnit.PerMinute;
					GraphViewer.Invalidate();
					break;
			}
		}

		private void AutomaticCompleteButton_Click(object sender, EventArgs e)
		{
			GraphViewer.graph.SatisfyAllItemDemands();
			GraphViewer.CreateMissingControls();
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			GraphViewer.graph.Nodes.Clear();
			GraphViewer.nodeControls.Clear();
		}
	}
}
