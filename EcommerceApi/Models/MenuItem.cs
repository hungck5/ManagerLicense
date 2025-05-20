using EcommerceApi.Framework.Domain;

namespace EcommerceApi.Models
{
    public class MenuItem : AggregateRoot
    {
        public string Label { get; set; }

        public Guid? SeoMetaId { get; set; }
        public SeoMeta? SeoMeta { get; set; }

        public int? ParentId { get; set; }
        public MenuItem? Parent { get; set; }

        public int Order { get; set; }
    }
}