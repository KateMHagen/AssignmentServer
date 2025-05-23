﻿using Microsoft.EntityFrameworkCore;
namespace AssignmentServer.Dtos
{
    // Implements pagination for the application and send information about numbers of pages/books
    // to take into account to the frontend
    public class ApiResult<T>
    {
        private ApiResult(List<T> data, int count, int pageIndex, int pageSize)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        }
        public static async Task<ApiResult<T>> CreateAsync(
            IQueryable<T> source,
            int pageIndex,
            int pageSize)
        {
            var count = await source.CountAsync();
            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            var data = await source.ToListAsync();
            return new ApiResult<T>(
                data,
                count,
                pageIndex,
                pageSize);
        }

        public List<T> Data { get; private set; }
   
        public int PageIndex { get; private set; }
        
        public int PageSize { get; private set; }
  
        public int TotalCount { get; private set; }
        
        public int TotalPages { get; private set; }
    
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }

    }
}

