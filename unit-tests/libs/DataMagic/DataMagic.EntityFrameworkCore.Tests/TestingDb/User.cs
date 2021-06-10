using System;
using System.ComponentModel.DataAnnotations;
using DataMagic.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    [Keyless]
    public class User
    {
       
        public int Id { get; set; }
        public string Name { get; set; }

        public double Birthday { get; set; }
    }
}