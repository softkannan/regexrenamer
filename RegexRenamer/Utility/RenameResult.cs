using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Utility
{
    public class RenameResult
    {
        private int countSuccess = 0;
        private int countErrors = 0;
        private RenameErrorDialog errorDialog = null;

        public bool Cancelled = false;
        public bool RenameToSubfolders = false;

        public bool AnyErrors
        {
            get
            {
                return countErrors != 0;
            }
        }
        public int FilesRenamed
        {
            get
            {
                return countSuccess;
            }
        }

        public void ReportSuccess()
        {
            countSuccess++;
        }
        public void ReportError(string oldname, string newname, string error)
        {
            countErrors++;

            if (errorDialog == null)
                errorDialog = new RenameErrorDialog();

            errorDialog.AddEntry(oldname, newname, error);
        }

        public void ShowErrorDialog(string strFile)
        {
            int countTotal = countSuccess + countErrors;
            errorDialog.Message = "The following " + (countErrors == 1 ? "error" : "errors")
                                + " occured during the batch rename.\n" + countSuccess + " of " + countTotal + " " + strFile
                                + (countTotal == 1 ? " was" : "s were") + " renamed successfully.";
            errorDialog.AutoSizeColumns();
            errorDialog.ShowDialog();
        }
    }
}
