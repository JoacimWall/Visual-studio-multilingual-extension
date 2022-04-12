using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "MultilingualExtension",
    Namespace = "MultilingualExtension.Mac",
    Version = "2.0.0"
)]

[assembly: AddinName("Multilingual Extension")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Synchronization of resx or resw files and auto translation throw Google translate and Microsoft cognitve service")]
[assembly: AddinAuthor("Joacim Wall")]
