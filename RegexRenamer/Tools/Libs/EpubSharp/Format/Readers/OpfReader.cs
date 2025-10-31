using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


namespace EpubSharp.Format.Readers
{
    internal static class OpfReader
    {
        public static OpfDocument Read(XDocument xml)
        {
            if (xml == null) throw new ArgumentNullException(nameof(xml));
            if (xml.Root == null) throw new ArgumentException("XML document has no root element.", nameof(xml));

            XmlNamespaceManager nsT = new XmlNamespaceManager(new NameTable());
            nsT.AddNamespace("opf", EpubSharp.Format.Constants.OpfNamespace.NamespaceName);
            nsT.AddNamespace("dc", EpubSharp.Format.Constants.OpfMetadataNamespace.NamespaceName);
            nsT.AddNamespace("ops", EpubSharp.Format.Constants.OpsNamespace.NamespaceName);

            Func<XElement, OpfMetadataCreator> readCreator = elem => new OpfMetadataCreator
            {
                Role = (string) elem.Attribute(OpfMetadataCreator.Attributes.Role),
                FileAs = (string) elem.Attribute(OpfMetadataCreator.Attributes.FileAs),
                AlternateScript = (string) elem.Attribute(OpfMetadataCreator.Attributes.AlternateScript),
                Text = elem.Value
            };

            var ns = xml.Root.GetDefaultNamespace();

            var epubVersionStr = (string) xml.Root.Attribute(OpfDocument.Attributes.Version);
            var epubVersion = GetAndValidateVersion(epubVersionStr);
            var metadata = xml.Root.Element(ns.GetName("metadata"));
            var guide = xml.Root.Element(ns.GetName("guide"));
            var spine = xml.Root.Element(ns.GetName("spine"));

            var nsDC = metadata.GetNamespaceOfPrefix("dc");

            //var metadata = xml.Root.Element(OpfElements.Metadata);
            //var guide = xml.Root.Element(OpfElements.Guide);
            //var spine = xml.Root.Element(OpfElements.Spine);

            var package = new OpfDocument
            {
                UniqueIdentifier = (string) xml.Root.Attribute(OpfDocument.Attributes.UniqueIdentifier),
                EpubVersion = epubVersion,
                Version = epubVersionStr,
                Metadata = new OpfMetadata
                {
                    Creators = metadata?.Elements(nsDC.GetName("creator")).AsObjectList(readCreator),
                    Contributors = metadata?.Elements(nsDC.GetName("contributor")).AsObjectList(readCreator),
                    Coverages = metadata?.Elements(nsDC.GetName("coverages")).AsStringList(),
                    Dates = metadata?.Elements(nsDC.GetName("date")).AsObjectList(elem => new OpfMetadataDate
                    {
                        Text = elem.Value,
                        Event = (string)elem.Attribute(OpfMetadataDate.Attributes.Event)
                    }),
                    Descriptions = metadata?.Elements(nsDC.GetName("description")).AsStringList(),
                    Formats = metadata?.Elements(nsDC.GetName("format")).AsStringList(),
                    Identifiers = metadata?.Elements(nsDC.GetName("identifier")).AsObjectList(elem => new OpfMetadataIdentifier
                    {
                        Id = (string) elem.Attribute(OpfMetadataIdentifier.Attributes.Id),
                        Scheme = (string) elem.Attribute(OpfMetadataIdentifier.Attributes.Scheme),
                        Text = elem.Value
                    }),
                    Languages = metadata?.Elements(nsDC.GetName("language")).AsStringList(),
                    Metas = metadata?.Elements(ns.GetName("meta")).AsObjectList(elem => new OpfMetadataMeta
                    {
                        Id = (string) elem.Attribute(OpfMetadataMeta.Attributes.Id),
                        Name = (string) elem.Attribute(OpfMetadataMeta.Attributes.Name),
                        Refines = (string) elem.Attribute(OpfMetadataMeta.Attributes.Refines),
                        Scheme = (string) elem.Attribute(OpfMetadataMeta.Attributes.Scheme),
                        Property = (string) elem.Attribute(OpfMetadataMeta.Attributes.Property),
                        Text = epubVersion == EpubVersion.Epub2 ? (string) elem.Attribute(OpfMetadataMeta.Attributes.Content) : elem.Value
                    }),
                    Publishers = metadata?.Elements(nsDC.GetName("publisher")).AsStringList(),
                    Relations = metadata?.Elements(nsDC.GetName("relation")).AsStringList(),
                    Rights = metadata?.Elements(nsDC.GetName("rights")).AsStringList(),
                    Sources = metadata?.Elements(nsDC.GetName("source")).AsStringList(),
                    Subjects = metadata?.Elements(nsDC.GetName("subject")).AsStringList(),
                    Titles = metadata?.Elements(nsDC.GetName("title")).AsStringList(),
                    Types = metadata?.Elements(nsDC.GetName("type")).AsStringList()
                },
                Guide = guide == null ? null : new OpfGuide
                {
                    References = guide.Elements(ns.GetName("reference")).AsObjectList(elem => new OpfGuideReference
                    {
                        Title = (string) elem.Attribute(OpfGuideReference.Attributes.Title),
                        Type = (string) elem.Attribute(OpfGuideReference.Attributes.Type),
                        Href = (string) elem.Attribute(OpfGuideReference.Attributes.Href)
                    })
                },
                Manifest = new OpfManifest
                {
                    Items = xml.Root.Element(ns.GetName("manifest"))?.Elements(ns.GetName("item")).AsObjectList(elem => new OpfManifestItem
                    {
                        Fallback = (string) elem.Attribute(OpfManifestItem.Attributes.Fallback),
                        FallbackStyle = (string) elem.Attribute(OpfManifestItem.Attributes.FallbackStyle),
                        Href = (string) elem.Attribute(OpfManifestItem.Attributes.Href),
                        Id = (string) elem.Attribute(OpfManifestItem.Attributes.Id),
                        MediaType = (string) elem.Attribute(OpfManifestItem.Attributes.MediaType),
                        Properties = ((string) elem.Attribute(OpfManifestItem.Attributes.Properties))?.Split(' ') ?? new string[0],
                        RequiredModules = (string) elem.Attribute(OpfManifestItem.Attributes.RequiredModules),
                        RequiredNamespace = (string) elem.Attribute(OpfManifestItem.Attributes.RequiredNamespace)
                    })
                },
                Spine = new OpfSpine
                {
                    ItemRefs = spine?.Elements(ns.GetName("itemref")).AsObjectList(elem => new OpfSpineItemRef
                    {
                        IdRef = (string) elem.Attribute(OpfSpineItemRef.Attributes.IdRef),
                        Linear = (string) elem.Attribute(OpfSpineItemRef.Attributes.Linear) != "no",
                        Id = (string) elem.Attribute(OpfSpineItemRef.Attributes.Id),
                        Properties = ((string) elem.Attribute(OpfSpineItemRef.Attributes.Properties))?.Split(' ').ToList() ?? new List<string>()
                    }),
                    Toc = spine?.Attribute(OpfSpine.Attributes.Toc)?.Value
                }
            };

            return package;
        }

        private static EpubVersion GetAndValidateVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentNullException(nameof(version));

            if (version == "2.0")
            {
                return EpubVersion.Epub2;
            }
            if (version == "3.0" || version == "3.0.1" || version == "3.1")
            {
                return EpubVersion.Epub3;
            }

            throw new Exception($"Unsupported EPUB version: {version}.");
        }
    }
}
