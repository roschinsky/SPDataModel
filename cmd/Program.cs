using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using TRoschinsky.SPDataModel.Lib;
using TRoschinsky.SPDataModel.Lib.Data;
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
                        RunInteractiveSession();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: {0}.", ex.Message);
                }
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void RunInteractiveSession()
        {
            List<Model> models = new List<Model>();
            string lb = Environment.NewLine;
            string headlineBase = "Commands in this context:";
            string prompt = String.Empty;
            int selectedModel = 0;

            ConsoleKey key = new ConsoleKey();
            while (key != ConsoleKey.Q && key != ConsoleKey.Backspace)
            {
                PrintInteractiveMenu(headlineBase, selectedModel, models);
                switch (key)
                {
                    // Create model
                    case ConsoleKey.C:
                        while (key != ConsoleKey.Q && key != ConsoleKey.Backspace)
                        {
                            PrintInteractiveMenuCreate(headlineBase);
                            key = Console.ReadKey().Key;
                        }
                        break;

                    // Create model
                    case ConsoleKey.A:
                        while (key != ConsoleKey.Q && key != ConsoleKey.Backspace)
                        {
                            PrintInteractiveMenuAdd(headlineBase);
                            switch (key)
                            {
                                case ConsoleKey.NumPad1:
                                case ConsoleKey.D1:
                                    models.Add(SampleModels.GetSampleModel(SampleModels.SampleModelType.MeasureTracking));
                                    break;
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D2:
                                    models.Add(SampleModels.GetSampleModel(SampleModels.SampleModelType.OrderProcess));
                                    break;
                            }
                            key = Console.ReadKey().Key;
                        }
                        break;

                    // List models
                    case ConsoleKey.L:
                        Console.WriteLine();
                        Console.WriteLine("--------------------------------------");
                        foreach (Model model in models)
                        {
                            Console.WriteLine("\t- {0}", model.ToString());
                        }
                        Console.ReadKey();
                        break;

                    // Output models
                    case ConsoleKey.O:
                        while (key != ConsoleKey.Q && key != ConsoleKey.Backspace)
                        {
                            PrintInteractiveMenuOutput(headlineBase);
                            switch (key)
                            {
                                case ConsoleKey.M:
                                    if (models.Count > 0)
                                    {
                                        char inputChar = '?';
                                        while (!Char.IsNumber(inputChar) && int.Parse(inputChar.ToString()) >= models.Count)
                                        {
                                            Console.WriteLine("Select model index from [0 to {0}]: ", models.Count - 1);
                                            selectedModel = int.Parse(Console.ReadKey().KeyChar.ToString());
                                        }
                                    }
                                    break;

                                case ConsoleKey.NumPad1:
                                case ConsoleKey.D1:
                                    OutputModel(models[selectedModel], typeof(DrawioCsvDiagram));
                                    break;
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D2:
                                    OutputModel(models[selectedModel], typeof(DrawioTextDiagram));
                                    break;
                                case ConsoleKey.NumPad3:
                                case ConsoleKey.D3:
                                    OutputModel(models[selectedModel], typeof(DrawioTextList));
                                    break;
                            }
                            key = Console.ReadKey().Key;
                        }
                        break;

                    default:
                        break;
                }
                PrintInteractiveMenu(headlineBase, selectedModel, models);
                key = Console.ReadKey().Key;
            }
        }

        private static T CreateGenerator<T>(Model model) where T : new()
        {
            return new T();
        }

        private static void OutputModel(Model model, Type generatorType)
        {
            try
            {
                ModelGenerator generator = null;
                switch (generatorType.GetType().Name)
                {
                    case "DrawioCsvDiagram":
                        generator = new DrawioCsvDiagram(model, "Test");
                        break;
                    case "DrawioTextDiagram":
                        generator = new DrawioTextDiagram(model, "Test");
                        break;
                    case "DrawioTextList":
                        generator = new DrawioTextList(model, "Test");
                        break;
                }

                Console.WriteLine("----------| {0}", generator);
                Console.WriteLine(generator.Output);
                Console.WriteLine("-----------------------------------------");

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Output failed due to: {0}", ex.Message);
            }
        }

        #region helper UI

        private static void PrintInteractiveMenu(string headlineBase, int selectedModel, List<Model> models)
        {
            string prompt = "spdatamodel>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine("A: Add existing data models");
            Console.WriteLine("C: Create new data models from scratch");
            Console.WriteLine("L: List cached data models (selected #{0} '{1}')", selectedModel, models.Count > selectedModel ? models[selectedModel].Name : "<none>");
            Console.WriteLine("O: Output cached data model");
            Console.WriteLine("Q: Quit");
            Console.WriteLine();
            Console.Write(prompt);
        }

        private static void PrintInteractiveMenuAdd(string headlineBase)
        {
            string prompt = "spdatamodel add>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine("1: Sample 'Survey Measures'");
            Console.WriteLine("2: Sample 'Order Process'");
            Console.WriteLine("Q: Quit context");
            Console.WriteLine();
            Console.Write(prompt);
        }

        private static void PrintInteractiveMenuCreate(string headlineBase)
        {
            string prompt = "spdatamodel create>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine("*** not implemented, use Q for exit context ***");
            Console.Write(prompt);
        }

        private static void PrintInteractiveMenuOutput(string headlineBase)
        {
            string prompt = "spdatamodel output>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine("M: Choose model for output");
            Console.WriteLine("1: Output type = DrawioCsvDiagram");
            Console.WriteLine("2: Output type = DrawioTextDiagram");
            Console.WriteLine("3: Output type = DrawioTextList");
            Console.WriteLine("Q: Quit context");
            Console.WriteLine();
            Console.Write(prompt);
        }

        #endregion
    }
}
