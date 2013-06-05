using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Editor2.Models;

namespace Editor2.Tests
{
    public class DocModelTest
    {
        public void TestCreateDoc()
        {
            DocModel doc = new DocModel();
            Debug.Assert(doc != null);
        }

    }
}