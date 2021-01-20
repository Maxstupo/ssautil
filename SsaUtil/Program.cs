namespace Maxstupo.SsaUtil {

    using System;
    using System.Linq;
    using CommandLine;
    using Maxstupo.SsaUtil.Subtitles;
    using Maxstupo.SsaUtil.Utility;

    public sealed class Program {

        private IOutput Output { get; } = new ColorConsole();

        private void Init(BaseOptions options) {



        }

        private int RunEdit(EditOptions options) {

            return 0;
        }

        private int RunInfo(InfoOptions options) {

            SsaReader reader = new SsaReader();

            SsaSubtitle subtitle = reader.ReadFrom(@"example-subs.ass");

            if (reader.HasErrors) {
                foreach (SsaError error in reader)
                    Output.WriteLine(Level.Error, string.Format(error.Message, error.Args.Select(x => $"&-e;{x}&-^;").ToArray()));

                return -1;
            }

            Output.WriteLine(Level.Info, $"Title: {subtitle.Title}");
            Output.WriteLine(Level.Info, $"# of Styles: {subtitle.Styles.Count}");

            return 0;
        }

        private static int Main(string[] args) {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                args = "info".Split(' ');
#endif

            Program program = new Program();

            return OnReturnCode(Parser.Default.ParseArguments<InfoOptions, EditOptions>(args)
                .MapResult(
                    (InfoOptions options) => { program.Init(options); return program.RunInfo(options); },
                    (EditOptions options) => { program.Init(options); return program.RunEdit(options); },
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