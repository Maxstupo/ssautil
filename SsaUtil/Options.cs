namespace Maxstupo.SsaUtil {

    using System.Collections.Generic;
    using CommandLine;

    public abstract class BaseOptions {

        [Option('i', "input", HelpText = "<TODO>")]
        public IEnumerable<string> Inputs { get; set; }

        [Option('r', "recursive", HelpText = "<TODO>")]
        public bool Recursive { get; set; }

    }


    [Verb("edit", HelpText = "<TODO>")]
    public sealed class EditOptions : BaseOptions {



    }


    [Verb("info", HelpText = "<TODO>")]
    public sealed class InfoOptions : BaseOptions {



    }


}