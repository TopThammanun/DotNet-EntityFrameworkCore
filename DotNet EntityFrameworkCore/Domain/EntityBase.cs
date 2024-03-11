using System.Text.Json.Serialization;

namespace DotNet_EntityFrameworkCore.Domain
{
    public interface IModifyTrackingEntity
    {
        string CREATED_BY { get; set; }
        DateTime? CREATED_DATE { get; set; }
        string UPDATED_BY { get; set; }
        DateTime? UPDATED_DATE { get; set; }
    }

    public interface IDeleteFlagEntity
    {
        string DELETED_FLAG { get; set; }
        string DELETED_BY { get; set; }
        DateTime? DELETED_DATE { get; set; }
    }

    public abstract class EntityBase : IModifyTrackingEntity
    {
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }

    public abstract class DeleteFlagEntity : EntityBase, IDeleteFlagEntity
    {
        public string DELETED_FLAG { get; set; }
        public string DELETED_BY { get; set; }
        public DateTime? DELETED_DATE { get; set; }
    }

    public abstract class EntityStringBase
    {

        [JsonPropertyName("createdBy")]
        public string CREATED_BY { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CREATED_DATE { get; set; }// = DateTime.Now;

        [JsonPropertyName("updatedBy")]
        public string UPDATED_BY { get; set; }

        [JsonPropertyName("updatedDate")]
        public DateTime? UPDATED_DATE { get; set; }// = DateTime.Now;
    }
}
