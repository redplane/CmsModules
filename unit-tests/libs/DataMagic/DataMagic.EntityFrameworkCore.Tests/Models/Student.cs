using System;

namespace DataMagic.EntityFrameworkCore.Tests.Models
{
    public class Student
    {
        #region Properties

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public int Age { get; set; }
        //
        // public int Posts { get; set; }

        #endregion

        #region Constructor

        public Student(Guid id)
        {
            Id = id;
        }

        #endregion
    }
}