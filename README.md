# ssautil

<p float="left" align="left" width="100%">
 <img src="https://img.shields.io/github/license/Maxstupo/ssautil.svg" />
 <img src="https://img.shields.io/github/release/Maxstupo/ssautil.svg" />
</p>

ssautil allows for the bulk editing of SubStation Alpha subtitles.

## Typical Usage

Bulk edit the fontsize and vertical margin for all styles called `main` and `italics` for all files located in `<dir>` and output modified copies to `./modified`<br/>
`ssa edit -i "<dir>" -s --set Fontsize=50 MarginV=40 --filter "Name=main|italics" -o "./modified"`

<br/>
Use `ssa edit --help` for all available options.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments
* [Commandline](https://github.com/commandlineparser/commandline)
* [Fody](https://github.com/Fody/Fody/) & [Fody.Costura](https://github.com/Fody/Costura)
