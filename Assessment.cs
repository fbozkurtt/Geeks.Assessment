using System;
using System.Collections.Generic;
using System.Linq;

namespace Assessment
{
    public class Assessment : IAssessment
    {
        #region Methods

#nullable enable
        /// <summary>
        /// Returns the score with the highest value
        /// </summary>
        public Score? WithMax(IEnumerable<Score> scores)
        {
            return scores.Max();
        }
#nullable disable

        /// <summary>
        /// Returns the average value of the collection. For an empty collection it returns null
        /// </summary>
        public double? GetAverageOrDefault(IEnumerable<int> items)
        {
            var itemsList = items?.ToList();
            var nullOrEmpty = items is null || itemsList.Any() is false;

            if (nullOrEmpty)
                return null;
            return itemsList.Average();
        }


        /// <summary>
        /// Appends the suffix value to the text if the text has value. If not, it returns empty.
        /// </summary>
        public string WithSuffix(string text, string suffixValue)
        {
            if (text is null)
                return string.Empty;
            return string.Concat(text, suffixValue);
        }

        /// <summary>
        /// It fetches all the data from the source.
        /// </summary>
        /// <param name="source">The source data provider returns items by page. NextPageToken is the page token of the next page. If there are no more items to return, nextPageToken will be empty. Passing a null or empty string to the provider will return the first page of the data.
        /// If no value is specified for nextPageToken, the provider will return the first page.
        /// </param>
        /// <returns></returns>
        public IEnumerable<Score> GetAllScoresFrom(IDataProvider<Score> source)
        {
            var scores = new List<Score>();

            IDataProvider<Score>.IDataProviderResponse response = null;

            do
            {
                response = source.GetData(response?.NextPageToken);
                scores.AddRange(response.Items);
            } while (string.IsNullOrWhiteSpace(response.NextPageToken) is false);

            return scores;
        }

        /// <summary>
        /// Returns child's name prefixed with all its parents' names separated by the specified separator.Example : Parent/Child
        /// </summary>
        public string GetFullName(IHierarchy child, string separator = null)
        {
            separator ??= "/";

            if (child.Parent is null)
                return child.Name;
            return string.Concat(GetFullName(child.Parent, separator), separator, child.Name);
        }

        /// <summary>
        /// Refactor: Returns the value that is closest to the average value of the specified numbers.
        /// </summary>
        public int? ClosestToAverageOrDefault(IEnumerable<int> numbers)
        {
            var numberArray = numbers as int[] ?? numbers.ToArray();
            var average = numberArray.Average();

            var result = numberArray
                .Select(n => new {Value = n, Distance = Math.Abs(average - n)})
                .GroupBy(g => g.Distance)
                .OrderBy(g => g.Key)
                .ToList();

            if (result.FirstOrDefault()?.Distinct().Count() > 1)
                return null;

            return result.FirstOrDefault()?.FirstOrDefault()?.Value;
        }

        /// <summary>
        /// Returns date ranges that have similar bookings on each day in the range.
        /// Read the example carefully.
        /// Example : [{Project:HR, Date: 01/02/2020 , Allocation: 10},
        ///            {Project:CRM, Date: 01/02/2020 , Allocation: 15},
        ///            {Project:HR, Date: 02/02/2020 , Allocation: 10},
        ///            {Project:CRM, Date: 02/02/2020 , Allocation: 15},
        ///            {Project:HR, Date: 03/02/2020 , Allocation: 15},
        ///            {Project:CRM, Date: 03/02/2020 , Allocation: 15},
        ///            {Project:HR, Date: 04/02/2020 , Allocation: 15},
        ///            {Project:CRM, Date: 04/02/2020 , Allocation: 15},
        ///            {Project:HR, Date: 05/02/2020 , Allocation: 15},
        ///            {Project:CRM, Date: 05/02/2020 , Allocation: 15},
        ///            {Project:ECom, Date: 05/02/2020 , Allocation: 15},
        ///            {Project:ECom, Date: 06/02/2020 , Allocation: 10},
        ///            {Project:CRM, Date: 06/02/2020 , Allocation: 15}
        ///            {Project:ECom, Date: 07/02/2020 , Allocation: 10},
        ///            {Project:CRM, Date: 07/02/2020 , Allocation: 15}]    
        /// Returns : 
        ///          [
        ///            { From:01/02/2020 , To:02/02/2020 , [{ Project:CRM , Allocation:15 },{ Project:HR , Allocation:10 }]  },
        ///            { From:03/02/2020 , To:04/02/2020 , [{ Project:CRM , Allocation:15 },{ Project:HR , Allocation:15 }]  },
        ///            { From:05/02/2020 , To:05/02/2020 , [{ Project:CRM , Allocation:15 },{ Project:HR , Allocation:15 },{ Project:ECom , Allocation:15 }]  },
        ///            { From:06/02/2020 , To:07/02/2020 , [{ Project:CRM , Allocation:15 },{ Project:ECom , Allocation:10 }]  }
        ///          ]
        /// </summary>
        public IEnumerable<BookingGrouping> Group(IEnumerable<Booking> dates)
        {
            return dates
                .GroupBy(b => b.Date.Date) // Grouping by date so we can group by similar bookings 
                .GroupBy(dg =>
                    CalculateHash(dg.ToList())) // Grouping by similar bookings by using a custom hash function
                .Select(g =>
                {
                    return new BookingGrouping()
                    {
                        From = g.First().Key,
                        To = g.Last().Key,
                        Items = g.First().Select(b => new BookingGroupingItem() // Taking only the first grouping
                            // because similar bookings already grouped
                            // so they consists of the same bookings
                            {
                                Allocation = b.Allocation,
                                Project = b.Project
                            }).ToList()
                    };
                }).ToList();
        }

        /// <summary>
        /// Merges the specified collections so that the n-th element of the second list should appear after the n-th element of the first collection. 
        /// Example : first : 1,3,5 second : 2,4,6 -> result : 1,2,3,4,5,6
        /// </summary>
        public IEnumerable<int> Merge(IEnumerable<int> first, IEnumerable<int> second)
        {
            var firstArray = first as int[] ?? first.ToArray();
            var secondArray = second as int[] ?? second.ToArray();

            if (firstArray.Length != secondArray.Length)
                throw new InvalidOperationException("Collection sizes do not match.");

            var mergedArray = new List<int>();

            for (var i = 0; i < firstArray.Length; i++)
            {
                mergedArray.Add(firstArray[i]);
                mergedArray.Add(secondArray[i]);
            }

            return mergedArray.AsEnumerable();
        }

        #endregion

        #region Utilities

        private static int CalculateHash(List<Booking> items)
        {
            items = items.OrderBy(b => b.Allocation.GetHashCode() + b.Project.GetHashCode()).ToList();
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return items.Aggregate(seed, (current, item) =>
                    (current * modifier) + item.Allocation.GetHashCode() + item.Project.GetHashCode());
            }
        }

        #endregion
    }
}