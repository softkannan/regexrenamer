using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexRenamer.Tools.Translate;

public class GoogleTranslateIPBannedException : Exception
{
    public enum Operation { TokenGeneration, Translation }

    public Operation OperationBanned { get; }

    public GoogleTranslateIPBannedException(string message, Operation operation)
      : base("Google translate banned this IP for some time (about a few hours). " + message)
    {
        OperationBanned = operation;
    }

    public GoogleTranslateIPBannedException(Operation operation)
      : this(String.Empty, operation) { }
}
