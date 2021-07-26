using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.ViewModels
{
    public static class TrickViewModel
    {
        public static Expression<Func<Trick, object>> Default =>
            trick => new
            {
                trick.Id,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                Categories = trick.TrickCategories.Select(tc => tc.CategoryId),
                Prerequisites = trick.Prerequisites.Select(pq => pq.PrerequisiteId),
                Progressions = trick.Progressions.Select(pg => pg.ProgressionId)
            };
    }
}