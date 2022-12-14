using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using InferenceLibrary;

namespace ItoFuzzyLogicInference
{
    public partial class MainForm : Form
    {
        private const string ConfigurationFilePath = "inference.xml";
        private const int VariableInputCellIndex = 1;

        public DataRepository DataRepository { get; set; }
        public FuzzyInference FuzzyInference { get; private set; }
        
        public MainForm()
        {
            InitializeComponent();
            tabControl.TabPages.Remove(resultsTabPage);
            LoadData();
        }


        private void LoadData()
        {
            DataRepository = new DataRepository();
            var factory = new InferenceFactory(ConfigurationFilePath);
            factory.LoadData();
            DataRepository.FuzzyRules = factory.FuzzyRules;
            DataRepository.LinguisticVariables = factory.LinguisticVariables;

            PopulateDecisionTypeComboBox();
            PopulateVariableTree();
            InitializeInference(DataRepository.ConclusionVariables.First());
        }

        private void InitializeInference(LinguisticVariable variable)
        {
            FuzzyInference = new FuzzyInference(DataRepository.RulesForVariable(variable));

            PopulateVariableInputs(variable);
            PopulateRulesList();
        }

        private void PopulateRulesList()
        {
            rulesListBox.Items.Clear();
            foreach (var fuzzyRule in FuzzyInference.Rules)
            {
                rulesListBox.Items.Add(fuzzyRule.Text);
            }
        }

        private void PopulateVariableTree()
        {
            variablesTreeView.Nodes.Clear();
            foreach (var linguisticVariable in DataRepository.LinguisticVariables)
            {
                variablesTreeView.Nodes.Add(BuildVariableTreeNode(linguisticVariable));
            }
            FunctionNodeClicked(variablesTreeView.Nodes[0].FirstNode);
        }

        private TreeNode BuildVariableTreeNode(LinguisticVariable variable)
        {
            var variableNode = new TreeNode(variable.DisplayName);
            foreach (var membershipFunction in variable.MembershipFunctions)
            {
                variableNode.Nodes.Add(BuildFunctionTreeNode(membershipFunction));
            }
            variableNode.Tag = variable;

            return variableNode;
        }

        private TreeNode BuildFunctionTreeNode(MembershipFunction membershipFunction)
        {
            var functionNode = new TreeNode(membershipFunction.DisplayName);
            functionNode.Tag = membershipFunction;
            return functionNode;
        }

        private void PopulateVariableInputs(LinguisticVariable outputVariable)
        {
            inputValuesGrid.Rows.Clear();
            foreach (var variable in DataRepository.InputVariables(outputVariable))
            {
                var row = new DataGridViewRow {Tag = variable};
                var nameCell = new DataGridViewTextBoxCell {Value = variable.DisplayName};

                var valueCell = new DataGridViewTextBoxCell
                {
                    ValueType = typeof(double),
                    Value = 0
                };
                var unitCell = new DataGridViewTextBoxCell
                {
                    Value = variable.DisplayUnit
                };
                row.Cells.Add(nameCell);
                row.Cells.Add(valueCell);
                row.Cells.Add(unitCell);
                inputValuesGrid.Rows.Add(row);
            }
        }

        private void PopulateDecisionTypeComboBox()
        {
            decisionTypeComboBox.DataSource = DataRepository.ConclusionVariables.ToList();
            decisionTypeComboBox.DisplayMember = "DisplayName";
            decisionTypeComboBox.ValueMember = "Id";
        }

        private LinguisticVariable CurrentVariable()
        {
            return (LinguisticVariable)decisionTypeComboBox.SelectedItem;
        }
    }
}
