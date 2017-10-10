using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheAnimal
{
    /// Animal data model
    public class Animal
    {
        public string Name { get; set; }
        public List<string> Facts { get; set; }
    }
}
