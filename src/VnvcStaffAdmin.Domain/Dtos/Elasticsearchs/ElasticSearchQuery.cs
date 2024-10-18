using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace VnvcStaffAdmin.Domain.Dtos.Elasticsearchs
{
    public class ElasticSearchQuery
    {
        /// <summary>
        /// The starting index of the search results (for pagination).
        /// </summary>
        public int From { get; set; } = 0;

        /// <summary>
        /// The maximum number of search results to return.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "Size must be between 1 and 1000.")]
        public int Size { get; set; } = 10;

        /// <summary>
        /// The query object, which can be a complex query structure.
        /// </summary>
        public object? Query { get; set; }

        /// <summary>
        /// Single sort option to sort the search results.
        /// </summary>
        public ElasticSearchSort? Sort { get; set; }

        /// <summary>
        /// Allows to specify which fields to include or exclude from the search results.
        /// </summary>
        public ElasticSearchSource Source { get; set; } = new ElasticSearchSource();

        /// <summary>
        /// Specify index aliases to search across multiple indices if needed.
        /// </summary>
        public List<string>? Indices { get; set; } = [];

        /// <summary>
        /// Aggregations to be used in the search query (optional).
        /// </summary>
        public Dictionary<string, object>? Aggregations { get; set; } = [];

        /// <summary>
        /// Indicates whether to fetch total hits.
        /// </summary>
        public bool? TrackTotalHits { get; set; } = true;
    }
}