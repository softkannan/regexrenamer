using EpubSharp;
using EpubSharp.Format.Writers;
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

public static class EpubSharpHelper
{
    private static EpubBook WriteAndRead(string filePath, EpubWriter writer)
    {
        var stream = new MemoryStream();
        writer.Write(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var epub = EpubReader.Read(stream, false);
        return epub;
    }

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
        // SharpCompress open archive file for read write 
        EpubWriter writer = new EpubWriter();
        EpubBook book = WriteAndRead(filePath, new EpubWriter());

        /*
            <meta name="calibre:series" content="Test"/>
            <meta name="calibre:series_index" content = "12.0" />
            <meta property="belongs-to-collection" id="id-2">Test</meta>
            <meta refines="#id-2" property="collection-type">series</meta>
            <meta refines="#id-2" property="group-position">12</meta>
         */

        writer.ClearAuthors();
        writer.RemoveTitle();
        writer.ClearSeriesAndVolume();


        //var calibreNS = "http://calibre-ebook.com/xmp-namespace";
        //var seriesMeta = opf.Metadata.Metas.FirstOrDefault(meta => meta.Name == "calibre:series");
        //if (seriesMeta != null)
        //{
        //    seriesMeta.Text = "";
        //}
        //else
        //{
        //    opf.Metadata.Metas.Add(new EpubSharp.Format.OpfMetadataMeta { Name = "calibre:series", Text = "" });
        //}

        //var volumeMeta = opf.Metadata.Metas.FirstOrDefault(meta => meta.Name == "calibre:series_index");
        //if (volumeMeta != null)
        //{
        //    volumeMeta.Text = "";
        //}
        //else
        //{
        //    opf.Metadata.Metas.Add(new EpubSharp.Format.OpfMetadataMeta { Name = "calibre:series_index", Text = "" });
        //}



        writer.Write(new MemoryStream());

        return true;
    }

    public static bool WriteEpubMetadata(string filePath, ComicInfo metadata)
    {
        EpubBook book = EpubReader.Read(filePath);
        EpubWriter writer = new EpubWriter(book);
        var opf = book.Format.Opf;
        int writerCount = 0;
        if (!string.IsNullOrWhiteSpace(metadata.Title))
        {
            writerCount++;
            opf.Metadata.Titles.Clear();
            opf.Metadata.Titles.Add(metadata.Title);
        }
        if (!string.IsNullOrWhiteSpace(metadata.Writer))
        {
            writerCount++;
            opf.Metadata.Creators.Clear();
            opf.Metadata.Creators.Add(new EpubSharp.Format.OpfMetadataCreator { Text = metadata.Writer });
        }
        if (!string.IsNullOrWhiteSpace(metadata.Series))
        {
            writerCount++;
            var seriesMeta = opf.Metadata.Metas.FirstOrDefault(meta => meta.Name == "calibre:series");
            if (seriesMeta != null)
            {
                seriesMeta.Text = metadata.Series;
            }
            else
            {
                opf.Metadata.Metas.Add(new EpubSharp.Format.OpfMetadataMeta { Name = "calibre:series", Text = metadata.Series });
            }
        }
        if (!string.IsNullOrWhiteSpace(metadata.Volume))
        {
            writerCount++;
            var volumeMeta = opf.Metadata.Metas.FirstOrDefault(meta => meta.Name == "calibre:series_index");
            if (volumeMeta != null)
            {
                volumeMeta.Text = metadata.Volume;
            }
            else
            {
                opf.Metadata.Metas.Add(new EpubSharp.Format.OpfMetadataMeta { Name = "calibre:series_index", Text = metadata.Volume });
            }
        }
        if (!string.IsNullOrWhiteSpace(metadata.LanguageISO))
        {
            writerCount++;
            opf.Metadata.Languages.Clear();
            opf.Metadata.Languages.Add(metadata.LanguageISO);
        }

        if(writerCount == 0)
        {
            // Nothing to write
            return false;
        }
        writer.Write();
        return true;
    }
}
