﻿using System.Text;
using OSPSuite.Core.Domain;

namespace MoBi.Assets
{
   public static class ToolTips
   {
      public static readonly string ZoomIn = "Magnifies the view of the Flow Chart";
      public static readonly string ZoomOut = "Reduces the view of the Flow Chart";
      public static readonly string FitToPage = "Zoom to fit all elements in the current Flow Chart window ";
      public static readonly string SetToDefault = "Sets Values to Defaults defined in Molecules and Spatial Structure";
      public static readonly string Extend = "Adds new start values from Molecules and Spatial Structure";
      public static readonly string AddMoleculeNameToList = "Add molecule name to selectable list";
      public static readonly string AddToProject = "Save as a new building block in project.";
      public static readonly string ResetParameterToolTip = "Reset parameter to default";
      public static readonly string SaveSimulationSettingsToolTip = "Save current simulations settings as template into project or user profile. These settings will be used as default settings for each newly created simulation.";

      public static class BuildingBlockMolecule
      {
         public static readonly string NewMolecule = "Create a new Molecule";
         public static readonly string LoadMolecule = $"Load Molecule from Molecules Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
         public static readonly string AddPKSimMolecule = "Add a PK-Sim Molecule";
      }

      public static class BuildingBlockReaction
      {
         public static readonly string NewReaction = "Create a new Reaction";
         public static readonly string LoadReaction = $"Load Reaction from Reactions Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
         public static readonly string NewMolecule = "Create a new Molecule";
      }

      public static class BuildingBlockSpatialStructure
      {
         public static readonly string NewTopContainer = "Create a new Top Container";
         public static readonly string LoadTopContainer = $"Load Top Container from Spatial Structures Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
      }

      public static class BuildingBlockPassiveTransport
      {
         public static readonly string NewTransport = "Create a new Passive Transport\n Active transporters are located below the molecule to be transported and created in Molecules Building Block";
         public static readonly string LoadTransport = $"Load Passive Transport from Passive Transports Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
      }

      public static class BuildingBlockObserver
      {
         public static readonly string NewObserver = "Create a new Observer";
         public static readonly string LoadObserver = $"Load Observer from Observers Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";

         public static readonly string NewAmountObs = "Create a new Molecule Observer";
         public static readonly string NewContainerObs = "Create a new Container Observer";

         public static readonly string LoadAmountObs = $"Load Molecule Observer from Observers Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
         public static readonly string LoadContainerObs = $"Load Container Observer from Observers Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
      }

      public static class BuildingBlockEventGroup
      {
         public static readonly string NewEventGroup = "Create a new Event Group";
         public static readonly string LoadEventGroup = $"Load Event Group from Event Groups Building Block file (*{Constants.Filter.PKML_FILE_FILTER})";
      }

      public static class ModellingRibbon
      {
         public static readonly string CreateMoleculesBB = "Create a new Molecules Building Block";
         public static readonly string CreateReactionsBB = "Create a new Reactions Building Block";
         public static readonly string CreateSpatStructuresBB = "Create a new Spatial Structures Building Block";
         public static readonly string CreatePassiveTansportsBB = "Create a new Passive Transports Building Block";
         public static readonly string CreateEventGroupsBB = "Create a new Event Groups Building Block";
         public static readonly string CreateObserversBB = "Create a new Observers Building Block";
         public static readonly string CreateSimulationSettingsBB = "Create a new Simulation Settings Building Block";
      }

      public static class EditRibbon
      {
         public static readonly string UndoDescription = "Undo the last action";
      }

      public static class FileRibbon
      {
         public static readonly string AboutThisApplication = "About this application...";
         public static readonly string NewProjectDescription = "Create a new project...";
         public static readonly string NewAmountProjectDescription = "Reactions compute amount (µmol) changes";
         public static readonly string NewConcentrationProjectDescription = "Reactions compute concentration (µmol/l) changes";
         public static readonly string OpenProjectDescription = "Open an existing project...";
         public static readonly string ProjectDescriptionDescription = "Show or edit project description...";
         public static readonly string CloseProjectDescription = "Close the project";
         public static readonly string SaveProjectDescription = "Save the project...";
         public static readonly string SaveProjectAsDescription = "Save the project to a new file...";
         public static readonly string ExitDescription = "Exit the application";
         public static readonly string OpenSimulationDescription = "Open an existing simulation...";
         public static readonly string ImportSBMLDescription = "Import an existing SBML model...";
      }

