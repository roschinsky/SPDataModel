using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using TRoschinsky.SPDataModel.Lib.ModelGenerators;

namespace TRoschinsky.SPDataModel.cmd
{
    class Program
    {
        static int Main(string[] args)
        {
            RootCommand rootCommand = new RootCommand
            {
                new Option<bool>(
                    new string[] {"--interactive","-i"},
                    getDefaultValue: () => false,
                    description: "Interactive mode to use a commandline menu."),
                new Option<bool>(
                    new string[] {"--list","-l"},
                    getDefaultValue: () => false,
                    description: "List available types of conversions."),
                new Option<string>(
                    new string[] {"--inputType", "-it"},
                    getDefaultValue: () => "",
                    description: "Type of generator used for output."),
                new Option<string>(
                    new string[] {"--outputType", "-ot"},
                    getDefaultValue: () => typeof(ConsoleSimple).Name,
                    description: "Type of generator used for output.")
                    /*,
                new Argument<string>("input",
                    description: "Source of model. Can be a single file or a folder."
                ),
                new Argument<string>("output",
                    description: "Target for output - depending of the outputType it'll be a file or folder if given."
                )*/
            };

            rootCommand.Description = "'SharePoint Data Model' converts relational data structures from one representation to another.";
            rootCommand.TreatUnmatchedTokensAsErrors = true;
            //rootCommand.Handler = CommandHandler.Create<bool, bool, string, string>(Run(interactive, list, ));
            //return await rootCommand.InvokeAsync(args);

            rootCommand.Handler = CommandHandler.Create<bool, bool, string, string, string, string>((interactive, list, inputType, outputType, input, output) =>
            {
                try
                {
                    if (!interactive)
                    {
                        rootCommand.Invoke("-h");
                        Console.WriteLine($"The value for --interactive is: {interactive}");
                        Console.WriteLine($"The value for --list is: {list}");
                        Console.WriteLine($"The value for --inputType is: {inputType}");
                        //Console.WriteLine($"The value for --input is: {input?.FullName ?? "null"}");
                        Console.WriteLine($"The value for --input is: {input}");
                        Console.WriteLine($"The value for --outputType is: {outputType}");
                        Console.WriteLine($"The value for --output is: {output}");
                    }
                    else
                    {
                        Interactive interactiveMenu = new Interactive();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: {0}.", ex.Message);
                }
            });

            return rootCommand.InvokeAsync(args).Result;
        }

    }
}
