﻿namespace Music.Models.Viewmodels;

public class PaginationViewModel
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public string Query { get; set; }
}