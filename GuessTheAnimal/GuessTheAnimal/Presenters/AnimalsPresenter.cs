using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuessTheAnimal.Presenters
{
    public interface IAnimalsPresenter
    {
        IEnumerable<Animal> Animals { get; }
        void addAnimal(Animal animal);
        void guessAnimal();
    }

    public class AnimalsPresenter : IAnimalsPresenter
    {
        private List<Animal> animals;
        private IMainView mainView;
        private static readonly Random rndGenerator = new Random();
        public AnimalsPresenter(IMainView mainView)
        {
            this.mainView = mainView;
            // Setup initial list of animals from a file or defaults
            if (File.Exists("animals.json"))
            {
                using (StreamReader sr = new StreamReader("animals.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    animals = new List<Animal>((Animal[])serializer.Deserialize(sr, typeof(Animal[])));
                }
            }
            else
            {
                animals = new List<Animal>()
                {
                    new Animal() { Name="Elephant", Facts= new List<string>(){ "has a trunk", "trumpets", "is grey" } },
                    new Animal() { Name="Lion", Facts= new List<string>(){ "has a mane", "roars", "is yellow" } }
                };
            }
        }

        public IEnumerable<Animal> Animals { get { return animals; } }

        public void addAnimal(Animal animal)
        {
            if (!animals.Any(x => x.Name.Equals(animal.Name, StringComparison.OrdinalIgnoreCase)))
            {
                animals.Add(animal);
            }

            //Serialize defined animals
            using (StreamWriter file = File.CreateText("animals.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, animals);
            }
        }

        public void guessAnimal()
        {
            mainView.showAnimalList();
            
            List<Animal> selctedAnimals = new List<Animal>(Animals);
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
                List<Animal> newSelctedAnimals = new List<Animal>();
                if (mainView.inquireFact(facts[randomIndex]))
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
            mainView.showGuesedAnimal(selctedAnimals[0].Name);
        }
    }
}
