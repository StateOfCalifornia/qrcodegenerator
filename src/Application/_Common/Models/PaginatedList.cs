using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Models
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items, int count, int currentPage, int pageSize, string sortField, string sortOrder)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
            PageSize = pageSize;
            SortField = sortField;
            SortOrder = sortOrder;
        }

        public List<T> Items { get; }
        public int CurrentPage { get; }
        public int PageSize { get; set; }
        public string SortOrder { get; set; }
        public string SortField { get; set; }


        public int TotalPages { get; }
        public int TotalCount { get; }
        
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public static PaginatedList<T> Create(IList<T> source, int currentPage, int pageSize, string sortField, string sortOrder)
        {
            var count = source.Count;
            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, currentPage, pageSize, sortField, sortOrder);
        }
    }
}