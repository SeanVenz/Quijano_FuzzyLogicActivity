using System;
using System.Windows.Forms;
using InferenceLibrary;

namespace ItoFuzzyLogicInference
{
    public partial class MainForm
    {
        private void inputValuesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != VariableInputCellIndex)
            {
                return;
            }

            var row = inputValuesGrid.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];
            var variable = (LinguisticVariable)row.Tag;
            var conditions = DataRepository.ConditionsForVariable(variable);
            foreach (var condition in conditions)
            {
                condition.InputValue = double.Parse(cell.Value.ToString());
            }
        }

        private void inputValuesGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != VariableInputCellIndex)
            {
                return;
            }
            var row = inputValuesGrid.Rows[e.RowIndex];
            double parseResult;
            if (!double.TryParse(e.FormattedValue.ToString(), out parseResult))
            {
                row.ErrorText = $"Enter a real number!";
                e.Cancel = true;
            }
            else
            {
                row.ErrorText = "";
            }
        }

        private void decisionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DataRepository.Initialized || !this.Visible)
            {
                return;
            }
            var variable = CurrentVariable();

            InitializeInference(variable);
        }
    }
}
