using System;
using System.Collections.Generic;

namespace StudentTaskManager.Models 
{
    public class ReportViewModel
    {
       
        public List<string> MonthlyLabels { get; set; } = new List<string>();
        public List<int> MonthlyCompletedCounts { get; set; } = new List<int>();

        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<int> CategoryCounts { get; set; } = new List<int>();
        public List<string> CategoryColors { get; set; } = new List<string>();

       
        public double CompletionRate { get; set; } = 0.0;
        public int OverdueCount { get; set; } = 0;
        public int OnTimeCount { get; set; } = 0;
    }
}