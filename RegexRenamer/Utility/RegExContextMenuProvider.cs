using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegexRenamer.Utility;

public class RegExContextMenuProvider
{
    private ContextMenuStrip cmRegexMatch;
    private ContextMenuStrip cmRegexReplace;
    private ToolStripMenuItem miRegexReplaceOrigAll;
    private ContextMenuStrip cmGlobMatch;
    private ContextMenuStrip cmsBlank;
    
    public RegExContextMenuProvider(IContainer components)
    {
        cmsBlank = new System.Windows.Forms.ContextMenuStrip(components);
        // 
        // cmsBlank
        // 
        cmsBlank.ImageScalingSize = new System.Drawing.Size(20, 20);
        cmsBlank.Name = "cmsBlank";
        cmsBlank.Size = new System.Drawing.Size(61, 4);

        CreateGlobContextMenu(components);
        CreateRegexMatchContextMenu(components);
        CreateRegexReplaceContextMenu(components);
    }

    public void ShowGlobMenu(TextBox txtCtrl, bool showGlob, Point loc)
    {
        lastControlRightClicked = txtCtrl;
        txtCtrl.ContextMenuStrip = cmsBlank;  // prevent default cms from being displayed
        txtCtrl.Focus();
        if (showGlob)
            cmGlobMatch.Show(txtCtrl, loc);
        else
            cmRegexMatch.Show(txtCtrl, loc);
    }

    public void ShowMatchMenu(ComboBox cmbCtrl, Point loc)
    {
        lastControlRightClicked = cmbCtrl;
        cmbCtrl.ContextMenuStrip = cmsBlank;  // prevent default cms from being displayed
        if (!cmbCtrl.Focused)  // prevent combobox from selecting all if already focused
            cmbCtrl.Focus();
        cmRegexMatch.Show(cmbCtrl, loc);
    }

    public void ShowReplaceMenu(ComboBox cmbCtrl, Point loc)
    {
        lastControlRightClicked = cmbCtrl;
        cmbCtrl.ContextMenuStrip = cmsBlank;  // prevent default cms from being displayed
        cmbCtrl.Focus();
        cmRegexReplace.Show(cmbCtrl, loc);
    }

    public void UpdateOrigFilename(string oldFilename,string strFilename)
    {
        miRegexReplaceOrigAll.Text = miRegexReplaceOrigAll.Text.Replace(oldFilename, strFilename);

    }

    // context menus
    private Control lastControlRightClicked;
    private void InsertRegexFragment(object sender, EventArgs e)
    {
        InsertArgs ia = (InsertArgs)((ToolStripMenuItem)sender).Tag;
        TextBox textBox = null;
        ComboBox comboBox = null;

        int selectionStart, selectionLength;
        string text;

        if (lastControlRightClicked.GetType().Name == "TextBox")
        {
            textBox = (TextBox)lastControlRightClicked;
            selectionStart = textBox.SelectionStart;
            selectionLength = textBox.SelectionLength;
            text = textBox.Text;
        }
        else
        {
            comboBox = (ComboBox)lastControlRightClicked;
            selectionStart = comboBox.SelectionStart;
            selectionLength = comboBox.SelectionLength;
            text = comboBox.Text;
        }

        if (ia.InsertBefore == "" && selectionLength == 0)
        {
            ia.InsertBefore = ia.InsertAfter;
            ia.InsertAfter = "";
        }

        if (ia.WrapIfSelection && selectionLength > 0)
            if (ia.InsertAfter == "")
                ia.InsertAfter = ia.InsertBefore;
            else
                ia.InsertBefore = ia.InsertAfter;

        int group = 0;
        if (ia.GroupSelection && selectionLength > 0)
        {
            text = text.Insert(selectionStart, "(");
            selectionStart += 1;
            text = text.Insert(selectionStart + selectionLength, ")");
            group = 1;
        }

        if (selectionLength > 0 && (ia.InsertBefore == "" || ia.InsertAfter == "") && !ia.GroupSelection)
        {
            text = text.Remove(selectionStart, selectionLength);
            selectionLength = 0;
        }

        if (ia.InsertBefore != "")
        {
            text = text.Insert(selectionStart - group, ia.InsertBefore);
            selectionStart += ia.InsertBefore.Length;
        }
        if (ia.InsertAfter != "")
        {
            text = text.Insert(selectionStart + selectionLength + group, ia.InsertAfter);
        }
        if (ia.SelectionStartOffset > 0)
        {
            selectionStart = selectionStart - group - ia.InsertBefore.Length + ia.SelectionStartOffset;
        }
        if (ia.SelectionStartOffset < 0)
        {
            selectionStart = selectionStart + selectionLength + group + ia.InsertAfter.Length + ia.SelectionStartOffset;
        }
        if (ia.SelectionLength != -1)
            selectionLength = ia.SelectionLength;

        if (textBox != null)
        {
            textBox.SelectAll(); textBox.Paste(text);  // allow undo
            textBox.SelectionStart = selectionStart;
            textBox.SelectionLength = selectionLength;
        }
        else
        {
            comboBox.SelectAll(); comboBox.SelectedText = text;  // allow undo
            comboBox.SelectionStart = selectionStart;
            comboBox.SelectionLength = selectionLength;
        }
    }

