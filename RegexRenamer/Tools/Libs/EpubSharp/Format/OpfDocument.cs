using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EpubSharp.Format
{
    internal static class OpfElements
    {
        /*
        <?xml version="1.0" encoding="UTF-8"?>
        <package xmlns="http://www.idpf.org/2007/opf" xmlns:opf="http://www.idpf.org/2007/opf" version="3.0" unique-identifier="BookID">
          <metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
           <dc:identifier id="BookID" opf:scheme="isbn">978-3-86680-192-9</dc:identifier>
           <dc:identifier>3b622266-b838-4003-bcb8-b126ee6ae1a2</dc:identifier>
           <dc:title>The title</dc:title>
           <dc:language>fr</dc:language>
           <dc:publisher>The publisher</dc:publisher>
           <dc:creator>The author</dc:creator>
           <dc:contributor>A contributor</dc:contributor>
           <dc:description>A description</dc:description>
           <dc:subject>A subject of the publication</dc:subject>
           <dc:subject>Another subject of the publication</dc:subject>
           <dc:rights>© copyright notice</dc:rights>
           <meta property="dcterms:modified">2020-01-01T01:01:01Z</meta>
           <meta name="calibre:series" content="Test"/>
           <meta name="calibre:series_index" content = "12.0" />
           <meta property="belongs-to-collection" id="id-2">Test</meta>
           <meta property="collection-type">series</meta>
           <meta property="group-position">12</meta>
          </metadata>
          <manifest>
           <item id="coverimage" href="Images/cover.jpg" media-type="image/jpeg" properties="cover-image"/>
           <item id="cover" href="Text/cover.xhtml" media-type="application/xhtml+xml"/>
           <item id="toc" href="toc.html" media-type="application/xhtml+xml" properties="nav"/>
           <item id="chapter-1" href="Text/chapter-1.xhtml" media-type="application/xhtml+xml"/>
           <item id="chapter-2" href="Text/chapter-2.xhtml" media-type="application/xhtml+xml"/>
           <item id="css" href="Styles/publication.css" media-type="text/css"/>
           <item id="font1" href="Fonts/Andada-Italic.otf" media-type="application/vnd.ms-opentype"/>
           <item id="font2" href="Fonts/Andada-Regular.otf" media-type="application/vnd.ms-opentype"/>
           <item id="glyph" href="Images/glyph.png" media-type="image/png"/>
          </manifest>
          <spine>
           <itemref idref="cover"/>
           <itemref idref="toc"/>
           <itemref idref="chapter-1"/>
           <itemref idref="chapter-2"/>
          </spine>
        </package>
        */

        public static readonly XName Package = XNamespace.Xmlns + "package";
        public static readonly XName Metadata = XNamespace.Xmlns + "metadata";
        public static readonly XName Meta = XNamespace.Xmlns + "meta";

        public static readonly XName Contributor = Constants.OpfMetadataNamespace +  "contributor";
        public static readonly XName Creator = Constants.OpfMetadataNamespace + "creator";
        public static readonly XName Description = Constants.OpfMetadataNamespace + "description";
        public static readonly XName Identifier = Constants.OpfMetadataNamespace + "identifier";
        public static readonly XName Language = Constants.OpfMetadataNamespace + "language";
        public static readonly XName Publisher = Constants.OpfMetadataNamespace + "publisher";
        public static readonly XName Rights = Constants.OpfMetadataNamespace + "rights";
        public static readonly XName Subject = Constants.OpfMetadataNamespace + "subject";
        public static readonly XName Title = Constants.OpfMetadataNamespace + "title";
        
        public static readonly XName Coverages = XNamespace.Xmlns + "coverages";
        public static readonly XName Date = XNamespace.Xmlns + "date";
        public static readonly XName Format = XNamespace.Xmlns + "format";
        public static readonly XName Relation = XNamespace.Xmlns + "relation";
        public static readonly XName Source = XNamespace.Xmlns + "source";
        public static readonly XName Type = XNamespace.Xmlns + "type";
              
        public static readonly XName Guide =  XNamespace.Xmlns + "guide";
        public static readonly XName Reference = XNamespace.Xmlns + "reference";
                      
        public static readonly XName Manifest = XNamespace.Xmlns + "manifest";
        public static readonly XName Item = XNamespace.Xmlns + "item";
                      
        public static readonly XName Spine = XNamespace.Xmlns + "spine";
        public static readonly XName ItemRef = XNamespace.Xmlns + "itemref";
    }

    public enum EpubVersion
    {
        Epub2 = 2,
        Epub3 = 3
    }
    
    public class OpfDocument
    {
        internal static class Attributes
        {
            public static readonly XName UniqueIdentifier = "unique-identifier";
            public static readonly XName Version = "version";
        }

        public string UniqueIdentifier { get; internal set; }
        public EpubVersion EpubVersion { get; internal set; }
        public string Version { get; internal set; }

        public OpfMetadata Metadata { get; internal set; } = new OpfMetadata();
        public OpfManifest Manifest { get; internal set; } = new OpfManifest();
        public OpfSpine Spine { get; internal set; } = new OpfSpine();
        public OpfGuide Guide { get; internal set; } = new OpfGuide();

        internal string FindCoverPath()
        {
            var coverMetaItem = Metadata.FindCoverMeta();
            if (coverMetaItem != null)
            {
                var item = Manifest.Items.FirstOrDefault(e => e.Id == coverMetaItem.Text);
                if (item != null)
                {
                    return item.Href;
                }
            }

            var coverItem = Manifest.FindCoverItem();
            return coverItem?.Href;
        }

        internal string FindAndRemoveCover()
        {
            var path = FindCoverPath();
            var meta = Metadata.FindAndDeleteCoverMeta();
            Manifest.DeleteCoverItem(meta?.Text);
            return path;
        }
        
        internal string FindNcxPath()
        {
            string path = null;

            var ncxItem = Manifest.Items.FirstOrDefault(e => e.MediaType == "application/x-dtbncx+xml");
            if (ncxItem != null)
            {
                path = ncxItem.Href;
            }
            else
            {
                // If we can't find the toc by media-type then try to look for id of the item in the spine attributes as
                // according to http://www.idpf.org/epub/20/spec/OPF_2.0.1_draft.htm#Section2.4.1.2,
                // "The item that describes the NCX must be referenced by the spine toc attribute."

                if (!string.IsNullOrWhiteSpace(Spine.Toc))
                {
                    var tocItem = Manifest.Items.FirstOrDefault(e => e.Id == Spine.Toc);
                    if (tocItem != null)
                    {
                        path = tocItem.Href;
                    }
                }
            }

            return path;
        }

        internal string FindNavPath()
        {
            var navItem = Manifest.Items.FirstOrDefault(e => e.Properties.Contains("nav"));
            return navItem?.Href;
        }
    }

    public class OpfMetadata
    {
        public IList<string> Titles { get; internal set; } = new List<string>();
        public IList<string> Series { get; internal set; } = new List<string>();
        public IList<string> Volumes { get; internal set; } = new List<string>();

        public IList<string> Subjects { get; internal set; } = new List<string>();
        public IList<string> Descriptions { get; internal set; } = new List<string>();
        public IList<string> Publishers { get; internal set; } = new List<string>();
        public IList<OpfMetadataCreator> Creators { get; internal set; } = new List<OpfMetadataCreator>();
        public IList<OpfMetadataCreator> Contributors { get; internal set; } = new List<OpfMetadataCreator>();
        public IList<OpfMetadataDate> Dates { get; internal set; } = new List<OpfMetadataDate>();
        public IList<string> Types { get; internal set; } = new List<string>();
        public IList<string> Formats { get; internal set; } = new List<string>();
        public IList<OpfMetadataIdentifier> Identifiers { get; internal set; } = new List<OpfMetadataIdentifier>();
        public IList<string> Sources { get; internal set; } = new List<string>();
        public IList<string> Languages { get; internal set; } = new List<string>();
        public IList<string> Relations { get; internal set; } = new List<string>();
        public IList<string> Coverages { get; internal set; } = new List<string>();
        public IList<string> Rights { get; internal set; } = new List<string>();
        public IList<OpfMetadataMeta> Metas { get; internal set; } = new List<OpfMetadataMeta>();

        internal OpfMetadataMeta FindCoverMeta()
        {
            return Metas.FirstOrDefault(metaItem => metaItem.Name == "cover");
        }

        internal OpfMetadataMeta FindAndDeleteCoverMeta()
        {
            var meta = FindCoverMeta();
            if (meta == null) return null;
            Metas.Remove(meta);
            return meta;
        }
    }

    public class OpfMetadataDate
    {
        internal static class Attributes
        {
            public static readonly XName Event = Constants.OpfNamespace + "event";
        }

        public string Text { get; internal set; }

        /// <summary>
        /// i.e. "modification"
        /// </summary>
        public string Event { get; internal set; }
    }

    public class OpfMetadataCreator
    {
        internal static class Attributes
        {
            public static readonly XName Role = Constants.OpfNamespace + "role";
            public static readonly XName FileAs = Constants.OpfNamespace + "file-as";
            public static readonly XName AlternateScript = Constants.OpfNamespace + "alternate-script";
        }

        public string Text { get; internal set; }
        public string Role { get; internal set; }
        public string FileAs { get; internal set; }
        public string AlternateScript { get; internal set; }
    }

    public class OpfMetadataIdentifier
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Scheme = "scheme";
        }

        public string Id { get; internal set; }
        public string Scheme { get; internal set; }
        public string Text { get; internal set; }
    }

    public class OpfMetadataMeta
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Name = "name";
            public static readonly XName Refines = "refines";
            public static readonly XName Scheme = "scheme";
            public static readonly XName Property = "property";
            public static readonly XName Content = "content";

            public static readonly XName Series = "calibre:series";
            public static readonly XName Volume = "calibre:series_index";
        }

        public string Name { get; internal set; }
        public string Id { get; internal set; }
        public string Refines { get; internal set; }
        public string Property { get; internal set; }
        public string Scheme { get; internal set; }
        public string Text { get; internal set; }
        public string Content { get; internal set; }

        public string Series { get; internal set; }
        public string Volume { get; internal set; }
    }

    public class OpfManifest
    {
        internal const string ManifestItemCoverImageProperty = "cover-image";

        public IList<OpfManifestItem> Items { get; internal set; } = new List<OpfManifestItem>();

        internal OpfManifestItem FindCoverItem()
        {
            return Items.FirstOrDefault(e => e.Properties.Contains(ManifestItemCoverImageProperty));
        }

        internal void DeleteCoverItem(string id = null)
        {
            var item = id != null ? Items.FirstOrDefault(e => e.Id == id) : FindCoverItem();
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }

    public class OpfManifestItem
    {
        internal static class Attributes
        {
            public static readonly XName Fallback = "fallback";
            public static readonly XName FallbackStyle = "fallback-style";
            public static readonly XName Href = "href";
            public static readonly XName Id = "id";
            public static readonly XName MediaType = "media-type";
            public static readonly XName Properties = "properties";
            public static readonly XName RequiredModules = "required-modules";
            public static readonly XName RequiredNamespace = "required-namespace";
        }

        public string Id { get; internal set; }
        public string Href { get; internal set; }
        public IList<string> Properties { get; internal set; } = new List<string>();
        public string MediaType { get; internal set; }
        public string RequiredNamespace { get; internal set; }
        public string RequiredModules { get; internal set; }
        public string Fallback { get; internal set; }
        public string FallbackStyle { get; internal set; }

        public override string ToString()
        {
            return $"Id: {Id}, Href = {Href}, MediaType = {MediaType}";
        }
    }

    public class OpfSpine
    {
        internal static class Attributes
        {
            public static readonly XName Toc = "toc";
        }

        public string Toc { get; internal set; }
        public IList<OpfSpineItemRef> ItemRefs { get; internal set; } = new List<OpfSpineItemRef>();
    }

    public class OpfSpineItemRef
    {
        internal static class Attributes
        {
            public static readonly XName IdRef = "idref";
            public static readonly XName Linear = "linear";
            public static readonly XName Id = "id";
            public static readonly XName Properties = "properties";
        }

        public string IdRef { get; internal set; }
        public bool Linear { get; internal set; }
        public string Id { get; internal set; }
        public IList<string> Properties { get; internal set; } = new List<string>();

        public override string ToString()
        {
            return "IdRef: " + IdRef;
        }
    }

    public class OpfGuide
    {
        public IList<OpfGuideReference> References { get; internal set; } = new List<OpfGuideReference>();
    }

    public class OpfGuideReference
    {
        internal static class Attributes
        {
            public static readonly XName Title = "title";
            public static readonly XName Type = "type";
            public static readonly XName Href = "href";
        }

        public string Type { get; internal set; }
        public string Title { get; internal set; }
        public string Href { get; internal set; }

        public override string ToString()
        {
            return $"Type: {Type}, Href: {Href}";
        }
    }
}
