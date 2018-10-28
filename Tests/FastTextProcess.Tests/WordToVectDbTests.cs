using FastTextProcess.Context;
using FastTextProcess.Entities;
using System;
using Xunit;
using Xunit.Abstractions;

namespace FastTextProcess.Tests
{
    public class WordToVectDbTests
    {
        ITestOutputHelper _output;
        public WordToVectDbTests(ITestOutputHelper output)
        {
            _output = output; 
        }

        [Fact]
        public void testCreateInsert()
        {
            var dbf = "w2v_test.db";
            WordToVectDb.CreateDB(dbf);
            var w2v = new Word2Vect { Word = "test", Vect = new byte[] { 1, 2, 3 } };
            using (var dbx = new WordToVectDb(dbf))
            {
                dbx.Insert(w2v);
                var id1 = w2v.Id;
                Assert.True(id1 > 0);
                w2v.Word = "Test";
                dbx.Insert(w2v);
                Assert.True(w2v.Id > id1);
            }
            _output.WriteLine("done");
        }
    }
}