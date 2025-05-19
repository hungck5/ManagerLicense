namespace EcommerceApi.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public int SeoMetaId { get; set; }
        public SeoMeta SeoMeta { get; set; }

        public int? ParentId { get; set; }
        public MenuItem? Parent { get; set; }

        public int Order { get; set; }
    }
}