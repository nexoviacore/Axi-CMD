using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Collections;

/// <summary>
/// Reset bundle files order as specified : BundleSorter
/// </summary>
/// <Author>  Prashik  </Author>

public class BundleSorter : IBundleOrderer
{
    public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
    {
        return files;
    }
}