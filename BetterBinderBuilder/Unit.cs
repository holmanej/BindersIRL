using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterBinderBuilder {
    class Unit {
        public string id;
        public string name;
        public string level;
        public string cost;
        public string health;
        public string mana;
        public string damage;
        public string damageType;
        public string attackRange;
        public string attackSpeed;
        public string armorType;
        public string moveSpeed;
        public string[] abilities;
        public string[] children;
        public string[] parents;
        
        public Unit() {
        }

        public Unit(string id, string name, string level, string cost, string health, string mana, string damage, string damageType, string attackRange, string attackSpeed, string armorType, string moveSpeed, string[] abilities, string[] children, string[] parents) {
            this.id = id;
            this.name = name;
            this.level = level;
            this.cost = cost;
            this.health = health;
            this.mana = mana;
            this.damage = damage;
            this.damageType = damageType;
            this.attackRange = attackRange;
            this.attackSpeed = attackSpeed;
            this.armorType = armorType;
            this.moveSpeed = moveSpeed;
            this.abilities = abilities;
            this.children = children;
            this.parents = parents;
        }

        public void PrintUnit() {
            Console.WriteLine(id);
            Console.WriteLine(name);
            Console.WriteLine(level);
            Console.WriteLine(cost);
            Console.WriteLine(health);
            Console.WriteLine(mana);
            Console.WriteLine(damage);
            Console.WriteLine(damage);
            Console.WriteLine(damageType);
            Console.WriteLine(attackRange);
            Console.WriteLine(attackSpeed);
            Console.WriteLine(armorType);
            Console.WriteLine(moveSpeed);
            foreach (string ability in abilities) Console.WriteLine(ability);
            foreach (string child in children) Console.WriteLine(child);
            foreach (string parent in parents) Console.WriteLine(parent);
            Console.WriteLine();
        }
    }
}
