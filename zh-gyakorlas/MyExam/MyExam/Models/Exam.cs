//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyExam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exam
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int T1 { get; set; }
        public int A1 { get; set; }
        public int T2 { get; set; }
        public int A2 { get; set; }
        public int T3 { get; set; }
        public int A3 { get; set; }
        public int T4 { get; set; }
        public int A4 { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Task Task { get; set; }
        public virtual Task Task1 { get; set; }
        public virtual Task Task2 { get; set; }
        public virtual Task Task3 { get; set; }
    }
}
