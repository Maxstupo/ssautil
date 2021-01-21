namespace Maxstupo.SsaUtil {

    using System.Collections.Generic;
    using CommandLine;

    public abstract class BaseOptions {

        [Option('i', "input", HelpText = "Directories and/or filepaths to SSA files that will be parsed. Directories will be scanned top level only unless -r switch is used. Only accepts *.ass files.", Min = 1)]
        public IEnumerable<string> Inputs { get; set; }

        [Option('r', "recursive", HelpText = "Scan the entire directory tree of all directory filepaths recursively.")]
        public bool Recursive { get; set; }


        [Option('s', "styles", HelpText = "Set and scale options will be applied to the properties of the subtitle styles.")]
        public bool ScopeStyles { get; set; }

        [Option('e', "events", HelpText = "Set and scale options will be applied to the properties of the subtitle events.")]
        public bool ScopeEvents { get; set; }


        [Option("ignore-errors", HelpText = "Ignore errors in subtitle files.")]
        public bool IgnoreErrors { get; set; }

    }


    [Verb("edit", HelpText = "Edit single or multiple subtitle files at once.")]
    public sealed class EditOptions : BaseOptions {


        [Option("set", HelpText = "Set subtitle properties within the current scope. See -s and -e switches.", Min = 1)]
        public IEnumerable<string> Setters { get; set; }

        [Option("scale", HelpText = "Scale subtitle properties within the current scope. See -s and -e switches.", Min = 1)]
        public IEnumerable<string> Scalars { get; set; }

        [Option("filter", HelpText = "Filter subtitle properties within the current scope. See -s and -e switches.", Min = 1)]
        public IEnumerable<string> Filters { get; set; }


        [Option('o', "output", HelpText = "The output directory for saving modified subtitle files.")]
        public string Output { get; set; }


        [Option("overwrite", HelpText = "Allow overwriting existing files.")]
        public bool Overwrite { get; set; }

    }


    [Verb("info", HelpText = "<TODO>")]
    public sealed class InfoOptions : BaseOptions {



    }


}