﻿using System;
using System.Collections.Generic;

namespace DZ5_Savchuk.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string PublisherName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
