namespace Maxstupo.SsaUtil {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommandLine;
    using Maxstupo.SsaUtil.Subtitles;
    using Maxstupo.SsaUtil.Utility;

    public sealed class Program {

        private IOutput Output { get; } = new ColorConsole();

        private readonly SsaReader Reader = new SsaReader();


        private HashSet<string> inputFiles;


        private int? Init(BaseOptions options) {

            inputFiles = options.Inputs.SelectMany(filepath => {
                if (Directory.Exists(filepath)) {
                    return Directory.EnumerateFiles(filepath, "*.ass", options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                } else if (Path.GetExtension(filepath).Equals(".ass", StringComparison.InvariantCultureIgnoreCase)) {
                    return Enumerable.Repeat(filepath, 1);

                } else {
                    return Enumerable.Empty<string>();

                }
            }).ToHashSet();

            if (inputFiles.Count == 0) {
                Output.WriteLine(Level.Severe, "No input files specified! Use the &-e;-i&-^; option.");
                return -1;
            }

            return null;
        }

        private int RunEdit(EditOptions options) {

            foreach (string file in inputFiles) {
                if (!File.Exists(file)) {
                    Output.WriteLine(Level.Warn, $"File doesn't exist &-e;{file}&-^; Ignoring...");
                    continue;
                }

                Output.WriteLine(Level.None, $"Parsing: {file}");

                SsaSubtitle subtitle = Reader.ReadFrom(file);
                if (CheckReaderErrors())
                    return -1;


                Output.WriteLine(Level.Info, $"  - Title: {subtitle.Title}");
                Output.WriteLine(Level.Info, $"  - # of Styles: {subtitle.Styles.Count}");
                Output.WriteLine(Level.Info);
            }

            return 0;
        }

        private int RunInfo(InfoOptions options) {

            Output.WriteLine(Level.Severe, "Info verb isn't implement yet!");

            return -1;
        }

        private bool CheckReaderErrors() {
            if (!Reader.HasErrors)
                return false;

            foreach (SsaError error in Reader)
                Output.WriteLine(Level.Error, $"  - {string.Format(error.Message, error.Args.Select(x => $"&-e;{x}&-^;").ToArray())}");

            return true;
        }

        private static int Main(string[] args) {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                args = new string[] { "edit", "--help" };
#endif

            Program program = new Program();

            return OnReturnCode(Parser.Default.ParseArguments<InfoOptions, EditOptions>(args)
                .MapResult(
                    (InfoOptions options) => { int? code = program.Init(options); return code ?? program.RunInfo(options); },
                    (EditOptions options) => { int? code = program.Init(options); return code ?? program.RunEdit(options); },
                    errors => -1
                ));
        }

        private static int OnReturnCode(int returnCode) {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                Console.ReadKey();
#endif
            return returnCode;
        }

    }

}