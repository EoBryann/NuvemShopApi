using System;
using System.Collections.Generic;

namespace DestakAPI.Models
{
    public class NomeMultilanguage
    {
        public string en { get; set; } = "";
        public string es { get; set; } = "";
        public string pt { get; set; } = "";
    }



    public class AttributeValue
    {
        public string value { get; set; } = "";
    }

    public class ProductAttribute
    {
        public string name { get; set; } = "";
        public List<AttributeValue> values { get; set; } = new();
    }

    public class Brand
    {
        public int id { get; set; }
        public string name { get; set; } = "";
    }

    public class Image
    {
        public int? id { get; set; }
        public string src { get; set; } = "";
        public int? position { get; set; }
        public int? product_id { get; set; }
    }

    public class Variant
    {
        public int? id { get; set; }
        public string price { get; set; } = "";
        public string promotional_price { get; set; } = "";
        public bool stock_management { get; set; } = true;
        public int stock { get; set; }
        public string? sku { get; set; }
        public string weight { get; set; } = "";
        public string? cost { get; set; }
        public string? depth { get; set; }
        public string? height { get; set; }
        public string? width { get; set; }
        public List<string> values { get; set; } = new(); // valores de atributos variant, se houver
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public int? product_id { get; set; }
    }

    public class ProductPostModel
    {
        public List<ProductAttribute> attributes { get; set; } = new();

        public List<int> categories { get; set; } = new();
        public DateTime? created_at { get; set; }
        public NomeMultilanguage description { get; set; } = new();
        public NomeMultilanguage handle { get; set; } = new();
        public int? id { get; set; }
        public List<Image> images { get; set; } = new();
        public NomeMultilanguage name { get; set; } = new();
        public Brand? brand { get; set; }
        public string video_url { get; set; } = "";
        public string seo_title { get; set; } = "";
        public string seo_description { get; set; } = "";
        public bool published { get; set; } = true;
        public bool free_shipping { get; set; } = false;
        public List<Variant> variants { get; set; } = new();
        public string tags { get; set; } = "";
    }
}
