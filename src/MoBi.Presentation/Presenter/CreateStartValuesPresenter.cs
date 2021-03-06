using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateStartValuesPresenter : IDisposablePresenter
   {
      IEnumerable<IMoleculeBuildingBlock> GetMolecules();
      IEnumerable<IMoBiSpatialStructure> GetSpatialStructures();
   }

   public interface ICreateStartValuesPresenter<T> : ICreateStartValuesPresenter
   {
      T Create();
   }

   public abstract class CreateStartValuesPresenter<T> : AbstractDisposablePresenter<ICreateStartValuesView, ICreateStartValuesPresenter>, ICreateStartValuesPresenter<T> where T : class
   {
      protected readonly IMoBiContext _context;
      protected readonly List<string> _unallowedNames;

      protected CreateStartValuesPresenter(ICreateStartValuesView view, IMoBiContext context) : base(view)
      {
         _context = context;
         _unallowedNames = new List<string>();
      }

      public T Create()
      {
         var dto = createDto(String.Empty, GetMolecules().First(), GetSpatialStructures().First());
         _view.Show(dto);
         _view.Display();
         if (_view.Canceled) return null;
         return CreateStartValuesFromDTO(dto);
      }

      protected abstract T CreateStartValuesFromDTO(StartValuesDTO dto);

      private StartValuesDTO createDto(string name, IMoleculeBuildingBlock moleculeBuildingBlock, IMoBiSpatialStructure spatialStructure)
      {
         var dto = new StartValuesDTO {Name = name, Molecules = moleculeBuildingBlock, SpatialStructrue = spatialStructure};
         dto.AddUsedNames(AppConstants.UnallowedNames);
         dto.AddUsedNames(_unallowedNames);
         return dto;
      }

      public IEnumerable<IMoleculeBuildingBlock> GetMolecules()
      {
         return _context.CurrentProject.MoleculeBlockCollection;
      }

      public IEnumerable<IMoBiSpatialStructure> GetSpatialStructures()
      {
         return _context.CurrentProject.SpatialStructureCollection;
      }
   }

   public class CreateMoleculeStartValuesPresenter : CreateStartValuesPresenter<IMoleculeStartValuesBuildingBlock>
   {
      private readonly IMoleculeStartValuesCreator _startValuesCreator;

      public CreateMoleculeStartValuesPresenter(ICreateStartValuesView view, IMoBiContext context, IMoleculeStartValuesCreator startValuesCreator) : base(view, context)
      {
         _startValuesCreator = startValuesCreator;
         _unallowedNames.AddRange(_context.CurrentProject.MoleculeStartValueBlockCollection.Select(x => x.Name));
         view.ApplicationIcon = ApplicationIcons.MoleculeStartValues;
         view.Caption = AppConstants.Captions.NewMoleculeStartValues;
      }

      protected override IMoleculeStartValuesBuildingBlock CreateStartValuesFromDTO(StartValuesDTO dto)
      {
         return _startValuesCreator.CreateFrom(dto.SpatialStructrue, dto.Molecules).WithName(dto.Name);
      }
   }

   public class CreateParameterStartValuesPresenter : CreateStartValuesPresenter<IParameterStartValuesBuildingBlock>
   {
      private readonly IParameterStartValuesCreator _startValuesCreator;

      public CreateParameterStartValuesPresenter(ICreateStartValuesView view, IMoBiContext context, IParameterStartValuesCreator startValuesCreator)
         : base(view, context)
      {
         _startValuesCreator = startValuesCreator;
         _unallowedNames.AddRange(_context.CurrentProject.ParametersStartValueBlockCollection.Select(x => x.Name));
         view.ApplicationIcon = ApplicationIcons.ParameterStartValues;
         view.Caption = AppConstants.Captions.NewParameterStartValues;
      }

      protected override IParameterStartValuesBuildingBlock CreateStartValuesFromDTO(StartValuesDTO dto)
      {
         return _startValuesCreator.CreateFrom(dto.SpatialStructrue, dto.Molecules).WithName(dto.Name);
      }
   }
}