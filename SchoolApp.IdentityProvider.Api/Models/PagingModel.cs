using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.IdentityProvider.Api.Models;

public class PagingModel
{
    [DefaultValue(30)]
    [Range(1, 60)]
    public int Top { get; set; }

    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }
}
