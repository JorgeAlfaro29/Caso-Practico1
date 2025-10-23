using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PAW3CP1.Models.DTO
{
    public class TaskDTO
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("dueDate")]
        public DateTime? DueDate { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("approved")]
        public bool? Approved { get; set; }

    }
}
