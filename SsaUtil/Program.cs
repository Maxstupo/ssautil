namespace Maxstupo.SsaUtil {

    using System;
    using CommandLine;
    using Maxstupo.SsaUtil.Utility;

    public sealed class Program {

        private IOutput Output { get; } = new ColorConsole();

        private void Init(BaseOptions options) {
            Output.WriteLine(Level.Info, "Hello World");
            Output.WriteLine(Level.Warn, "Hello World");
            Output.WriteLine(Level.Severe, "Hello World");
            Output.WriteLine(Level.Fine, "Hello World");
        }

        private int RunEdit(EditOptions options) {

            return 0;
        }

        private int RunInfo(InfoOptions options) {

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