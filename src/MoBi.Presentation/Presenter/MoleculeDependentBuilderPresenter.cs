﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeDependentBuilderPresenter : IPresenter<IMoleculeDependentBuilderView>, IEditPresenter<IMoleculeDependentBuilder>
   {
      void RemoveFromIncludeList(string molecule);
      void RemoveFromExcludeList(string molecule);
      void AddToIncludeList();
      void AddToExcludeList();
      void SetForAll(bool forAll);
      IBuildingBlock BuildingBlock { set; get; }
   }

   public class MoleculeDependentBuilderPresenter : AbstractEditPresenter<IMoleculeDependentBuilderView, IMoleculeDependentBuilderPresenter, IMoleculeDependentBuilder>, IMoleculeDependentBuilderPresenter
   {
      private readonly IMoBiContext _context;
      private IMoleculeDependentBuilder _moleculeDependentBuilder;
      private readonly IDialogCreator _dialogCreator;
      public IBuildingBlock BuildingBlock { get; set; }

      public MoleculeDependentBuilderPresenter(IMoleculeDependentBuilderView view, IMoBiContext context, IDialogCreator dialogCreator) : base(view)
      {
         _context = context;
         _dialogCreator = dialogCreator;
      }

      public override void Edit(IMoleculeDependentBuilder objectToEdit)
      {
         _moleculeDependentBuilder = objectToEdit;
         _view.BuilderType = new ObjectTypeResolver().TypeFor(objectToEdit);
         refreshView();
      }

      private void refreshView()
      {
         _view.BindTo(_moleculeDependentBuilder.MoleculeList);
      }

      public override object Subject
      {
         get { return _moleculeDependentBuilder; }
      }

      public void RemoveFromIncludeList(string molecule)
      {
         removeMolecule(_moleculeDependentBuilder.MoleculeNames(), molecule, () => new RemoveMoleculeNameFromIncludeCommand(_moleculeDependentBuilder, molecule, BuildingBlock));
      }

      public void RemoveFromExcludeList(string molecule)
      {
         removeMolecule(_moleculeDependentBuilder.MoleculeNamesToExclude(), molecule, () => new RemoveMoleculeNameFromExcludeCommand(_moleculeDependentBuilder, molecule, BuildingBlock));
      }

      private void removeMolecule(IEnumerable<string> availables, string molecule, Func<RemoveMoleculeNameCommand> removeItemCommand)
      {
         if (!availables.ContainsItem(molecule)) return;
         AddCommand(removeItemCommand().Run(_context));
      }

      public void AddToIncludeList()
      {
         addMolecule(_moleculeDependentBuilder.MoleculeNames(), moleculeName => new AddMoleculeNameToIncludeCommand(_moleculeDependentBuilder, moleculeName, BuildingBlock));
      }

      public void AddToExcludeList()
      {
         addMolecule(_moleculeDependentBuilder.MoleculeNamesToExclude(), moleculeName => new AddMoleculeNameToExcludeCommand(_moleculeDependentBuilder, moleculeName, BuildingBlock));
      }

      private string newMoleculeName()
      {
         return _dialogCreator.AskForInput(AppConstants.Dialog.GetReactionMoleculeName,
            AppConstants.Captions.AddReactionMolecule,
            string.Empty, Enumerable.Empty<string>(), getMoleculeNames());
      }

      private void addMolecule(IEnumerable<string> availables, Func<string, AddMoleculeNameCommand> addItemCommand)
      {
         var moleculeName = newMoleculeName();
         if (string.IsNullOrEmpty(moleculeName)) return;
         if (availables.ContainsItem(moleculeName)) return;
         AddCommand(addItemCommand(moleculeName).Run(_context));
      }

      public void SetForAll(bool forAll)
      {
         AddCommand(new SetForAllCommand(_moleculeDependentBuilder, forAll, BuildingBlock).Run(_context));
      }

      public override void AddCommand(ICommand command)
      {
         base.AddCommand(command);
         refreshView();
      }

      private IEnumerable<string> getMoleculeNames()
      {
         var moleculeBB = _context.CurrentProject.MoleculeBlockCollection;
         var moleculeNames = new HashSet<string>();
         moleculeBB.SelectMany(x => x).Each(molecule => moleculeNames.Add(molecule.Name));
         return moleculeNames.OrderBy(x => x);
      }
   }
}