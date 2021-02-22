using System;
using System.Collections.Generic;
using TRoschinsky.SPDataModel.Lib;
using TRoschinsky.SPDataModel.Lib.Data;
using TRoschinsky.SPDataModel.Lib.ModelGenerators;

namespace TRoschinsky.SPDataModel.cmd
{
    public class Interactive
    {
        private const string headlineBase = "=== SharePoint Data Model - Interactive Console ===\nCommands in this context:";
        private const string horizontalRuler = "----------------------------------";
        private List<Model> models = new List<Model>();
        private string prompt = String.Empty;
        private string promptStatus = String.Empty;
        private int selectedModel = 0;
        private ConsoleKey key = new ConsoleKey();


        public Interactive()
        {
            Run();
        }

        private void Run()
        {
            PrintInteractiveMenu();
            while (IsNotEscape(key))
            {
                switch (key)
                {
                    // Create model
                    case ConsoleKey.C:
                        while (IsNotEscape(key))
                        {
                            PrintInteractiveMenuCreate();
                            key = Console.ReadKey().Key;
                        }
                        break;

                    // Add model
                    case ConsoleKey.A:
                        while (IsNotEscape(key))
                        {
                            Model sampleModel = null;
                            switch (key)
                            {
                                case ConsoleKey.NumPad1:
                                case ConsoleKey.D1:
                                    sampleModel = SampleModels.GetSampleModel(SampleModels.SampleModelType.MeasureTracking);
                                    models.Add(sampleModel);
                                    break;
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D2:
                                    sampleModel = SampleModels.GetSampleModel(SampleModels.SampleModelType.OrderProcess);
                                    models.Add(sampleModel);
                                    break;
                                default:
                                    promptStatus = String.Empty;
                                    break;
                            }

                            if (sampleModel != null)
                            {
                                promptStatus = String.Format("Added sample model '{0}'...", String.IsNullOrEmpty(sampleModel.Name) ? sampleModel.GetType().Name : sampleModel.Name);
                            }

                            PrintInteractiveMenuAdd();
                            key = Console.ReadKey().Key;
                        }
                        break;

                    // List models
                    case ConsoleKey.L:
                        promptStatus = models.Count == 0 ? "** No models loaded... **" : "** Currently loaded models... **\n";
                        foreach (Model model in models)
                        {
                            bool isSelected = model == models[selectedModel];
                            promptStatus += String.Format("[{0}] {1}\n", isSelected ? "X" : " ", model);
                        }
                        break;

                    // Output models
                    case ConsoleKey.O:
                        if (!(models.Count > 0))
                        {
                            promptStatus = "** Fist choose A or C to add a model! **";
                            break;
                        }
                        while (IsNotEscape(key))
                        {
                            switch (key)
                            {
                                case ConsoleKey.M:
                                    if (models.Count > 0)
                                    {
                                        bool wasChanged = false;
                                        Console.WriteLine();
                                        while (!wasChanged && IsNotEscape(key))
                                        {
                                            Console.Write("Select model index from [0 to {0}]: ", models.Count - 1);
                                            ConsoleKeyInfo keyInfo = Console.ReadKey();
                                            Console.WriteLine();
                                            key = keyInfo.Key;
                                            if (char.IsNumber(keyInfo.KeyChar))
                                            {
                                                selectedModel = int.Parse(keyInfo.KeyChar.ToString());
                                                if (selectedModel < models.Count)
                                                {
                                                    wasChanged = true;
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case ConsoleKey.NumPad0:
                                case ConsoleKey.D0:
                                    OutputModel(new ConsoleSimple(models[selectedModel]));
                                    break;
                                case ConsoleKey.NumPad1:
                                case ConsoleKey.D1:
                                    OutputModel(new DrawioCsvDiagram(models[selectedModel]));
                                    break;
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D2:
                                    OutputModel(new DrawioTextDiagram(models[selectedModel]));
                                    break;
                                case ConsoleKey.NumPad3:
                                case ConsoleKey.D3:
                                    OutputModel(new DrawioTextList(models[selectedModel]));
                                    break;
                                case ConsoleKey.NumPad4:
                                case ConsoleKey.D4:
                                    OutputModel(new SpJsomInstaller(models[selectedModel]));
                                    break;
                                default:
                                    promptStatus = String.Empty;
                                    break;
                            }
                            PrintInteractiveMenuOutput();
                            key = Console.ReadKey().Key;
                        }
                        break;

                    default:
                        break;
                }
                PrintInteractiveMenu();
                key = Console.ReadKey().Key;
                promptStatus = String.Empty;
            }
        }

        private void OutputModel(ModelGenerator generator)
        {
            try
            {
                promptStatus = String.Format("** Generator '{0}' output... **\n", String.IsNullOrEmpty(generator.Name) ? generator.GetType().Name : generator.Name);
                promptStatus += generator.Output;
            }
            catch (System.Exception ex)
            {
                promptStatus = String.Format("** Generator output failed due to: {0} **", ex.Message);
            }
        }

        #region Helper UI

        private void PrintInteractiveMenu()
        {
            string prompt = "spdatamodel>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine(horizontalRuler);
            Console.WriteLine("A: Add existing data models");
            Console.WriteLine("C: Create new data models from scratch");
            Console.WriteLine("L: List cached data models (selected #{0} '{1}')", selectedModel, models.Count > selectedModel ? models[selectedModel].Name : "<none>");
            Console.WriteLine("O: Output cached data model");
            Console.WriteLine("Q: Quit");
            Console.WriteLine(horizontalRuler);
            if (!String.IsNullOrEmpty(promptStatus))
            {
                Console.WriteLine(promptStatus);
                Console.WriteLine(horizontalRuler);
            }
            Console.Write(prompt);
        }

        private void PrintInteractiveMenuAdd()
        {
            string prompt = "spdatamodel add>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine(horizontalRuler);
            Console.WriteLine("1: Sample 'Survey Measures'");
            Console.WriteLine("2: Sample 'Order Process'");
            Console.WriteLine("Q: Quit context");
            Console.WriteLine(horizontalRuler);
            if (!String.IsNullOrEmpty(promptStatus))
            {
                Console.WriteLine(promptStatus);
                Console.WriteLine(horizontalRuler);
                promptStatus = String.Empty;
            }
            Console.Write(prompt);
        }

        private void PrintInteractiveMenuCreate()
        {
            string prompt = "spdatamodel create>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine("*** not implemented, use Q for exit context ***");
            Console.Write(prompt);
        }

        private void PrintInteractiveMenuOutput()
        {
            string prompt = "spdatamodel output>";
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headlineBase);
            Console.WriteLine("M: Set currently selected model for output");
            Console.WriteLine("0: Just show me the model...");
            Console.WriteLine("1: Output type = DrawioCsvDiagram");
            Console.WriteLine("2: Output type = DrawioTextDiagram");
            Console.WriteLine("3: Output type = DrawioTextList");
            Console.WriteLine("4: Output type = SpJsomInstaller");
            Console.WriteLine("Q: Quit context");
            Console.WriteLine(horizontalRuler);
            if (!String.IsNullOrEmpty(promptStatus))
            {
                Console.WriteLine(promptStatus);
                Console.WriteLine(horizontalRuler);
                promptStatus = String.Empty;
            }
            Console.Write(prompt);
        }

        private bool IsNotEscape(ConsoleKey pressedKey)
        {
            return pressedKey != ConsoleKey.Q && pressedKey != ConsoleKey.Backspace && pressedKey != ConsoleKey.Escape;
        }

        #endregion

    }
}