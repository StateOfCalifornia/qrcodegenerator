using Application.Common.Models;
using System.Collections.Generic;

namespace Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IList<TDestination> queryable, int pageNumber, int pageSize, string sortField, string sortOrder)
            => PaginatedList<TDestination>.Create(queryable, pageNumber, pageSize, sortField, sortOrder);
    }
}