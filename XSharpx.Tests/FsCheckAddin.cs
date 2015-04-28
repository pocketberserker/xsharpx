using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Core.Extensibility;
using FsCheck.NUnit;
using FsCheck.NUnit.Addin;

namespace XSharpx.Tests
{
    [NUnitAddin(Description = "FsCheck addin")]
    public class FsCheckAddin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var tcBuilder = new FsCheckTestCaseBuilder();
            host.GetExtensionPoint("TestCaseBuilders").Install(tcBuilder);
            return true;
        }
    }
}
