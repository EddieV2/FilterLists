﻿using System;
using JetBrains.Annotations;

namespace FilterLists.Services.Seed.Models
{
    [UsedImplicitly]
    public class FilterListSeedDto
    {
        public uint Id { get; set; }
        public string ChatUrl { get; set; }
        public string Description { get; set; }
        public string DescriptionSourceUrl { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public string DonateUrl { get; set; }
        public string EmailAddress { get; set; }
        public string ForumUrl { get; set; }
        public string HomeUrl { get; set; }
        public string IssuesUrl { get; set; }
        public uint? LicenseId { get; set; }
        public string Name { get; set; }
        public string PolicyUrl { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string SubmissionUrl { get; set; }
        public uint? SyntaxId { get; set; }
        public string ViewUrl { get; set; }
        public string ViewUrlMirror1 { get; set; }
        public string ViewUrlMirror2 { get; set; }
    }
}