using FastTextProcess.Context;
using FastTextProcess.Entities;
using FastTextProcess.Tests.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace FastTextProcess.Tests
{
    public class DictDbRoutines : TestBase
    {
        public DictDbRoutines(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ProcCreateDbEn()
        {
            var fvec = Resources.DataArcDir + "cc.en.300.vec";
            AssertFileExists(fvec, "FastText file of vectors");

            var dbf = "w2v_en.db";
            AssertFileNotExists(dbf, "word2vect DB");
            FastTextProcessDB.CreateDB(dbf);

            using (var dbx = new FastTextProcessDB(dbf))
            {
                var w2v_tbl = dbx.Dict(DictDbSet.DictDb.Main);
                var trans = dbx.BeginTransaction();
                w2v_tbl.ControlWordsIndex(is_enabled: false);
                using (var sr = new StreamReader(fvec))
                {
                    // header
                    var line = sr.ReadLine();
                    var harr = line.Split(' ');
                    Assert.Equal(2, harr.Length);
                    Log($"'{fvec}': {harr[0]} - samples count, {harr[1]} - sample dim.");
                    // data
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            continue;
                        var w2v = Dict.Create(line);
                        w2v_tbl.Insert(w2v);
                    }
                }
                Log("ControlWordsIndex create...");
                w2v_tbl.ControlWordsIndex(is_enabled: true);
                Log("Done");
                trans.Commit();
            }
        }
    }
}