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
    }

    public class AnimalsPresenter : IAnimalsPresenter
    {
        private List<Animal> animals;
        public AnimalsPresenter()
        {
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
    }
}
