using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    class Day21
    {
        public int CalcA()
        {
            var (_, list) = CalculateData();

            return list.SelectMany(l => l.ingredients).Count();
        }

        public string CalcB()
        {
            var (ingredientToAllergen, _) = CalculateData();

            var canonicalList = ingredientToAllergen.OrderBy(kvp => kvp.Value).Select(kvp => kvp.Key);

            return string.Join(",", canonicalList);
        }

        private static (Dictionary<string, string> ingredientToAllergen, List<(HashSet<string> ingredients, HashSet<string> allergens)> listAfterExlusionOfAllergen) CalculateData()
        {
            var lines = File.ReadAllLines("Day21.txt");
            var list = new List<(HashSet<string> ingredients, HashSet<string> allergens)>();
            var allergensToMatch = new HashSet<string>();
            foreach (var line in lines)
            {
                var parts = line.Split("(contains ");
                var ingredients = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var allergens = parts[1].Substring(0, parts[1].Length - 1).Split(", ");

                foreach (var a in allergens)
                    allergensToMatch.Add(a);

                list.Add((Enumerable.ToHashSet(ingredients), Enumerable.ToHashSet(allergens)));
            }

            var ingredientToAllergen = new Dictionary<string, string>();
            while (allergensToMatch.Count > 0)
            {
                bool foundMatch = false;
                foreach (var a in allergensToMatch)
                {
                    HashSet<string> ingredientsWithAllergen = null;
                    foreach (var row in list)
                    {
                        if (row.allergens.Contains(a))
                        {
                            ingredientsWithAllergen ??= new HashSet<string>(row.ingredients);
                            ingredientsWithAllergen.IntersectWith(row.ingredients);
                            if (ingredientsWithAllergen.Count == 1)
                            {
                                var ingredient = ingredientsWithAllergen.Single();
                                ingredientToAllergen.Add(ingredient, a);

                                foreach (var (ingredients, allergens) in list)
                                {
                                    ingredients.Remove(ingredient);
                                    allergens.Remove(a);
                                }

                                allergensToMatch.Remove(a);

                                foundMatch = true;
                                break;
                            }
                        }
                    }

                    if (foundMatch)
                        break;
                }
            }

            return (ingredientToAllergen, list);
        }
    }
}