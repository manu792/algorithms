using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AwsTest
{
    class Program
    {
        private static int total = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            totalScore(0, new List<string>() { "5", "-2", "4", "z", "X", "9", "+", "+" });

            largestItemAssociation(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("item1", "item2"),
                new Tuple<string, string>("item3", "item4"),
                new Tuple<string, string>("item4", "item5"),
                new Tuple<string, string>("item10", "item2"),
            });

            //fizzBuzz(15);

            var cost = getMoneySpent(new int[] { 3, 1, 0, 10, 15, 7, 2, 90 }, new int[] { 5, 2, 8, 1, 100, 2, 5, 7 }, 25);

            minimumBribes(new int[] { 1, 2, 5, 3, 7, 8, 6, 4 });

            var result = twoStrings("helloo", "world");

            abbreviation("ABCdE", "ABCCDE");
        }

        static string abbreviation(string a, string b)
        {
            var arrayA = a.ToArray();
            var arrayB = b.ToArray();

            Array.Sort(arrayA);
            Array.Sort(arrayB);

            StringBuilder sb = new StringBuilder(arrayA.ToString());


            if (!IsBInA(a, b))
                return "NO";

            foreach(var character in arrayB)
            {
                var firstOccurrence = sb.ToString().IndexOf(character.ToString().ToLower());

                if(firstOccurrence > -1) 
                {
                    sb.Replace(character.ToString().ToLower(), character.ToString(), firstOccurrence, character.ToString().ToLower().Length);
                }
            }

            var result = (sb.ToString().Where(x => char.IsUpper(x))).ToString();

            return result;
        }

        private static bool IsBInA(string a, string b)
        {
            foreach(var character in b)
            {
                if (!a.Contains(character))
                    return false;
            }
            return true;
        }

        static string twoStrings(string s1, string s2)
        {
            var dict = s1.GroupBy(x => x.ToString().ToLower()).ToDictionary(x => x.Key, x => x);

            foreach (var character in s2)
                if (dict.ContainsKey(character.ToString().ToLower()))
                    return "YES";

            return "NO";
    }

        private static void minimumBribes(int[] q)
        {
            var totalMovesCount = 0;

            for(var i = 0; i < q.Count(); i++)
            {
                var moveCount = q[i] - (i + 1);

                if(moveCount > 2)
                {
                    Console.WriteLine("Too chaotic");
                    return;
                }

                var index = (q[i] - 2);
                index = index < 0 ? 0 : index;
                
                for(var j = index; j < i; j++)
                    if (q[j] > q[i])
                        totalMovesCount += 1;
            }
            
            Console.WriteLine(totalMovesCount);
        }

        private static int getMoneySpent(int[] keyboards, int[] drives, int b)
        {
            /*
             * Write your code here.
             */

            // Remove any element that equals to b or 0
            var keyboardsList = keyboards.ToList();
            keyboardsList.RemoveAll(x => (x == b || x == 0 || x > b));

            var drivesList = drives.ToList();
            drivesList.RemoveAll(x => (x == b || x == 0 || x > b));

            // Sort lists in ascending order
            keyboardsList = keyboardsList.OrderByDescending(x => x).ToList();
            drivesList = drivesList.OrderByDescending(x => x).ToList();

            var maxCost = -1;

            foreach (var keyboard in keyboardsList)
                foreach(var drive in drivesList)
                {
                    var cost = keyboard + drive;

                    if (cost == b)
                        return cost;
                    else if (cost < b) 
                    {
                        if(cost > maxCost)
                            maxCost = cost;
                        break; // dont waste time checking the other values drives they all will be lesser, so go to next keyboard and start over
                    }
                }

            return maxCost;
        }

        public static void fizzBuzz(int n)
        {
            var numbers = Enumerable.Range(1, n);

            Print(numbers.ToList(), numbers.First(), 0);
        }

        private static void Print(List<int> numbers, int number, int index)
        {
            

            if (number % 3 == 0)
            {
                if(number % 5 == 0)
                    Console.WriteLine("FizzBuzz");
                else 
                    Console.WriteLine("Fizz");
            }

            else if(number % 5 == 0)
                Console.WriteLine("Buzz");
            else
                Console.WriteLine(number);

            index++;
            if (index >= numbers.Count)
                return; 

            Print(numbers, numbers[index], index);
        }



        public static List<string> largestItemAssociation(
                               List<Tuple<string, string>> itemAssociation)
        {
            // WRITE YOUR CODE HERE

            var result = new List<string>();
            var firstItems = new List<string>();

            while (itemAssociation.Count > 0)
            {
                firstItems = new List<string>()
                {
                    itemAssociation[0].Item1,
                    itemAssociation[0].Item2
                };

                itemAssociation.RemoveAt(0);

                for(var item = 0; item < firstItems.Count; item++)
                {
                    for(var j = 0; j < itemAssociation.Count; j++)
                    {
                        var secondItems = new List<string>()
                        {
                            itemAssociation[j].Item1,
                            itemAssociation[j].Item2
                        };

                        if(secondItems.Any(x => x.Equals(firstItems[item], StringComparison.OrdinalIgnoreCase)))
                        {
                            firstItems = firstItems.Concat(secondItems).Distinct().ToList();
                            itemAssociation.RemoveAt(j);
                            j = -1;
                            item = 0;
                        }
                    }
                }

                if (firstItems.Count > result.Count)
                    result = firstItems.ToList();
                else if (firstItems.Count == result.Count)
                    result = GetOrderedLexicographically(firstItems, result);
            }

            return result;
        }

        private static List<string> GetOrderedLexicographically(List<string> firstItems, List<string> result)
        {
            if (result.Count <= 0)
                return result;

            var textF = string.Join("", firstItems);

            var textR = string.Join("", result);

            var lexicographic = string.Compare(textF, textR);
            if (lexicographic == 0)
                return result;
            else if (lexicographic > 0)
                return result;
            else
                return firstItems;
        }

        private static int Calculate(int num, List<string> blocks, string score, int previousScore, int secondPreviousScore, int index)
        {
            int actualScore = 0;
            bool removed = false;

            if (index >= blocks.Count)
                return actualScore;

            var next = index + 1;

            if (blocks.ElementAtOrDefault(next) != "z")
            {
                if (!int.TryParse(score, out actualScore))
                {
                    // check if its any of the valid possible signs
                    if (score.Equals("x", StringComparison.OrdinalIgnoreCase))
                        actualScore = previousScore * 2;

                    else if (score.Equals("+", StringComparison.OrdinalIgnoreCase))
                        actualScore = previousScore + secondPreviousScore;
                }

                index += 1;
            }
            else
            {
                index += 2;
                actualScore = 0;
                removed = true;
            }

            
            return actualScore + Calculate(num, blocks, blocks.ElementAtOrDefault(index), !removed ? actualScore : previousScore, !removed ? previousScore : secondPreviousScore, index);
        }

        public static int totalScore(int num, List<string> blocks)
        {
            // WRITE YOUR CODE HERE
            // int totalA = 0;

            var s = Calculate(num, blocks, blocks.FirstOrDefault(), 0, 0, 0);

            var previousScores = new List<int>();

            var total = 0;

            foreach (var score in blocks)
            {
                int actualScore;

                if (int.TryParse(score, out actualScore))
                {
                    total += actualScore;

                    previousScores.Add(actualScore);
                }
                else
                {
                    // check if its any of the valid possible signs
                    if (score.Equals("x", StringComparison.OrdinalIgnoreCase))
                    {
                        var lastScore = previousScores.LastOrDefault();

                        var currentScore = lastScore * 2;

                        total += currentScore;

                        previousScores.Add(currentScore);
                    }

                    else if (score.Equals("z", StringComparison.OrdinalIgnoreCase))
                    {
                        var lastScore = previousScores.LastOrDefault();

                        total -= lastScore; // substract last score number from total

                        previousScores.RemoveAt(previousScores.Count - 1); // remove last score from list of scores
                    }

                    else if (score.Equals("+", StringComparison.OrdinalIgnoreCase))
                    {
                        var lastScore = previousScores.LastOrDefault();
                        var secondLast = previousScores.ElementAtOrDefault(previousScores.Count - 2); // get the score before the last one

                        var currentScore = lastScore + secondLast;

                        total += currentScore;
                        previousScores.Add(currentScore);
                    }
                }
            }

            return total;
        }
    }
}