      public static class SimulationRibbon
      {
         public static readonly string CreateSimulation = "Create a new simulation from existing building blocks";
         public static readonly string RunSimulation = "Simulate the selected simulation";
         public static readonly string RunWithSettingsDescription = "Select simulation settings and simulate the active simulation";
         public static readonly string StopSimulation = "Stop the currently running simulation";
         public static readonly string CalculateScaleFactors = "Calculate scale factors";
         public static readonly string ConfigureSimulationDescription = "Configure simulation";
      }

      public static class ImportRibbon
      {
         public static readonly string LoadSimulation = $"Load an existing Simulation (*{Constants.Filter.PKML_FILE_FILTER}) into Project";
         public static readonly string AddObservedData = "Import Observed Data into Project";
         public static readonly string LoadObservedData = "Load Observed Data into Project";
      }

      public static class ExportRibbon
      {
         public static readonly string CreateReport = "Export History to Excel file";
         public static readonly string ExportToPDF = "Create a PDF report of the project";
         public static readonly string ExportHistoryToPDF = "Create a PDF report of the project history";
      }

      public static class ViewRibbon
      {
         public static readonly string ViewBBs = "Show or hide the building blocks explorer";
         public static readonly string ViewSims = "Show or hide the simulations explorer";
         public static readonly string ViewsHistoryManager = "Show or hide the history";
         public static readonly string ViewSearch = "Show or hide the search window";
         public static readonly string ViewNotification = "Show or hide the notifications window";
         public static readonly string ViewComparison = "Show or hide the comparison window";
      }

      public static class ExtrasRibbon
      {
         public static readonly string ConvertMoBi2Project = "Creates Molecules and Reactions Building blocks from a MoBi 2 project. Also, a Spatial Structure that is not intended for simulation use is created. Regroup parameters manually after import.";
         public static readonly string Options = "Change general settings and diagram options";
      }

      public class WorkFlowRibbon
      {
         public static readonly string Merge = $"Merge an existing Simulation (*{Constants.Filter.PKML_FILE_FILTER}) into Project";
      }

      public static readonly string Description = "Free text for background information on the here defined object";
      public static string TransporterName = "Name used in Simulation";
      public static string MultipleParameterChangesView = "More then on value found for a parameter changed in simulation. Select one to commit to builder in building block";

      public static class ParameterView
      {
         public static readonly string ParameterName = "Define a name for the Parameter";
         public static readonly string ParameterType = "Specify the parameters area of validity \n \n Local: should only be used locally, i.e., within the corresponding reaction or for a molecule where a local parameter is defined \n \n Global:can also be used in other formulas  \n \n Property: are identical to Global parameters except that they will not be listed and set in the parameter start values";
         public static readonly string ParameterDimension = "";
         public static readonly string IsStateVariable = "Parameter (P) is defined and calculated by solving a differential equation as: \n \n P: “dP/dt = Right Hand Side”; \n \n The selected formula defines the Right Hand Side. The Value defined above will be used as initial condition";
         public static readonly string Persistable = "Parameter values plotable";
      }

      public static class ParameterList
      {
         public static readonly string DeleteParameter = "Delete Parameter from list";
         public static readonly string SetParameterValue = "Set a new parameter value \n \n Formulas, even Constants are overruled but not overwritten";
         public static readonly string NewParameterName = "Define a new name for the Parameter";
         public static readonly string MoveUp = "Move Up";
         public static readonly string MoveDown = "Move Down";
      }

      public static class FormulaList
      {
         public static readonly string DeleteFormula = "Delete formula from list";
      }

      public static class Formula
      {
         public static string FormulaType
         {
            get
            {
               var sb = new StringBuilder("Specify the type of formula to calculate the value");
               sb.AppendLine();
               sb.AppendLine();
               sb.AppendLine("<b>Constant</b>: a single numeric value");
               sb.AppendLine();
               sb.AppendLine("<b>Formula</b>: an explicit formula");
               sb.AppendLine();
               sb.AppendLine("<b>Table</b>: multiple time discrete and piecewise constant numeric values");
               sb.AppendLine();
               sb.AppendLine("<b>Table formula with offset</b>: table formula shifted by a constant time value");
               sb.AppendLine();
               sb.AppendLine("<b>Sum formula</b>: sum of a named parameter matching tag criteria");
               return sb.ToString();
            }
         }

