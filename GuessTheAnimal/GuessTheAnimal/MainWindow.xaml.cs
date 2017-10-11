using GuessTheAnimal.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GuessTheAnimal
{
    public interface IMainView
    {
        void showAnimalList();
        bool inquireFact(string fact);
        void showGuesedAnimal(string animal);
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private IAnimalsPresenter presenter;
        public MainWindow()
        {
            InitializeComponent();
            this.presenter = new AnimalsPresenter(this);
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.guessAnimal();
        }

        private void addAnimalBtn_Click(object sender, RoutedEventArgs e)
        {
            AddAnimalWindow dialog = new AddAnimalWindow(presenter);
            dialog.ShowDialog();
        }

        public void showAnimalList()
        {
            string message = "Choose one of the listed animals in your head:\n";
            foreach (Animal animal in presenter.Animals)
            {
                message += "  > " + animal.Name + "\n";
            }

            MessageBox.Show(message,
                "Choose Animal",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public bool inquireFact(string fact)
        {
            string message = "Is your animal " + fact;
            return MessageBox.Show(message, "Fact verification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void showGuesedAnimal(string animal)
        {
            MessageBox.Show("The animal you chose was " + animal, "Found animal!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
