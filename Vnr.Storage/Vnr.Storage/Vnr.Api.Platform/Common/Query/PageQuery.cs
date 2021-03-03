using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vnr.Api.Platform.Common.Query
{
    public class PageQuery
    {
        [FromQuery]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [FromQuery]
        [Range(0, int.MaxValue)]
        public int PageSize { get; set; } = 20;
    }
}