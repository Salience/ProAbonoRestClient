using System;
using System.Collections.Generic;

namespace ProAbono
{
    public class PaginatedList<T>
    {
        public PaginatedList()
        {
            this.Items = new List<T>();
        }

        public int Page { get; set; }
        public int SizePage { get; set; }
        public int Count { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
        public DateTime DateGenerated { get; set; }
        public List<Link> Links { get; set; }
    }
}