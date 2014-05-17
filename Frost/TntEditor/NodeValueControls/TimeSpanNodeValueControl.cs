using System;
using System.Windows.Forms;
using Frost.Tnt;

namespace Frost.TntEditor.NodeValueControls
{
	public partial class TimeSpanNodeValueControl : UserControl, INodeEditorControl
	{
		public TimeSpanNodeValueControl ()
		{
			InitializeComponent();

			var maxDays = Int64.MaxValue / TimeSpan.TicksPerDay;
			daysNumericUpDown.Maximum = maxDays;

			// Create links between controls for overflow
			ticksNumericUpDown.Tag   = msNumericUpDown;
			msNumericUpDown.Tag      = secondsNumericUpDown;
			secondsNumericUpDown.Tag = minutesNumericUpDown;
			minutesNumericUpDown.Tag = hoursNumericUpDown;
			hoursNumericUpDown.Tag   = daysNumericUpDown;
		}

		/// <summary>
		/// Gets a new node that contains the value given in the control
		/// </summary>
		/// <returns>A node with the entered value</returns>
		public Node AsNode ()
		{
			var ticks = (long)ticksNumericUpDown.Value;
			ticks    += (long)msNumericUpDown.Value      * TimeSpan.TicksPerMillisecond;
			ticks    += (long)secondsNumericUpDown.Value * TimeSpan.TicksPerSecond;
			ticks    += (long)minutesNumericUpDown.Value * TimeSpan.TicksPerMinute;
			ticks    += (long)hoursNumericUpDown.Value   * TimeSpan.TicksPerHour;
			ticks    += (long)daysNumericUpDown.Value    * TimeSpan.TicksPerDay;
			if(negativeCheckBox.Checked)
				ticks *= -1L;
			var span = new TimeSpan(ticks);
			return new TimeSpanNode(span);
		}

		/// <summary>
		/// Sets the values contained in the control to the values from <paramref name="node"/>
		/// </summary>
		/// <param name="node">Node to pull values from</param>
		public void FromNode (Node node)
		{
			var span = (TimeSpanNode)node;
			var ticks = span.Value.Ticks;
			if(ticks < 0L)
			{
				negativeCheckBox.Checked = true;
				ticks = Math.Abs(ticks);
			}
			else
				negativeCheckBox.Checked = false;

			var days    = ticks / TimeSpan.TicksPerDay;
			ticks      %= TimeSpan.TicksPerDay;
			var hours   = ticks / TimeSpan.TicksPerHour;
			ticks      %= TimeSpan.TicksPerHour;
			var minutes = ticks / TimeSpan.TicksPerMinute;
			ticks      %= TimeSpan.TicksPerMinute;
			var seconds = ticks / TimeSpan.TicksPerSecond;
			ticks      %= TimeSpan.TicksPerSecond;
			var millis  = ticks / TimeSpan.TicksPerMillisecond;
			ticks      %= TimeSpan.TicksPerMillisecond;

			daysNumericUpDown.Value    = days;
			hoursNumericUpDown.Value   = hours;
			minutesNumericUpDown.Value = minutes;
			secondsNumericUpDown.Value = seconds;
			msNumericUpDown.Value      = millis;
			ticksNumericUpDown.Value   = ticks;
		}

		private void numericUpDown_ValueChanged (object sender, EventArgs e)
		{
			var numericUpDown = (NumericUpDown)sender;
			if(numericUpDown.Value >= numericUpDown.Maximum)
			{// Spill into next
				var next = numericUpDown.Tag as NumericUpDown;
				if(next != null)
				{
					++next.Value;
					numericUpDown.Value = numericUpDown.Minimum;
				}
			}
		}
	}
}
