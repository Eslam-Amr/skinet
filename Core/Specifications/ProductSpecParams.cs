using System;

namespace Core.Specifications;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int pageSize = 6;
    public int PageSize
    {
        get => this.pageSize;
        set => this.pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private List<String> brands = [];
    public List<String> Brands
    {
        get => this.brands;
        set
        {
            this.brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    private List<String> types = [];
    public List<String> Types
    {
        get => this.types;
        set
        {
            this.types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    public string? Sort { get; set; }
    private string? search;
    public string Search
    {
        get => this.search ?? "";
        set =>
            this.search = value.ToLower();

    }
}
