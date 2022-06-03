using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Assessment
{
    internal static class Program
    {
        private static void Main()
        {
            var assessment = new Assessment();

            #region WithMax

            Console.WriteLine("WithMax");

            Console.WriteLine(assessment.WithMax(
                    Enumerable.Range(1, 10)
                        .Select(x => new Score {Value = x * x}))?.Value.ToString()
            );

            Console.WriteLine();

            #endregion

            #region GetAverageOrDefault

            Console.WriteLine("GetAverageOrDefault");

            Console.WriteLine(assessment.GetAverageOrDefault(Enumerable.Range(1, 6)).ToString());

            Console.WriteLine();

            #endregion

            #region WithSuffix

            Console.WriteLine("WithSuffix");

            Console.WriteLine(assessment.WithSuffix("CORRECT_TO", "M_U_N_D_O"));

            Console.WriteLine();

            #endregion

            #region GetFullName

            Console.WriteLine("GetFullName");

            var child = new Hierarchy("John",
                new Hierarchy("Jane",
                    new Hierarchy("Joe",
                        new Hierarchy("Jolene",
                            new Hierarchy("Josh",
                                new Hierarchy("Jake"))))));

            Console.WriteLine(assessment.GetFullName(child));

            Console.WriteLine();

            #endregion

            #region ClosestToAverageOrDefault

            Console.WriteLine("ClosestToAverageOrDefault");

            Console.WriteLine(assessment.ClosestToAverageOrDefault(Enumerable.Range(1, 5)).ToString());

            Console.WriteLine();

            #endregion

            #region Group

            Console.WriteLine("Group");

            var bookingList = new List<Booking>()
            {
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("01/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "HR",
                    Date = DateTime.ParseExact("01/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 10
                },
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("02/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "HR",
                    Date = DateTime.ParseExact("02/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 10
                },
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("03/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "HR",
                    Date = DateTime.ParseExact("03/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("04/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "HR",
                    Date = DateTime.ParseExact("04/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("05/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "HR",
                    Date = DateTime.ParseExact("05/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "ECom",
                    Date = DateTime.ParseExact("05/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("06/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "ECom",
                    Date = DateTime.ParseExact("06/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 10
                },
                new Booking()
                {
                    Project = "CRM",
                    Date = DateTime.ParseExact("07/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 15
                },
                new Booking()
                {
                    Project = "ECom",
                    Date = DateTime.ParseExact("07/02/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allocation = 10
                }
            };

            var bookingGroups = assessment.Group(bookingList);

            foreach (var bookingGroup in bookingGroups)
            {
                Console.Write($"From: {bookingGroup.From.ToShortDateString()}," +
                              $" To:{bookingGroup.To.ToShortDateString()}");
                foreach (var bookingGroupingItem in bookingGroup.Items)
                {
                    Console.Write($", 'Project:{bookingGroupingItem.Project}," +
                                  $" Allocation:{bookingGroupingItem.Allocation.ToString(CultureInfo.InvariantCulture)}'");
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            #endregion

            #region Merge

            Console.WriteLine("Merge");

            var mergedArray = assessment.Merge(new[] {1, 3, 5}, new[] {2, 4, 6});

            Console.WriteLine(string.Join(",", mergedArray));

            Console.WriteLine();

            #endregion
        }
    }

    internal class Hierarchy : IHierarchy
    {
        public IHierarchy Parent { get; }
        public string Name { get; }

        public Hierarchy(string name, IHierarchy parent = null)
        {
            Parent = parent;
            Name = name;
        }
    }
}