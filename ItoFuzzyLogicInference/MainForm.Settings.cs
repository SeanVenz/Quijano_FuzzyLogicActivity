using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using InferenceLibrary;

namespace ItoFuzzyLogicInference
{
    public partial class MainForm
    {
        private void xValueTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var textBox = (TextBox)sender;
            double parseResult;
            var parseSuccess = double.TryParse(textBox.Text, out parseResult);
            if (!parseSuccess)
            {
                MessageBox.Show(this, "Please enter a real number");
                e.Cancel = true;
            }
        }
        
        private void x1ValueTextBox_Validated(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            var function = (MembershipFunction)textBox.Tag;
            function.X1 = double.Parse(textBox.Text);
            UpdateFunctionControls(function);
        }

        private void x2ValueTextBox_Validated(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            var function = (MembershipFunction)textBox.Tag;
            function.X2 = double.Parse(textBox.Text);
            UpdateFunctionControls(function);
        }

        private void x3ValueTextBox_Validated(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            var function = (MembershipFunction)textBox.Tag;
            function.X3 = double.Parse(textBox.Text);
            UpdateFunctionControls(function);
        }

        private void x4ValueTextBox_Validated(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            var function = (MembershipFunction)textBox.Tag;
            function.X4 = double.Parse(textBox.Text);
            UpdateFunctionControls(function);
        }

        void UpdateFunctionControls(MembershipFunction function)
        {
            UpdateXLabels(function.Inverted);
            UpdateMembershipFunctionChart(function);
            UpdateRangeCheckBoxes(function);
        }

        private void UpdateMembershipFunctionChart(MembershipFunction function)
        {
            var series = membershipFunctionChart.Series.First();
            series.Points.Clear();
            if (function.X1 != function.X2 || !function.IncludeX3)
            {
                series.Points.AddXY(function.X1, function.Inverted ? 1 : 0);
            }
            series.Points.AddXY(function.X2, function.Inverted ? 0 : 1);
            series.Points.AddXY(function.X3, function.Inverted ? 0 : 1);
            if (function.X3 != function.X4 || !function.IncludeX3)
            {
                series.Points.AddXY(function.X4, function.Inverted ? 1 : 0);
            }

            series.Name = function.LinguisticVariable.DisplayName + " - " + function.DisplayName;
            membershipFunctionChart.ChartAreas.First().AxisX.Minimum = function.X1;
            membershipFunctionChart.ChartAreas.First().AxisX.Maximum = function.X4;
        }

        private void VariableNodeClicked(TreeNode node)
        {
        }

        private void FunctionNodeClicked(TreeNode node)
        {
            var function = (MembershipFunction)node.Tag;
            var variable = function.LinguisticVariable;
            x1UnitLabel.Text = x2UnitLabel.Text = x3UnitLabel.Text = x4UnitLabel.Text = variable.DisplayUnit;
            
            ignoreX1Checkbox.Tag = ignoreX4Checkbox.Tag = includeX2CheckBox.Tag = includeX3CheckBox.Tag = invertTrapezoidCheckbox.Tag = x1ValueTextBox.Tag = x2ValueTextBox.Tag = x3ValueTextBox.Tag = x4ValueTextBox.Tag = function;

            invertTrapezoidCheckbox.Checked = function.Inverted;

            x1ValueTextBox.Text = function.X1.ToString();
            x2ValueTextBox.Text = function.X2.ToString();
            x3ValueTextBox.Text = function.X3.ToString();
            x4ValueTextBox.Text = function.X4.ToString();
            UpdateFunctionControls(function);
        }

        private void variablesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag.GetType() == typeof(LinguisticVariable))
            {
                VariableNodeClicked(e.Node);
            }
            else if (e.Node.Tag.GetType() == typeof(MembershipFunction))
            {
                FunctionNodeClicked(e.Node);
            }
        }

        private void UpdateXLabels(bool inverseTrapezoid)
        {
            if (inverseTrapezoid)
            {
                x1Label.Text = "x1 (left 1):";
                x2Label.Text = "x2 (left 0):";
                x3Label.Text = "x3 (right 0):";
                x4Label.Text = "x4 (right 1):";
            }
            else
            {
                x1Label.Text = "x1 (left 0):";
                x2Label.Text = "x2 (left 1):";
                x3Label.Text = "x3 (right 1):";
                x4Label.Text = "x4 (right 0):";
            }
        }

        private void UpdateRangeCheckBoxes(MembershipFunction function)
        {
            if (function.X1 == function.X2)
            {
                ignoreX1Checkbox.Show();
                ignoreX1Checkbox.Checked = function.IgnoreX1;
                includeX2CheckBox.Show();
                includeX2CheckBox.Checked = function.IncludeX2;
            }
            else
            {
                ignoreX1Checkbox.Hide();
                includeX2CheckBox.Hide();
            }

            if (function.X3 == function.X4)
            {
                ignoreX4Checkbox.Show();
                ignoreX4Checkbox.Checked = function.IgnoreX4;
                includeX3CheckBox.Show();
                includeX3CheckBox.Checked = function.IncludeX3;
            }
            else
            {
                ignoreX4Checkbox.Hide();
                includeX3CheckBox.Hide();
            }
        }

        private void invertTrapezoidCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var function = (MembershipFunction) invertTrapezoidCheckbox.Tag;
            function.Inverted = invertTrapezoidCheckbox.Checked;
            UpdateFunctionControls(function);
        }
        
        private void ignoreX1Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            var function = (MembershipFunction) checkBox.Tag;
            function.IgnoreX1 = checkBox.Checked;
            UpdateFunctionControls(function);
        }

        private void ignoreX4Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            var function = (MembershipFunction)checkBox.Tag;
            function.IgnoreX4 = checkBox.Checked;
            UpdateFunctionControls(function);
        }

        private void includeX2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            var function = (MembershipFunction)checkBox.Tag;
            function.IncludeX2 = checkBox.Checked;
            UpdateFunctionControls(function);
        }

        private void includeX3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            var function = (MembershipFunction)checkBox.Tag;
            function.IncludeX3 = checkBox.Checked;
            UpdateFunctionControls(function);
        }
    }
}
