using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Editor2.Models;

namespace Editor2.Tests
{
    public class ElementTest
    {
        public void TestFindElementByGuid()
        {
            // first add an element to a doc
            DocModel doc = new DocModel();
            Element expected = new Element();
            doc.AddElement(expected);

            Guid ExpectedGuid = expected.ElementId;

            Element actual = doc.getElementByGuid(ExpectedGuid);

            Debug.Equals(actual, expected);

        }
    }
}