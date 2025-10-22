using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using entityframework.entities;
using Microsoft.EntityFrameworkCore;

namespace MyBoardsBenchmark
{
    [MemoryDiagnoser]
    public class TrackingBenchmark
    {
        [Benchmark]
        public int WithTracking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyBoardsDb;Trusted_Connection=True;");
            var _dbContext = new MyBoardsContext(optionsBuilder.Options);

            var comments = _dbContext.Comments.ToList();
            return comments.Count;
        }
        [Benchmark]
        public int WithoutTracking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyBoardsDb;Trusted_Connection=True;");
            var _dbContext = new MyBoardsContext(optionsBuilder.Options);

            var comments = _dbContext.Comments.AsNoTracking().ToList();
            return comments.Count;
        }
    }
}