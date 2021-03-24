using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Employee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     
    //JAN BEZOUŠKA

    public partial class MainWindow : Window
    {
        List<EmployeeClass> employeesList = new List<EmployeeClass>();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 1920; i <= 2006; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                cbBirthyear.Items.Add(item);
            }
        }

        private void butOK_Click(object sender, RoutedEventArgs e)
        {
            NewEmployee();
        }
        private void butLog_Click(object sender, RoutedEventArgs e)
        {
            LogEmployees();
        }

        private void tbWage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-ZáčďéěíňóřšťůúýžÁČĎÉÍŇÓŘŠŤÚŽ]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-ZáčďéěíňóřšťůúýžÁČĎÉÍŇÓŘŠŤÚŽ]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void NewEmployee()
        {
            tbLog.Text = string.Empty;

            if (tbName.Text == "" | tbSurname.Text == "" |  tbWage.Text == "" | cbBirthyear.SelectedIndex == -1 | cbEducation.SelectedIndex == -1 | cbPosition.SelectedIndex == -1)
            {
                tbLog.Foreground = Brushes.Red;
                tbLog.Text = "prosím vyplňte všecha pole";
            }
            else
            {
                EmployeeClass newEmployee = new EmployeeClass(cbEducation.Text, cbPosition.Text, Convert.ToInt32(tbWage.Text), tbName.Text, tbSurname.Text, Convert.ToInt32(cbBirthyear.Text));

                if (newEmployee.errLongName | newEmployee.errShortName | newEmployee.errLongSurname | newEmployee.errShortSurname | newEmployee.errWageBig | newEmployee.errWageSmall)
                {
                    tbLog.Foreground = Brushes.Red;

                    if (newEmployee.errLongName)
                        tbLog.Text = "Moc dlouhé jméno\n";
                    else if (newEmployee.errShortName)
                        tbLog.Text = "Moc krátké jméno\n";
                    if (newEmployee.errLongSurname)
                        tbLog.Text += "Moc dlouhé příjmení\n";
                    else if (newEmployee.errShortSurname)
                        tbLog.Text += "Moc krátké příjmení\n";
                    if (newEmployee.errWageBig)
                        tbLog.Text += "Moc velký plat\n";
                    else if (newEmployee.errWageSmall)
                        tbLog.Text += "Moc malý plat\n";

                }
                else if (employeesList.Any(x => (x.FirstName.ToLower() == newEmployee.FirstName.ToLower()) && (x.Surname.ToLower() == newEmployee.Surname.ToLower()) && (x.birthyear == newEmployee.birthyear)))
                {
                    tbLog.Foreground = Brushes.Red;
                    tbLog.Text = "už je v listu";
                }
                else
                {
                    employeesList.Add(newEmployee);
                    tbLog.Foreground = Brushes.Black;
                    tbLog.Text = "zaměstnanec přidán";

                    tbName.Text = string.Empty;
                    tbSurname.Text = string.Empty;
                    tbWage.Text = string.Empty;
                    cbBirthyear.SelectedIndex = -1;
                    cbEducation.SelectedIndex = -1;
                    cbPosition.SelectedIndex = -1;
                }
            }
        }

        public void LogEmployees()
        {
            tbLog.Text = string.Empty;

            if (employeesList.Count > 0)
            {
                for (int i = 0; i < employeesList.Count; i++)
                {
                    EmployeeClass emp = employeesList[i];
                    tbLog.Text += $"{i+1}) {emp.FirstName} {emp.Surname}, {emp.birthyear}, {emp.education}, {emp.position}, {emp.Wage}";
                    tbLog.Text += "\n";
                }
            }
            else
                tbLog.Text = "Prázdný list";
        }
    }

    class Person
    {
        public bool errShortName = false;
        public bool errLongName = false;
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value.Length < 2)
                    errShortName = true;
                else if (value.Length > 20)
                    errLongName = true;
                else
                    firstName = value;
            }
        }

        public bool errShortSurname = false;
        public bool errLongSurname = false;
        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value.Length < 2)
                    errShortSurname = true;
                else if (value.Length > 20)
                    errLongSurname = true;
                else
                    surname = value;
            }
        }

        public int birthyear;

        public Person(string _firstName, string _surname, int _birthyear)
        {
            FirstName = _firstName;
            Surname = _surname;
            birthyear = _birthyear;
        }
    }

    class EmployeeClass : Person
    {
        public string education;
        public string position;

        public bool errWageSmall = false;
        public bool errWageBig = false;
        private int wage;
        public int Wage
        {
            get { return wage; }
            set
            {
                if (value < 14600)
                    errWageSmall = true;
                else if (value > 200000) //kdyžtak změnit max plat
                    errWageBig = true;
                else
                    wage = value;
            }
        }

        public EmployeeClass(string _education, string _position, int _wage, string _firstName, string _surname, int _birthyear) : base(_firstName, _surname, _birthyear)
        {
            education = _education;
            position = _position;
            Wage = _wage;
        }
    }
}
