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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAnimalsPresenter presenter;
        private static readonly Random rndGenerator = new Random();
        public MainWindow(IAnimalsPresenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = "Choose one of the listed animals in your head:\n";
            foreach (Animal animal in presenter.Animals)
            {
                message += "  > " + animal.Name + "\n";
            }
            MessageBox.Show(message,
                "Chose Animal",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            List<Animal> selctedAnimals = new List<Animal>(presenter.Animals);
            while (selctedAnimals.Count > 1)
            {
                List<string> facts = new List<string>();
                foreach (Animal animal in selctedAnimals)
                {
                    foreach (string fact in animal.Facts)
                    {
                        facts.Add(fact); // May get duplicates, OK
                    }
                }

                int randomIndex = rndGenerator.Next(0, facts.Count);
                message = "Is your animal " + facts[randomIndex];
                List<Animal> newSelctedAnimals = new List<Animal>();
                if (MessageBox.Show(message, "Fact", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach (Animal animal in selctedAnimals)
                    {
                        if (animal.Facts.Any(x => x.Equals(facts[randomIndex], StringComparison.OrdinalIgnoreCase)))
                            newSelctedAnimals.Add(animal);
                    }
                }
                else
                {
                    foreach (Animal animal in selctedAnimals)
                    {
                        if (!animal.Facts.Any(x => x.Equals(facts[randomIndex], StringComparison.OrdinalIgnoreCase)))
                            newSelctedAnimals.Add(animal);
                    }
                }
                selctedAnimals = newSelctedAnimals;
            }
            MessageBox.Show("The animal you chose was " + selctedAnimals[0].Name, "Found animal", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void addAnimalBtn_Click(object sender, RoutedEventArgs e)
        {
            AddAnimalWindow dialog = new AddAnimalWindow(presenter);
            dialog.ShowDialog();
        }
    }
}