         public static readonly string FormulaName = "Define a name for the Formula";
         public static readonly string ConstantValue = "Set a numeric value";
         public static readonly string X = "Point in time";
         public static readonly string Y = "Formula value at t=X";
         public static readonly string RestartSolver = "Select to mark solver restart at t=X. Increases numerical accuracy  but may reduce solver speed";
         public static readonly string AddPoint = "Add a new point to the list";
         public static readonly string DeletePoint = "Delete the point from the list";
         public static readonly string ReferenceAlias = "Sort by Alias \n \n Alias is the identifier of the reference for use in the formula";
         public static readonly string ReferencePath = "Sort by Path \n \n The Path describes where the referenced object is located";
         public static readonly string ReferenceDimension = "Sort by Dimension \n \n Dimension defines the class of unit of measurement of the referenced object";
         public static readonly string UseDerivedValues = "If checked, the first derivative of the entered table values is used";
      }

      public static class ReferenceSelector
      {
         public static readonly string References = "List of possible references to be selected for use in the formula";
         public static readonly string RelativePath = "Relative Path: Displays the path of the selected reference relative to the Local Reference Point ";
         public static readonly string AbsolutePath = "Absolute Path: Displays the full path of the selected reference";
         public static readonly string SetLocalReferencePoint = "Choose a Local Reference Point for relative paths";
      }

      public static class Molecules
      {
         public static readonly string IsStationary = "If selected, the molecule is not subjected to active or passive transports";
         public static readonly string MoleculeName = "Change the name of the molecule";
         public static readonly string DistributionCellular = "Specify calculation method for vascular-intracellular space partition coefficient";
         public static readonly string DiffusionIntCell = "  Specify calculation method for diffusion between interstitial and intracellular space ";
         public static readonly string DistributionInterstitial = "  Specify calculation method for interstitial-intracellular space partition coefficient";
         public static readonly string MoleculeType = "Molecule Type for informational use";
      }

      public static class Observer
      {
         public static string SelectAll(string builderType)
         {
            return $"Define {builderType} for all existing molecules (except those listed in exclude list)";
         }

         public static readonly string ObserverName = "Change the name of the observer";
         public static readonly string AddMoleculeToIncludeList = "Add a molecule to the list of included molecules";
         public static readonly string AddMoleculeToExcludeList = "Add a molecule to the list of excluded molecules";
         public static readonly string DeleteMolecule = "Delete molecule from list of molecules ";
      }

      public static class Applications
      {
         public static readonly string ApplicationName = "Define a name for the Application";
         public static readonly string ApplicatedMolecule = "Select the molecule which is to be applicatedwhich the application will affect";
         public static readonly string ApplicationMoleculeBuilderr = "Define the source amount of the application";
         public static readonly string ApplicationMoleculeBuilderPath = "Path to container there application source is created";
         public static readonly string ApplicationMoleculeBuilderFormula = "Formula of application source amount";
      }

      public static class Container
      {
         public static readonly string ContainerName = "Define a name for the Container";
         public static readonly string ContainerType = " Specify the type of Container ";
         public static readonly string PhysicalContainer = "Container may contain molecules in the simulation";
         public static readonly string LogicalContainer = "Container groups other containers and parameters, cannot contain molecules";
         public static readonly string AddTag = "Add a Tag to the Container";
      }

      public static class SolverOptions
      {
         public static readonly string AbsTol = "Max. absolute error during solving ode";
         public static readonly string RelTol = "Max. relative error during solving ode";
         public static readonly string H0 = "Initial step size";
         public static readonly string HMin = "Minimum step size";
         public static readonly string HMax = "Maximum step size";
         public static readonly string MxStep = "Maximum number of solver steps";
         public static readonly string UseJacobian = "Use jacobian matrix for solving ode";
      }

      public static class DisplayUnits
      {
         public static string AddUnitMap = "Add new default unit for a specific dimension";
         public static string LoadUnits = "Load default units from file";
         public static string SaveUnits = "Save default units to file";
      }
   }
}