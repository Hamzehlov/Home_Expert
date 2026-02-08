using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class ServiceRequestIssue
{
    [Key]
    public int Id { get; set; }

    public int RequestId { get; set; }

    public int IssueTypeId { get; set; }

    [ForeignKey("IssueTypeId")]
    [InverseProperty("ServiceRequestIssues")]
    public virtual Code IssueType { get; set; } = null!;

    [ForeignKey("RequestId")]
    [InverseProperty("ServiceRequestIssues")]
    public virtual ServiceRequest Request { get; set; } = null!;
}
