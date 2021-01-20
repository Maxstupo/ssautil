namespace Maxstupo.SsaUtil {

    using System.Collections.Generic;
    using CommandLine;

    public abstract class BaseOptions {

        [Option('i', "input", HelpText = "Directories and/or filepaths to SSA files that will be parsed. Directories will be scanned top level only unless -r switch is used. Only accepts *.ass files.")]
        public IEnumerable<string> Inputs { get; set; }

        [Option('r', "recursive", HelpText = "Scan the entire directory tree of all directory filepaths recursively.")]
        public bool Recursive { get; set; }

    }


    [Verb("edit", HelpText = "<TODO>")]
    public sealed class EditOptions : BaseOptions {



    }


    [Verb("info", HelpText = "<TODO>")]
    public sealed class InfoOptions : BaseOptions {



    }


}