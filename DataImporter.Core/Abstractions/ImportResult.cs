using System;
using System.Collections.Generic;
using System.Text;

namespace DataImporter.Core.Abstractions
{
    public enum ImportResult
    {
        ImportOK=0,
        DeleteFailed=900,
        ImportFailed=999
    }
}
