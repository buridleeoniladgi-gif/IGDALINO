using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IgdalinoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string[] names = new string[100];
        string[] addresses = new string[100];
        string[] ordermeals = new string[100];
        string[] Quantity = new string[100];
        char status = 'A';

        int index = 0;
        int updatedIndex = -1;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string Quantity = "";
            string name = txtCustomername.Text;
            string address = txtAddress.Text; ;
            string ordermeals = comboBoxOrdermeals.Text;
            if (rbOne.IsChecked == true) Quantity = "1";
            else if (rbTwo.IsChecked == true) Quantity = "2";
            else if (rbThree.IsChecked == true) Quantity = "3";
            else if (rbFour.IsChecked == true) Quantity = "4";


            string data = $"{name} - {address} - {ordermeals} = {Quantity}";

            if (name == "" || address == "" || ordermeals == "" || Quantity == "")
            {
                MessageBox.Show("Please fill all fields", "Customer Data", MessageBoxButton.OK);
                return;
            }
            SaveData(name, address, ordermeals, Quantity);
            ClearData();
        }
        /**
         * Clears the form data after the sava action
         */
        private void ClearData()
        {
            txtCustomername.Clear();
            txtAddress.Clear();
            comboBoxOrdermeals.SelectedIndex = -1;
            rbOne.IsChecked = false;
            rbTwo.IsChecked = false;
            rbThree.IsChecked = false;
            rbFour.IsChecked = false;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex >= 0)
            {
                txtCustomername.Text = names[selectedIndex];
                txtAddress.Text = addresses[selectedIndex];
                comboBoxOrdermeals.Text = ordermeals[selectedIndex];

                if (Quantity[selectedIndex] == "1") rbOne.IsChecked = true;
                else if (Quantity[selectedIndex] == "2") rbTwo.IsChecked = true;
                else if (Quantity[selectedIndex] == "3") rbThree.IsChecked = true;
                else if (Quantity[selectedIndex] == "4") rbFour.IsChecked = true;

                status = 'E'; // EDIT mode
                updatedIndex = selectedIndex;
                btnDeleteData.IsEnabled = true;
            }
        }
        /**
         * Perform the save action. There are two save action here.
         * Add - Add the new data to the array
         * Update - updates the data from the array
         */
        private void SaveData(string n, string a, string o, string Q)
        {
            if (status == 'A') // Adding new row
            {
                names[index] = n;
                addresses[index] = a;
                ordermeals[index] = o;
                Quantity[index] = Q;

                dataGrid.Items.Add(new
                {
                    Customername = names[index],
                    Address = addresses[index],
                    OrderMeals = ordermeals[index],
                    Quantity = Quantity[index]
                });

                index++;
                MessageBox.Show("New data successfully added!", "Customer Data", MessageBoxButton.OK);
            }
            else if (status == 'E' && updatedIndex >= 0) // Editing existing row
            {
                names[updatedIndex] = n;
                addresses[updatedIndex] = a;
                ordermeals[updatedIndex] = o;
                Quantity[updatedIndex] = Q;

                RefreshGrid(); // Refresh the DataGrid to reflect changes
                status = 'A'; // Reset status
                updatedIndex = -1;
                btnDeleteData.IsEnabled = false;
                ClearData();
                MessageBox.Show("Data successfully updated!", "Customer Form", MessageBoxButton.OK);
            }
        }

        private void btnDeleteData_Click(object sender, RoutedEventArgs e)
        {
            int deleteIndex = dataGrid.SelectedIndex;

            if (deleteIndex == -1)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }
            ShiftElements(deleteIndex);
            //decrement the size
            index--;
            //update the grid
            RefreshGrid();
            //disables the delete button after deleting
            btnDeleteData.IsEnabled = false;
            //clears the data
            ClearData();
            MessageBox.Show("Customer data deleted successfully!", "Customer Form", MessageBoxButton.OK);
        }

        /**
         * This function refreshes the data grid
         */
        private void RefreshGrid()
        {
            dataGrid.Items.Clear();
            for (int i = 0; i < index; i++)
            {
                dataGrid.Items.Add(new
                {
                    Customername = names[i],
                    Address = addresses[i],
                    OrderMeals = ordermeals[i],
                    Quantity = Quantity[i]
                });
            }
        }

        /**
         * This function shifts the elements to the left after performing delete
         */
        private void ShiftElements(int deletedIndex)
        {
            for (int i = deletedIndex; i < index; i++)
            {
                names[i] = names[i + 1];
                addresses[i] = addresses[i + 1];
                ordermeals[i] = ordermeals[i + 1];
                Quantity[i] = Quantity[i + 1];
            }
        }
    }
}