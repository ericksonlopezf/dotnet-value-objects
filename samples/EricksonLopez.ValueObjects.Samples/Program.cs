// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.ValueObjects.Samples.Levels;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(@"
███████╗██████╗ ██╗ ██████╗██╗  ██╗███████╗ ██████╗ ███╗   ██╗
██╔════╝██╔══██╗██║██╔════╝██║ ██╔╝██╔════╝██╔═══██╗████╗  ██║
█████╗  ██████╔╝██║██║     █████═╝ ███████╗██║   ██║██╔██╗ ██║
██╔══╝  ██╔══██╗██║██║     ██╔═██╗ ╚════██║██║   ██║██║╚██╗██║
███████╗██║  ██║██║╚██████╗██║ ╚██╗███████║╚██████╔╝██║ ╚████║
╚══════╝╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═══╝
   ERICKSONLOPEZ.VALUEOBJECTS - OFFICIAL SHOWCASE & REFERENCE RUNTIME
");
Console.ResetColor();

Console.WriteLine("Executing progressive learning showcase (Levels 0 through 10)...\n");

Level00_Conceptual.Run();
Level01_QuickStart.Run();
Level02_ConfigurationAndPipelines.Run();
Level03_RealWorldUseCases.Run();
Level04_MultiCountryFiscalDomains.Run();
Level05_HighThroughputProcessing.Run();
Level06_ErrorHandlingAndValidation.Run();
Level07_ZeroAllocationAot.Run();
Level08_CustomValueObjects.Run();
Level09_PersistenceAndSerialization.Run();
Level10_EnterpriseDddPatterns.Run();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n===============================================================================");
Console.WriteLine(" ✔ ALL SHOWCASE LEVELS (0 THROUGH 10) EXECUTED SUCCESSFULLY.");
Console.WriteLine("===============================================================================");
Console.ResetColor();
