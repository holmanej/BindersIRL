using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterBinderBuilder {
    class Ability {
        public string name;
        public int cost;
        public string description;

        public Ability(string name, int cost, string description) {
            this.name = name;
            this.cost = cost;
            this.description = description;
        }
    }
}
