using GuessTheAnimal.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GuessTheAnimal
{
    /// <summary>
    /// Interaction logic for AddAnimalWindow.xaml
    /// </summary>
    public partial class AddAnimalWindow : Window
    {
        private IAnimalsPresenter presenter;
        public AddAnimalWindow(IAnimalsPresenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            List<string> animals = new List<string>();
            foreach(Animal animal in presenter.Animals)
            {
                animals.Add(" > " + animal.Name + ": " + string.Join(",", animal.Facts));
            }
            icDefinedAnimals.ItemsSource = animals;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtFacts.Text))
                return;

            if(presenter.Animals.Any(x => x.Name.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("The animal " + txtName.Text + " has already defined", "Animal already defined", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Animal newAnimal = new Animal();
            newAnimal.Name = txtName.Text;
            newAnimal.Facts = txtFacts.Text.Split(',').ToList();
            presenter.addAnimal(newAnimal);

            this.DialogResult = true;
        }
    }
}