    private void CreateRegexMatchContextMenu(IContainer components)
    {
        // add insert args to regex context menu items
        cmRegexMatch = new System.Windows.Forms.ContextMenuStrip(components);

        var miRegexMatchMatch = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchSingleChar = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchDigit = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchAlpha = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchSpace = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchMultiChar = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchNonDigit = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchNonAlpha = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchMatchNonSpace = new System.Windows.Forms.ToolStripMenuItem();
        // 
        // miRegexMatchMatch
        // 
        miRegexMatchMatch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchMatchSingleChar, miRegexMatchMatchDigit, miRegexMatchMatchAlpha, miRegexMatchMatchSpace, miRegexMatchMatchMultiChar, miRegexMatchMatchNonDigit, miRegexMatchMatchNonAlpha, miRegexMatchMatchNonSpace });
        miRegexMatchMatch.Name = "miRegexMatchMatch";
        miRegexMatchMatch.Size = new System.Drawing.Size(153, 22);
        miRegexMatchMatch.Text = "Match";
        // 
        // miRegexMatchMatchSingleChar
        // 
        miRegexMatchMatchSingleChar.Name = "miRegexMatchMatchSingleChar";
        miRegexMatchMatchSingleChar.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchSingleChar.Text = "Single character\t.";
        miRegexMatchMatchSingleChar.Tag = new InsertArgs(".");
        miRegexMatchMatchSingleChar.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchDigit
        // 
        miRegexMatchMatchDigit.Name = "miRegexMatchMatchDigit";
        miRegexMatchMatchDigit.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchDigit.Text = "Digit\t\\d";
        miRegexMatchMatchDigit.Tag = new InsertArgs("\\d");
        miRegexMatchMatchDigit.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchAlpha
        // 
        miRegexMatchMatchAlpha.Name = "miRegexMatchMatchAlpha";
        miRegexMatchMatchAlpha.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchAlpha.Text = "Alphanumeric\t\\w";
        miRegexMatchMatchAlpha.Tag = new InsertArgs("\\w");
        miRegexMatchMatchAlpha.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchSpace
        // 
        miRegexMatchMatchSpace.Name = "miRegexMatchMatchSpace";
        miRegexMatchMatchSpace.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchSpace.Text = "Space\t\\s";
        miRegexMatchMatchSpace.Tag = new InsertArgs("\\s");
        miRegexMatchMatchSpace.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchMultiChar
        // 
        miRegexMatchMatchMultiChar.Name = "miRegexMatchMatchMultiChar";
        miRegexMatchMatchMultiChar.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchMultiChar.Text = "Multiple characters\t.*";
        miRegexMatchMatchMultiChar.Tag = new InsertArgs(".*");
        miRegexMatchMatchMultiChar.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchNonDigit
        // 
        miRegexMatchMatchNonDigit.Name = "miRegexMatchMatchNonDigit";
        miRegexMatchMatchNonDigit.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchNonDigit.Text = "Non-digit\t\\D";
        miRegexMatchMatchNonDigit.Tag = new InsertArgs("\\D");
        miRegexMatchMatchNonDigit.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchNonAlpha
        // 
        miRegexMatchMatchNonAlpha.Name = "miRegexMatchMatchNonAlpha";
        miRegexMatchMatchNonAlpha.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchNonAlpha.Text = "Non-alphanumeric\t\\W";
        miRegexMatchMatchNonAlpha.Tag = new InsertArgs("\\W");
        miRegexMatchMatchNonAlpha.Click += InsertRegexFragment;
        // 
        // miRegexMatchMatchNonSpace
        // 
        miRegexMatchMatchNonSpace.Name = "miRegexMatchMatchNonSpace";
        miRegexMatchMatchNonSpace.Size = new System.Drawing.Size(191, 22);
        miRegexMatchMatchNonSpace.Text = "Non-space\t\\S";
        miRegexMatchMatchNonSpace.Tag = new InsertArgs("\\S");
        miRegexMatchMatchNonSpace.Click += InsertRegexFragment;



        var miRegexMatchAnchor = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchAnchorStart = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchAnchorEnd = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchAnchorStartEnd = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchAnchorBound = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchAnchorNonBound = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexMatchAnchor
        // 
        miRegexMatchAnchor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchAnchorStart, miRegexMatchAnchorEnd, miRegexMatchAnchorStartEnd, miRegexMatchAnchorBound, miRegexMatchAnchorNonBound });
        miRegexMatchAnchor.Name = "miRegexMatchAnchor";
        miRegexMatchAnchor.Size = new System.Drawing.Size(153, 22);
        miRegexMatchAnchor.Text = "Anchor";
        // 
        // miRegexMatchAnchorStart
        // 
        miRegexMatchAnchorStart.Name = "miRegexMatchAnchorStart";
        miRegexMatchAnchorStart.Size = new System.Drawing.Size(195, 22);
        miRegexMatchAnchorStart.Text = "Start\t^";
        miRegexMatchAnchorStart.Tag = new InsertArgs("^", "", "group");
        miRegexMatchAnchorStart.Click += InsertRegexFragment;
        // 
        // miRegexMatchAnchorEnd
        // 
        miRegexMatchAnchorEnd.Name = "miRegexMatchAnchorEnd";
        miRegexMatchAnchorEnd.Size = new System.Drawing.Size(195, 22);
        miRegexMatchAnchorEnd.Text = "End\t$";
        miRegexMatchAnchorEnd.Tag = new InsertArgs("", "$", "group");
        miRegexMatchAnchorEnd.Click += InsertRegexFragment;
        // 
        // miRegexMatchAnchorStartEnd
        // 
        miRegexMatchAnchorStartEnd.Name = "miRegexMatchAnchorStartEnd";
        miRegexMatchAnchorStartEnd.Size = new System.Drawing.Size(195, 22);
        miRegexMatchAnchorStartEnd.Text = "Start and End\t^(...)$";
        miRegexMatchAnchorStartEnd.Tag = new InsertArgs("^", "$", "group");
        miRegexMatchAnchorStartEnd.Click += InsertRegexFragment;
        // 
        // miRegexMatchAnchorBound
        // 
        miRegexMatchAnchorBound.Name = "miRegexMatchAnchorBound";
        miRegexMatchAnchorBound.Size = new System.Drawing.Size(195, 22);
        miRegexMatchAnchorBound.Text = "Word boundary\t\\b";
        miRegexMatchAnchorBound.Tag = new InsertArgs("\\b", "", "wrap");
        miRegexMatchAnchorBound.Click += InsertRegexFragment;
        // 
        // miRegexMatchAnchorNonBound
        // 
        miRegexMatchAnchorNonBound.Name = "miRegexMatchAnchorNonBound";
        miRegexMatchAnchorNonBound.Size = new System.Drawing.Size(195, 22);
        miRegexMatchAnchorNonBound.Text = "Non-word boundary\t\\B";
        miRegexMatchAnchorNonBound.Tag = new InsertArgs("\\B", "", "wrap");
        miRegexMatchAnchorNonBound.Click += InsertRegexFragment;


        var miRegexMatchGroup = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchGroupCapt = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchGroupNonCapt = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchGroupAlt = new System.Windows.Forms.ToolStripMenuItem();
        // 
        // miRegexMatchGroup
        // 
        miRegexMatchGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchGroupCapt, miRegexMatchGroupNonCapt, miRegexMatchGroupAlt });
        miRegexMatchGroup.Name = "miRegexMatchGroup";
        miRegexMatchGroup.Size = new System.Drawing.Size(153, 22);
        miRegexMatchGroup.Text = "Group";
        // 
        // miRegexMatchGroupCapt
        // 
        miRegexMatchGroupCapt.Name = "miRegexMatchGroupCapt";
        miRegexMatchGroupCapt.Size = new System.Drawing.Size(185, 22);
        miRegexMatchGroupCapt.Text = "With capture\t(...)";
        miRegexMatchGroupCapt.Tag = new InsertArgs("(", ")");
        miRegexMatchGroupCapt.Click += InsertRegexFragment;
        // 
        // miRegexMatchGroupNonCapt
        // 
        miRegexMatchGroupNonCapt.Name = "miRegexMatchGroupNonCapt";
        miRegexMatchGroupNonCapt.Size = new System.Drawing.Size(185, 22);
        miRegexMatchGroupNonCapt.Text = "Without capture\t(?:...)";
        miRegexMatchGroupNonCapt.Tag = new InsertArgs("(?:", ")");
        miRegexMatchGroupNonCapt.Click += InsertRegexFragment;
        // 
        // miRegexMatchGroupAlt
        // 
        miRegexMatchGroupAlt.Name = "miRegexMatchGroupAlt";
        miRegexMatchGroupAlt.Size = new System.Drawing.Size(185, 22);
        miRegexMatchGroupAlt.Text = "Alternative\t(...|...)";
        miRegexMatchGroupAlt.Tag = new InsertArgs("(", "|)", -1, 0);
        miRegexMatchGroupAlt.Click += InsertRegexFragment;

        var miRegexMatchQuant = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantGreedy = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantZeroOneG = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantOneMoreG = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantZeroMoreG = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantExactG = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantAtLeastG = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantBetweenG = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantLazy = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantZeroOneL = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantOneMoreL = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantZeroMoreL = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantExactL = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantAtLeastL = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchQuantBetweenL = new System.Windows.Forms.ToolStripMenuItem();


        // 
        // miRegexMatchQuant
        // 
        miRegexMatchQuant.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchQuantGreedy, miRegexMatchQuantZeroOneG, miRegexMatchQuantOneMoreG, miRegexMatchQuantZeroMoreG, miRegexMatchQuantExactG, miRegexMatchQuantAtLeastG, miRegexMatchQuantBetweenG, miRegexMatchQuantLazy, miRegexMatchQuantZeroOneL, miRegexMatchQuantOneMoreL, miRegexMatchQuantZeroMoreL, miRegexMatchQuantExactL, miRegexMatchQuantAtLeastL, miRegexMatchQuantBetweenL });
        miRegexMatchQuant.Name = "miRegexMatchQuant";
        miRegexMatchQuant.Size = new System.Drawing.Size(153, 22);
        miRegexMatchQuant.Text = "Quantifiers";
        // 
        // miRegexMatchQuantGreedy
        // 
        miRegexMatchQuantGreedy.Enabled = false;
        miRegexMatchQuantGreedy.Name = "miRegexMatchQuantGreedy";
        miRegexMatchQuantGreedy.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantGreedy.Text = "Match as much as possible";
        // 
        // miRegexMatchQuantZeroOneG
        // 
        miRegexMatchQuantZeroOneG.Name = "miRegexMatchQuantZeroOneG";
        miRegexMatchQuantZeroOneG.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantZeroOneG.Text = "Zero or one times\t?";
        miRegexMatchQuantZeroOneG.Tag = new InsertArgs("", "?", "group");
        miRegexMatchQuantZeroOneG.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantOneMoreG
        // 
        miRegexMatchQuantOneMoreG.Name = "miRegexMatchQuantOneMoreG";
        miRegexMatchQuantOneMoreG.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantOneMoreG.Text = "One or more times\t+";
        miRegexMatchQuantOneMoreG.Tag = new InsertArgs("", "+", "group");
        miRegexMatchQuantOneMoreG.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantZeroMoreG
        // 
        miRegexMatchQuantZeroMoreG.Name = "miRegexMatchQuantZeroMoreG";
        miRegexMatchQuantZeroMoreG.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantZeroMoreG.Text = "Zero or more times\t*";
        miRegexMatchQuantZeroMoreG.Tag = new InsertArgs("", "*", "group");
        miRegexMatchQuantZeroMoreG.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantExactG
        // 
        miRegexMatchQuantExactG.Name = "miRegexMatchQuantExactG";
        miRegexMatchQuantExactG.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantExactG.Text = "Exactly n times\t{n}";
        miRegexMatchQuantExactG.Tag = new InsertArgs("", "{n}", -2, 1, "group");
        miRegexMatchQuantExactG.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantAtLeastG
        // 
        miRegexMatchQuantAtLeastG.Name = "miRegexMatchQuantAtLeastG";
        miRegexMatchQuantAtLeastG.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantAtLeastG.Text = "At least n times\t{n,}";
        miRegexMatchQuantAtLeastG.Tag = new InsertArgs("", "{n,}", -3, 1, "group");
        miRegexMatchQuantAtLeastG.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantBetweenG
        // 
        miRegexMatchQuantBetweenG.Name = "miRegexMatchQuantBetweenG";
        miRegexMatchQuantBetweenG.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantBetweenG.Text = "Between n to m times\t{n,m}";
        miRegexMatchQuantBetweenG.Tag = new InsertArgs("", "{n,m}", -4, 3, "group");
        miRegexMatchQuantBetweenG.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantLazy
        // 
        miRegexMatchQuantLazy.Enabled = false;
        miRegexMatchQuantLazy.Name = "miRegexMatchQuantLazy";
        miRegexMatchQuantLazy.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantLazy.Text = "Match as little as possible";
        // 
        // miRegexMatchQuantZeroOneL
        // 
        miRegexMatchQuantZeroOneL.Name = "miRegexMatchQuantZeroOneL";
        miRegexMatchQuantZeroOneL.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantZeroOneL.Text = "Zero or one times\t??";
        miRegexMatchQuantZeroOneL.Tag = new InsertArgs("", "??", "group");
        miRegexMatchQuantZeroOneL.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantOneMoreL
        // 
        miRegexMatchQuantOneMoreL.Name = "miRegexMatchQuantOneMoreL";
        miRegexMatchQuantOneMoreL.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantOneMoreL.Text = "One or more times\t+?";
        miRegexMatchQuantOneMoreL.Tag = new InsertArgs("", "+?", "group");
        miRegexMatchQuantOneMoreL.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantZeroMoreL
        // 
        miRegexMatchQuantZeroMoreL.Name = "miRegexMatchQuantZeroMoreL";
        miRegexMatchQuantZeroMoreL.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantZeroMoreL.Text = "Zero or more times\t*?";
        miRegexMatchQuantZeroMoreL.Tag = new InsertArgs("", "*?", "group");
        miRegexMatchQuantZeroMoreL.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantExactL
        // 
        miRegexMatchQuantExactL.Name = "miRegexMatchQuantExactL";
        miRegexMatchQuantExactL.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantExactL.Text = "Exactly n times\t{n}?";
        miRegexMatchQuantExactL.Tag = new InsertArgs("", "{n}?", -3, 1, "group");
        miRegexMatchQuantExactL.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantAtLeastL
        // 
        miRegexMatchQuantAtLeastL.Name = "miRegexMatchQuantAtLeastL";
        miRegexMatchQuantAtLeastL.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantAtLeastL.Text = "At least n times\t{n,}?";
        miRegexMatchQuantAtLeastL.Tag = new InsertArgs("", "{n,}?", -4, 1, "group");
        miRegexMatchQuantAtLeastL.Click += InsertRegexFragment;
        // 
        // miRegexMatchQuantBetweenL
        // 
        miRegexMatchQuantBetweenL.Name = "miRegexMatchQuantBetweenL";
        miRegexMatchQuantBetweenL.Size = new System.Drawing.Size(223, 22);
        miRegexMatchQuantBetweenL.Text = "Between n to m times\t{n,m}?";
        miRegexMatchQuantBetweenL.Tag = new InsertArgs("", "{n,m}?", -5, 3, "group");
        miRegexMatchQuantBetweenL.Click += InsertRegexFragment;


        var miRegexMatchClass = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchClassPos = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchClassNeg = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchClassLower = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchClassUpper = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexMatchClass
        // 
        miRegexMatchClass.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchClassPos, miRegexMatchClassNeg, miRegexMatchClassLower, miRegexMatchClassUpper });
        miRegexMatchClass.Name = "miRegexMatchClass";
        miRegexMatchClass.Size = new System.Drawing.Size(153, 22);
        miRegexMatchClass.Text = "Character class";
        // 
        // miRegexMatchClassPos
        // 
        miRegexMatchClassPos.Name = "miRegexMatchClassPos";
        miRegexMatchClassPos.Size = new System.Drawing.Size(174, 22);
        miRegexMatchClassPos.Text = "Positive class\t[...]";
        miRegexMatchClassPos.Tag = new InsertArgs("[", "]");
        miRegexMatchClassPos.Click += InsertRegexFragment;
        // 
        // miRegexMatchClassNeg
        // 
        miRegexMatchClassNeg.Name = "miRegexMatchClassNeg";
        miRegexMatchClassNeg.Size = new System.Drawing.Size(174, 22);
        miRegexMatchClassNeg.Text = "Negative class\t[^...]";
        miRegexMatchClassNeg.Tag = new InsertArgs("[^", "]");
        miRegexMatchClassNeg.Click += InsertRegexFragment;
        // 
        // miRegexMatchClassLower
        // 
        miRegexMatchClassLower.Name = "miRegexMatchClassLower";
        miRegexMatchClassLower.Size = new System.Drawing.Size(174, 22);
        miRegexMatchClassLower.Text = "Lowercase\t[a-z]";
        miRegexMatchClassLower.Tag = new InsertArgs("[a-z]");
        miRegexMatchClassLower.Click += InsertRegexFragment;
        // 
        // miRegexMatchClassUpper
        // 
        miRegexMatchClassUpper.Name = "miRegexMatchClassUpper";
        miRegexMatchClassUpper.Size = new System.Drawing.Size(174, 22);
        miRegexMatchClassUpper.Text = "Uppercase\t[A-Z]";
        miRegexMatchClassUpper.Tag = new InsertArgs("[A-Z]");
        miRegexMatchClassUpper.Click += InsertRegexFragment;

        var miRegexMatchCapt = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchCaptCreateUnnamed = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchCaptMatchUnnamed = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchCaptCreateNamed = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchCaptMatchNamed = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexMatchCapt
        // 
        miRegexMatchCapt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchCaptCreateUnnamed, miRegexMatchCaptMatchUnnamed, miRegexMatchCaptCreateNamed, miRegexMatchCaptMatchNamed });
        miRegexMatchCapt.Name = "miRegexMatchCapt";
        miRegexMatchCapt.Size = new System.Drawing.Size(153, 22);
        miRegexMatchCapt.Text = "Captures";
        // 
        // miRegexMatchCaptCreateUnnamed
        // 
        miRegexMatchCaptCreateUnnamed.Name = "miRegexMatchCaptCreateUnnamed";
        miRegexMatchCaptCreateUnnamed.Size = new System.Drawing.Size(259, 22);
        miRegexMatchCaptCreateUnnamed.Text = "Create unnamed capture\t(...)";
        miRegexMatchCaptCreateUnnamed.Tag = new InsertArgs("(", ")");
        miRegexMatchCaptCreateUnnamed.Click += InsertRegexFragment;
        // 
        // miRegexMatchCaptMatchUnnamed
        // 
        miRegexMatchCaptMatchUnnamed.Name = "miRegexMatchCaptMatchUnnamed";
        miRegexMatchCaptMatchUnnamed.Size = new System.Drawing.Size(259, 22);
        miRegexMatchCaptMatchUnnamed.Text = "Match unnamed capture\t\\n";
        miRegexMatchCaptMatchUnnamed.Tag = new InsertArgs("\\n", "", 1, 1);
        miRegexMatchCaptMatchUnnamed.Click += InsertRegexFragment;
        // 
        // miRegexMatchCaptCreateNamed
        // 
        miRegexMatchCaptCreateNamed.Name = "miRegexMatchCaptCreateNamed";
        miRegexMatchCaptCreateNamed.Size = new System.Drawing.Size(259, 22);
        miRegexMatchCaptCreateNamed.Text = "Create named capture\t(?<name>...)";
        miRegexMatchCaptCreateNamed.Tag = new InsertArgs("(?<name>", ")", 3, 4);
        miRegexMatchCaptCreateNamed.Click += InsertRegexFragment;
        // 
        // miRegexMatchCaptMatchNamed
        // 
        miRegexMatchCaptMatchNamed.Name = "miRegexMatchCaptMatchNamed";
        miRegexMatchCaptMatchNamed.Size = new System.Drawing.Size(259, 22);
        miRegexMatchCaptMatchNamed.Text = "Match named capture\t\\<name>";
        miRegexMatchCaptMatchNamed.Tag = new InsertArgs("\\<name>", "", 2, 4);
        miRegexMatchCaptMatchNamed.Click += InsertRegexFragment;


        var miRegexMatchLook = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLookPosAhead = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLookNegAhead = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLookPosBehind = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLookNegBehind = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexMatchLook
        // 
        miRegexMatchLook.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchLookPosAhead, miRegexMatchLookNegAhead, miRegexMatchLookPosBehind, miRegexMatchLookNegBehind });
        miRegexMatchLook.Name = "miRegexMatchLook";
        miRegexMatchLook.Size = new System.Drawing.Size(153, 22);
        miRegexMatchLook.Text = "Lookaround";
        // 
        // miRegexMatchLookPosAhead
        // 
        miRegexMatchLookPosAhead.Name = "miRegexMatchLookPosAhead";
        miRegexMatchLookPosAhead.Size = new System.Drawing.Size(217, 22);
        miRegexMatchLookPosAhead.Text = "Positive lookahead\t(?=...)";
        miRegexMatchLookPosAhead.Tag = new InsertArgs("(?=", ")");
        miRegexMatchLookPosAhead.Click += InsertRegexFragment;
        // 
        // miRegexMatchLookNegAhead
        // 
        miRegexMatchLookNegAhead.Name = "miRegexMatchLookNegAhead";
        miRegexMatchLookNegAhead.Size = new System.Drawing.Size(217, 22);
        miRegexMatchLookNegAhead.Text = "Negative lookahead\t(?!...)";
        miRegexMatchLookNegAhead.Tag = new InsertArgs("(?!", ")");
        miRegexMatchLookNegAhead.Click += InsertRegexFragment;
        // 
        // miRegexMatchLookPosBehind
        // 
        miRegexMatchLookPosBehind.Name = "miRegexMatchLookPosBehind";
        miRegexMatchLookPosBehind.Size = new System.Drawing.Size(217, 22);
        miRegexMatchLookPosBehind.Text = "Positive lookbehind\t(?<=...)";
        miRegexMatchLookPosBehind.Tag = new InsertArgs("(?<=", ")");
        miRegexMatchLookPosBehind.Click += InsertRegexFragment;
        // 
        // miRegexMatchLookNegBehind
        // 
        miRegexMatchLookNegBehind.Name = "miRegexMatchLookNegBehind";
        miRegexMatchLookNegBehind.Size = new System.Drawing.Size(217, 22);
        miRegexMatchLookNegBehind.Text = "Negative lookbehind\t(?<!...)";
        miRegexMatchLookNegBehind.Tag = new InsertArgs("(?<!", ")");
        miRegexMatchLookNegBehind.Click += InsertRegexFragment;

        var miRegexMatchSep1 = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexMatchSep1
        // 
        miRegexMatchSep1.Name = "miRegexMatchSep1";
        miRegexMatchSep1.Size = new System.Drawing.Size(153, 22);
        miRegexMatchSep1.Text = "-";

        var miRegexMatchLiteral = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralDot = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralQuestion = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralPlus = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralStar = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralCaret = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralDollar = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralBackslash = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralOpenRound = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralCloseRound = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralOpenSquare = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralCloseSquare = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralOpenCurly = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralCloseCurly = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexMatchLiteralPipe = new System.Windows.Forms.ToolStripMenuItem();


        // 
        // miRegexMatchLiteral
        // 
        miRegexMatchLiteral.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchLiteralDot, miRegexMatchLiteralQuestion, miRegexMatchLiteralPlus, miRegexMatchLiteralStar, miRegexMatchLiteralCaret, miRegexMatchLiteralDollar, miRegexMatchLiteralBackslash, miRegexMatchLiteralOpenRound, miRegexMatchLiteralCloseRound, miRegexMatchLiteralOpenSquare, miRegexMatchLiteralCloseSquare, miRegexMatchLiteralOpenCurly, miRegexMatchLiteralCloseCurly, miRegexMatchLiteralPipe });
        miRegexMatchLiteral.Name = "miRegexMatchLiteral";
        miRegexMatchLiteral.Size = new System.Drawing.Size(153, 22);
        miRegexMatchLiteral.Text = "Literals";
        // 
        // miRegexMatchLiteralDot
        // 
        miRegexMatchLiteralDot.Name = "miRegexMatchLiteralDot";
        miRegexMatchLiteralDot.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralDot.Text = "Dot\t\\.";
        miRegexMatchLiteralDot.Tag = new InsertArgs("\\.");
        miRegexMatchLiteralDot.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralQuestion
        // 
        miRegexMatchLiteralQuestion.Name = "miRegexMatchLiteralQuestion";
        miRegexMatchLiteralQuestion.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralQuestion.Text = "Question mark\t\\?";
        miRegexMatchLiteralQuestion.Tag = new InsertArgs("\\?");
        miRegexMatchLiteralQuestion.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralPlus
        // 
        miRegexMatchLiteralPlus.Name = "miRegexMatchLiteralPlus";
        miRegexMatchLiteralPlus.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralPlus.Text = "Plus sign\t\\+";
        miRegexMatchLiteralPlus.Tag = new InsertArgs("\\+");
        miRegexMatchLiteralPlus.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralStar
        // 
        miRegexMatchLiteralStar.Name = "miRegexMatchLiteralStar";
        miRegexMatchLiteralStar.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralStar.Text = "Star\t\\*";
        miRegexMatchLiteralStar.Tag = new InsertArgs("\\*");
        miRegexMatchLiteralStar.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralCaret
        // 
        miRegexMatchLiteralCaret.Name = "miRegexMatchLiteralCaret";
        miRegexMatchLiteralCaret.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralCaret.Text = "Caret\t\\^";
        miRegexMatchLiteralCaret.Tag = new InsertArgs("\\^");
        miRegexMatchLiteralCaret.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralDollar
        // 
        miRegexMatchLiteralDollar.Name = "miRegexMatchLiteralDollar";
        miRegexMatchLiteralDollar.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralDollar.Text = "Dollar sign\t\\$";
        miRegexMatchLiteralDollar.Tag = new InsertArgs("\\$");
        miRegexMatchLiteralDollar.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralBackslash
        // 
        miRegexMatchLiteralBackslash.Name = "miRegexMatchLiteralBackslash";
        miRegexMatchLiteralBackslash.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralBackslash.Text = "Backslash\t\\\\";
        miRegexMatchLiteralBackslash.Tag = new InsertArgs("\\\\");
        miRegexMatchLiteralBackslash.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralOpenRound
        // 
        miRegexMatchLiteralOpenRound.Name = "miRegexMatchLiteralOpenRound";
        miRegexMatchLiteralOpenRound.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralOpenRound.Text = "Open round bracket\t\\(";
        miRegexMatchLiteralOpenRound.Tag = new InsertArgs("\\(");
        miRegexMatchLiteralOpenRound.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralCloseRound
        // 
        miRegexMatchLiteralCloseRound.Name = "miRegexMatchLiteralCloseRound";
        miRegexMatchLiteralCloseRound.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralCloseRound.Text = "Close round bracket\t\\)";
        miRegexMatchLiteralCloseRound.Tag = new InsertArgs("\\)");
        miRegexMatchLiteralCloseRound.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralOpenSquare
        // 
        miRegexMatchLiteralOpenSquare.Name = "miRegexMatchLiteralOpenSquare";
        miRegexMatchLiteralOpenSquare.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralOpenSquare.Text = "Open square bracket\t\\[";
        miRegexMatchLiteralOpenSquare.Tag = new InsertArgs("\\[");
        miRegexMatchLiteralOpenSquare.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralCloseSquare
        // 
        miRegexMatchLiteralCloseSquare.Name = "miRegexMatchLiteralCloseSquare";
        miRegexMatchLiteralCloseSquare.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralCloseSquare.Text = "Close square bracket\t\\]";
        miRegexMatchLiteralCloseSquare.Tag = new InsertArgs("\\]");
        miRegexMatchLiteralCloseSquare.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralOpenCurly
        // 
        miRegexMatchLiteralOpenCurly.Name = "miRegexMatchLiteralOpenCurly";
        miRegexMatchLiteralOpenCurly.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralOpenCurly.Text = "Open curly bracket\t\\{";
        miRegexMatchLiteralOpenCurly.Tag = new InsertArgs("\\{");
        miRegexMatchLiteralOpenCurly.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralCloseCurly
        // 
        miRegexMatchLiteralCloseCurly.Name = "miRegexMatchLiteralCloseCurly";
        miRegexMatchLiteralCloseCurly.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralCloseCurly.Text = "Close curly bracket\t\\}";
        miRegexMatchLiteralCloseCurly.Tag = new InsertArgs("\\}");
        miRegexMatchLiteralCloseCurly.Click += InsertRegexFragment;
        // 
        // miRegexMatchLiteralPipe
        // 
        miRegexMatchLiteralPipe.Name = "miRegexMatchLiteralPipe";
        miRegexMatchLiteralPipe.Size = new System.Drawing.Size(192, 22);
        miRegexMatchLiteralPipe.Text = "Pipe\t\\|";
        miRegexMatchLiteralPipe.Tag = new InsertArgs("\\|");
        miRegexMatchLiteralPipe.Click += InsertRegexFragment;


        // 
        // cmRegexMatch
        // 
        cmRegexMatch.ImageScalingSize = new System.Drawing.Size(20, 20);
        cmRegexMatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexMatchMatch, miRegexMatchAnchor, miRegexMatchGroup, miRegexMatchQuant, miRegexMatchClass, miRegexMatchCapt, miRegexMatchLook, miRegexMatchSep1, miRegexMatchLiteral });
        cmRegexMatch.Name = "cmRegexMatch";
        cmRegexMatch.Size = new System.Drawing.Size(154, 202);

    }

    private void CreateRegexReplaceContextMenu(IContainer components)
    {
        cmRegexReplace = new System.Windows.Forms.ContextMenuStrip(components);

        var miRegexReplaceCapture = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceCaptureUnnamed = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceCaptureNamed = new System.Windows.Forms.ToolStripMenuItem();


        // 
        // miRegexReplaceCapture
        // 
        miRegexReplaceCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceCaptureUnnamed, miRegexReplaceCaptureNamed });
        miRegexReplaceCapture.Name = "miRegexReplaceCapture";
        miRegexReplaceCapture.Size = new System.Drawing.Size(138, 22);
        miRegexReplaceCapture.Text = "Capture";
        // 
        // miRegexReplaceCaptureUnnamed
        // 
        miRegexReplaceCaptureUnnamed.Name = "miRegexReplaceCaptureUnnamed";
        miRegexReplaceCaptureUnnamed.Size = new System.Drawing.Size(157, 22);
        miRegexReplaceCaptureUnnamed.Text = "Unnamed\t$n";
        miRegexReplaceCaptureUnnamed.Tag = new InsertArgs("$n", "", 1, 1);
        miRegexReplaceCaptureUnnamed.Click += InsertRegexFragment;
        // 
        // miRegexReplaceCaptureNamed
        // 
        miRegexReplaceCaptureNamed.Name = "miRegexReplaceCaptureNamed";
        miRegexReplaceCaptureNamed.Size = new System.Drawing.Size(157, 22);
        miRegexReplaceCaptureNamed.Text = "Named\t${name}";
        miRegexReplaceCaptureNamed.Tag = new InsertArgs("${name}", "", 2, 4);
        miRegexReplaceCaptureNamed.Click += InsertRegexFragment;

        var miRegexReplaceOrig = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceOrigMatched = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceOrigBefore = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceOrigAfter = new System.Windows.Forms.ToolStripMenuItem();
        miRegexReplaceOrigAll = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexReplaceOrig
        // 
        miRegexReplaceOrig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceOrigMatched, miRegexReplaceOrigBefore, miRegexReplaceOrigAfter, miRegexReplaceOrigAll });
        miRegexReplaceOrig.Name = "miRegexReplaceOrig";
        miRegexReplaceOrig.Size = new System.Drawing.Size(138, 22);
        miRegexReplaceOrig.Text = "Original text";
        // 
        // miRegexReplaceOrigMatched
        // 
        miRegexReplaceOrigMatched.Name = "miRegexReplaceOrigMatched";
        miRegexReplaceOrigMatched.Size = new System.Drawing.Size(178, 22);
        miRegexReplaceOrigMatched.Text = "Matched text\t$0";
        miRegexReplaceOrigMatched.Tag = new InsertArgs("$0");
        miRegexReplaceOrigMatched.Click += InsertRegexFragment;
        // 
        // miRegexReplaceOrigBefore
        // 
        miRegexReplaceOrigBefore.Name = "miRegexReplaceOrigBefore";
        miRegexReplaceOrigBefore.Size = new System.Drawing.Size(178, 22);
        miRegexReplaceOrigBefore.Text = "Text before match\t$`";
        miRegexReplaceOrigBefore.Tag = new InsertArgs("$`");
        miRegexReplaceOrigBefore.Click += InsertRegexFragment;
        // 
        // miRegexReplaceOrigAfter
        // 
        miRegexReplaceOrigAfter.Name = "miRegexReplaceOrigAfter";
        miRegexReplaceOrigAfter.Size = new System.Drawing.Size(178, 22);
        miRegexReplaceOrigAfter.Text = "Text after match\t$'";
        miRegexReplaceOrigAfter.Tag = new InsertArgs("$'");
        miRegexReplaceOrigAfter.Click += InsertRegexFragment;
        // 
        // miRegexReplaceOrigAll
        // 
        miRegexReplaceOrigAll.Name = "miRegexReplaceOrigAll";
        miRegexReplaceOrigAll.Size = new System.Drawing.Size(178, 22);
        miRegexReplaceOrigAll.Text = "Original filename\t$_";
        miRegexReplaceOrigAll.Tag = new InsertArgs("$_");
        miRegexReplaceOrigAll.Click += InsertRegexFragment;

        var miRegexReplaceSpecial = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceSpecialNumSeq = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexReplaceSpecial
        // 
        miRegexReplaceSpecial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceSpecialNumSeq });
        miRegexReplaceSpecial.Name = "miRegexReplaceSpecial";
        miRegexReplaceSpecial.Size = new System.Drawing.Size(138, 22);
        miRegexReplaceSpecial.Text = "Special";
        // 
        // miRegexReplaceSpecialNumSeq
        // 
        miRegexReplaceSpecialNumSeq.Name = "miRegexReplaceSpecialNumSeq";
        miRegexReplaceSpecialNumSeq.Size = new System.Drawing.Size(184, 22);
        miRegexReplaceSpecialNumSeq.Text = "Number sequence\t$#";
        miRegexReplaceSpecialNumSeq.Tag = new InsertArgs("$#");
        miRegexReplaceSpecialNumSeq.Click += InsertRegexFragment;

        var miRegexReplaceSep1 = new System.Windows.Forms.ToolStripMenuItem();

        // 
        // miRegexReplaceSep1
        // 
        miRegexReplaceSep1.Name = "miRegexReplaceSep1";
        miRegexReplaceSep1.Size = new System.Drawing.Size(138, 22);
        miRegexReplaceSep1.Text = "-";

        var miRegexReplaceLiteral = new System.Windows.Forms.ToolStripMenuItem();
        var miRegexReplaceLiteralDollar = new System.Windows.Forms.ToolStripMenuItem();
        // 
        // miRegexReplaceLiteral
        // 
        miRegexReplaceLiteral.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceLiteralDollar });
        miRegexReplaceLiteral.Name = "miRegexReplaceLiteral";
        miRegexReplaceLiteral.Size = new System.Drawing.Size(138, 22);
        miRegexReplaceLiteral.Text = "Literals";
        // 
        // miRegexReplaceLiteralDollar
        // 
        miRegexReplaceLiteralDollar.Name = "miRegexReplaceLiteralDollar";
        miRegexReplaceLiteralDollar.Size = new System.Drawing.Size(142, 22);
        miRegexReplaceLiteralDollar.Text = "Dollar sign\t$$";
        miRegexReplaceLiteralDollar.Tag = new InsertArgs("$$");
        miRegexReplaceLiteralDollar.Click += InsertRegexFragment;


        // 
        // cmRegexReplace
        // 
        cmRegexReplace.ImageScalingSize = new System.Drawing.Size(20, 20);
        cmRegexReplace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miRegexReplaceCapture, miRegexReplaceOrig, miRegexReplaceSpecial, miRegexReplaceSep1, miRegexReplaceLiteral });
        cmRegexReplace.Name = "cmRegexReplace";
        cmRegexReplace.Size = new System.Drawing.Size(139, 114);

    }

    
    private void CreateGlobContextMenu(IContainer components)
    {
        cmGlobMatch = new System.Windows.Forms.ContextMenuStrip(components);
        var miGlobMatchSingle = new System.Windows.Forms.ToolStripMenuItem();
        var miGlobMatchMultiple = new System.Windows.Forms.ToolStripMenuItem();
        // 
        // cmGlobMatch
        // 
        cmGlobMatch.ImageScalingSize = new System.Drawing.Size(20, 20);
        cmGlobMatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miGlobMatchSingle, miGlobMatchMultiple });
        cmGlobMatch.Name = "cmGlobMatch";
        cmGlobMatch.Size = new System.Drawing.Size(181, 48);
        // 
        // miGlobMatchSingle
        // 
        miGlobMatchSingle.Name = "miGlobMatchSingle";
        miGlobMatchSingle.Size = new System.Drawing.Size(180, 22);
        miGlobMatchSingle.Text = "Single character\t?";
        miGlobMatchSingle.Tag = new InsertArgs("?");
        miGlobMatchSingle.Click += InsertRegexFragment;
        // 
        // miGlobMatchMultiple
        // 
        miGlobMatchMultiple.Name = "miGlobMatchMultiple";
        miGlobMatchMultiple.Size = new System.Drawing.Size(180, 22);
        miGlobMatchMultiple.Text = "Multiple characters\t*";
        miGlobMatchMultiple.Tag = new InsertArgs("*");
        miGlobMatchMultiple.Click += InsertRegexFragment;

    }
}
