using EpubSharp;
using EpubSharp.Format.Writers;
using iText.Pdfua.Checkers.Utils;
using Org.BouncyCastle.Asn1.Cms;
using Kavita;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RegexRenamer.Tools.EBookPDFTools;

public static class EpubMetaHelper
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
    public static bool ClearEpubMetadata(string filePath)
    {
        string tempFilePath = filePath.GetInFolderTempFilePath();
        // EPUB files are ZIP archives. Use SharpCompress to open and modify.
        using (var archive = ZipArchive.Open(filePath))
        {
            // Find the OPF file (package.opf) and clear metadata
            var opfEntry = archive.Entries.FirstOrDefault(e => e.Key.EndsWith(".opf", StringComparison.OrdinalIgnoreCase));
            if (opfEntry != null && !opfEntry.IsDirectory)
            {
                using (var ms = new MemoryStream())
                {
                    opfEntry.WriteTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    // Load OPF as XmlDocument
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(ms);

                    // Find the <metadata> node
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("opf", "http://www.idpf.org/2007/opf");
                    nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

                    // Get metadata node
                    var metadataNode = xmlDoc.SelectSingleNode("//*[local-name()='metadata']", nsmgr);
                    if (metadataNode == null)
                        return false;

                    //string nodesToRemoveNames = "dc:title,dc:language,dc:publisher,dc:creator,dc:contributor,dc:description,dc:subject,dc:rights";
                    Dictionary<string, string> dcElemSets = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "dc:title", string.Empty },
                        { "dc:language", string.Empty },
                        { "dc:publisher", string.Empty },
                        { "dc:creator", string.Empty },
                        { "dc:contributor", string.Empty }
                    };

                    foreach (XmlNode childNode in metadataNode.ChildNodes)
                    {
                        if (dcElemSets.ContainsKey(childNode.Name))
                        {
                            XmlElement nodeElem = (XmlElement)childNode;
                            nodeElem.InnerText = "";
                        }
                    }

                    Dictionary<string, string> metaPropertySets = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "belongs-to-collection", string.Empty },
                        { "collection-type", "series" },
                        { "group-position", string.Empty }
                    };

                    Dictionary<string, string> metaNameSets = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "calibre:series", string.Empty },
                        { "calibre:title_sort", string.Empty },
                        { "calibre:series_index", string.Empty },
                    };

                    foreach (XmlNode childNode in metadataNode.ChildNodes)
                    {
                        if (childNode.Name == "meta")
                        {
                            var propAttr = childNode.Attributes["property"];
                            var nameAttr = childNode.Attributes["name"];
                            if ((propAttr != null && metaPropertySets.ContainsKey(propAttr.Value)))
                            {
                                XmlElement nodeElem = (XmlElement)childNode;
                                // Set content attribute to empty
                                nodeElem.InnerText = "";
                            }

                            if ((nameAttr != null && metaNameSets.ContainsKey(nameAttr.Value)))
                            {
                                XmlElement nodeElem = (XmlElement)childNode;
                                // Set content attribute to empty
                                nodeElem.SetAttribute("content","");
                            }
                        }
                    }

                    // Save changes to a new memory stream
                    ms.SetLength(0);
                    using (var writer = XmlWriter.Create(ms, new XmlWriterSettings { 
                        CloseOutput = false,
                        Indent = true,
                        IndentChars = "  ",
                        NewLineOnAttributes = false,
                        Encoding = Encoding.UTF8 
                    }))
                    {
                        xmlDoc.Save(writer);
                        writer.Flush();
                    }
                    ms.Seek(0, SeekOrigin.Begin);
                    // Replace the entry in the archive
                    archive.RemoveEntry(opfEntry);
                    archive.AddEntry(opfEntry.Key, ms, true);
                    archive.SaveTo(tempFilePath, CompressionType.Deflate);
                }
            }
        }

        //After success overwrite original file
        File.Move(tempFilePath, filePath, true);
        return true;
    }

    class MetadataFiledInfo
    {
        public string Value { get; set; }
        public bool IsWritten { get; set; }

        public bool ForceWrite { get;private set; }

        public MetadataFiledInfo(string value)
        {
            Value = value;
            IsWritten = false;
            ForceWrite = false;
        }

        public MetadataFiledInfo(string value, bool forceWrite)
        {
            Value = value;
            IsWritten = false;
            ForceWrite = forceWrite;
        }
    }

    public static bool WriteEpubMetadata(string filePath, ComicInfo metadata)
    {
        string tempFilePath = filePath.GetInFolderTempFilePath();
        // EPUB files are ZIP archives. Use SharpCompress to open and modify.
        using (var archive = ZipArchive.Open(filePath))
        {
            // Find the OPF file (package.opf) and clear metadata
            var opfEntry = archive.Entries.FirstOrDefault(e => e.Key.EndsWith(".opf", StringComparison.OrdinalIgnoreCase));
            if (opfEntry != null && !opfEntry.IsDirectory)
            {
                using (var ms = new MemoryStream())
                {
                    opfEntry.WriteTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    // Load OPF as XmlDocument
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(ms);

                    // Find the <metadata> node
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("opf", "http://www.idpf.org/2007/opf");
                    nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

                    var metadataNode = xmlDoc.SelectSingleNode("//*[local-name()='metadata']", nsmgr);
                    if (metadataNode == null)
                    {
                        metadataNode = xmlDoc.CreateElement("metadata", "http://www.idpf.org/2007/opf");
                    }

                    Dictionary<string, MetadataFiledInfo> dcElemSets = new Dictionary<string, MetadataFiledInfo>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "dc:title", new MetadataFiledInfo(metadata.Title) },
                        { "dc:language",new MetadataFiledInfo(metadata.LanguageISO) },
                        { "dc:publisher", new MetadataFiledInfo(metadata.Writer) },
                        { "dc:creator",new MetadataFiledInfo( metadata.Writer) },
                        { "dc:contributor", new MetadataFiledInfo(metadata.Writer) }
                    };

                    foreach (XmlNode childNode in metadataNode.ChildNodes)
                    {
                        if (dcElemSets.ContainsKey(childNode.Name))
                        {
                            var mataValue = dcElemSets[childNode.Name];
                            if(!string.IsNullOrWhiteSpace(mataValue.Value))
                            {
                                XmlElement nodeElem = (XmlElement)childNode;
                                nodeElem.InnerText = mataValue.Value;
                                mataValue.IsWritten = true;
                            }
                        }
                    }

                    foreach (var dcSet in dcElemSets)
                    {
                        if (!dcSet.Value.IsWritten && !string.IsNullOrWhiteSpace(dcSet.Value.Value))
                        {
                            XmlElement newElem = xmlDoc.CreateElement(dcSet.Key, "http://purl.org/dc/elements/1.1/");
                            newElem.InnerText = dcSet.Value.Value;
                            metadataNode.AppendChild(newElem);
                        }
                    }

                    Dictionary<string, MetadataFiledInfo> metaPropertySets = new Dictionary<string, MetadataFiledInfo>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "belongs-to-collection", new MetadataFiledInfo(metadata.Series) },
                        { "collection-type", new MetadataFiledInfo("series") },
                        { "group-position", new MetadataFiledInfo(metadata.Volume) },
                        { "title-type", new MetadataFiledInfo("", true) }
                    };

                    Dictionary<string, MetadataFiledInfo> metaNameSets = new Dictionary<string, MetadataFiledInfo>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "calibre:series", new MetadataFiledInfo(metadata.Series) },
                        { "calibre:series_index", new MetadataFiledInfo(metadata.Volume) },
                        { "calibre:title_sort", new MetadataFiledInfo(metadata.IsSpecial ? metadata.Title : "", true) },
                    }
                ;

                    foreach (XmlNode childNode in metadataNode.ChildNodes)
                    {
                        if (childNode.Name == "meta")
                        {
                            var propertyAttr = childNode.Attributes["property"];
                            var nameAttr = childNode.Attributes["name"];
                            if (propertyAttr != null && metaPropertySets.ContainsKey(propertyAttr.Value))
                            {
                                var mataValue = metaPropertySets[propertyAttr.Value];
                                if (!string.IsNullOrWhiteSpace(mataValue.Value) || mataValue.ForceWrite)
                                {
                                    XmlElement nodeElem = (XmlElement)childNode;
                                    nodeElem.InnerText = mataValue.Value;
                                    mataValue.IsWritten = true;
                                }
                            }
                            if (nameAttr != null && metaNameSets.ContainsKey(nameAttr.Value))
                            {
                                var mataValue = metaNameSets[nameAttr.Value];
                                if (!string.IsNullOrWhiteSpace(mataValue.Value) || mataValue.ForceWrite)
                                {
                                    XmlElement nodeElem = (XmlElement)childNode;
                                    nodeElem.SetAttribute("content", mataValue.Value);
                                    mataValue.IsWritten = true;
                                }
                            }
                        }
                    }

                    foreach (var metaSet in metaPropertySets)
                    {
                        if (!metaSet.Value.IsWritten && !string.IsNullOrWhiteSpace(metaSet.Value.Value))
                        {
                            XmlElement newElem = xmlDoc.CreateElement("meta", "http://www.idpf.org/2007/opf");
                            XmlAttribute propertyAttr = xmlDoc.CreateAttribute("property");
                            propertyAttr.Value = metaSet.Key;
                            newElem.Attributes.Append(propertyAttr);
                            newElem.InnerText = metaSet.Value.Value;
                            metadataNode.AppendChild(newElem);
                        }
                    }

                    foreach(var metaSet in metaNameSets)
                    {
                        if (!metaSet.Value.IsWritten && !string.IsNullOrWhiteSpace(metaSet.Value.Value))
                        {
                            XmlElement newElem = xmlDoc.CreateElement("meta", "http://www.idpf.org/2007/opf");
                            XmlAttribute nameAttr = xmlDoc.CreateAttribute("name");
                            nameAttr.Value = metaSet.Key;
                            newElem.Attributes.Append(nameAttr);
                            XmlAttribute contentAttr = xmlDoc.CreateAttribute("content");
                            contentAttr.Value = metaSet.Value.Value;
                            newElem.Attributes.Append(contentAttr);
                            metadataNode.AppendChild(newElem);
                        }
                    }

                    // Save changes to a new memory stream
                    ms.SetLength(0);
                    using (var writer = XmlWriter.Create(ms, new XmlWriterSettings
                    {
                        CloseOutput = false,
                        Indent = true,
                        IndentChars = "  ",
                        NewLineOnAttributes = false,
                        Encoding = Encoding.UTF8
                    }))
                    {
                        xmlDoc.Save(writer);
                        writer.Flush();
                    }
                    ms.Seek(0, SeekOrigin.Begin);
                    // Replace the entry in the archive
                    archive.RemoveEntry(opfEntry);
                    archive.AddEntry(opfEntry.Key, ms, true);
                    archive.SaveTo(tempFilePath, CompressionType.Deflate);
                }
            }
        }

        //After success overwrite original file
        File.Move(tempFilePath, filePath, true);
        return true;
    }
}
