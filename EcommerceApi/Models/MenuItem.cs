using EcommerceApi.Framework.Domain;

namespace EcommerceApi.Models;

public class MenuItem : AggregateRoot
{
    public MenuItem( Guid id, string label, SeoMeta? seoMeta, int order, int? parentId = null )
    {
        Id = id;
        Label = label;
        SeoMeta = seoMeta ?? new SeoMeta();
        Order = order;
        ParentId = parentId;
    }

    public string Label { get; set; }

    public Guid? SeoMetaId { get; set; }
    public SeoMeta? SeoMeta { get; set; }

    public int? ParentId { get; set; }
    public MenuItem? Parent { get; set; }

    public int Order { get; set; }

    public void Update( MenuItem menuItem )
    {

        Label = menuItem.Label;
        SeoMeta = menuItem.SeoMeta;
        Order = menuItem.Order;
        ParentId = menuItem.ParentId;
    }
}
