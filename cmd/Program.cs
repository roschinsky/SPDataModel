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
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void RunInteractiveSession()
        {
            List<Model> models = new List<Model>();
            string headlineBase = "---| 'SharePoint Data Model' Interactive Console |- {0} ---";
            string breadcrumb = String.Empty;
            int selectedModel = 0;

            ConsoleKey key = new ConsoleKey();
            while (key != ConsoleKey.Q)
            {
                breadcrumb = "/..........";
                Console.Clear();
                Console.WriteLine(headlineBase, breadcrumb);
                switch (key)
                {
                    // Create model
                    case ConsoleKey.C:
                        Console.WriteLine("*** not implemented ***");
                        break;

                    // Create model
                    case ConsoleKey.A:
                        while (key != ConsoleKey.Q)
                        {
                            breadcrumb = "/Add.......";
                            Console.Clear();
                            Console.WriteLine(headlineBase, breadcrumb);
                            switch(key)
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
                            Console.WriteLine("Choose an option to proceed [sample1, sample2, Quit]: ");
                            key = Console.ReadKey().Key;
                        }
                        break;

                    // List models
                    case ConsoleKey.L:
                        foreach(Model model in models)
                        {
                            Console.WriteLine("\t- {0}", model);
                        }
                        break;


                    // Output models
                    case ConsoleKey.O:
                        while (key != ConsoleKey.Q)
                        {
                            breadcrumb = "/Output....";
                            Console.Clear();
                            Console.WriteLine(headlineBase, breadcrumb);
                            switch(key)
                            {
                                case ConsoleKey.M:
                                    if(models.Count > 0)
                                    {
                                        char inputChar = '?';
                                        while(!Char.IsNumber(inputChar) && int.Parse(inputChar.ToString()) >= models.Count)
                                        {
                                            Console.WriteLine("Select model index from [0 to {0}]: ", models.Count -1);
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
                            Console.WriteLine("Choose an option to proceed [Model, 1=DrawioCsvDiagram, 2=DrawioTextDiagram, 3=DrawioTextList, Quit]: ");
                            key = Console.ReadKey().Key;
                        }
                        
                        break;

                    default:
                        break;
                }
                Console.WriteLine("Choose an option to proceed [Add, Create, List, Output, Quit]: ");
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
                switch(generatorType.GetType().Name)
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
    }
}
