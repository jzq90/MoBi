﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using IEntityContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.Presenter
{
   public interface IEditApplicationBuilderPresenter : IEditPresenterWithParameters<IApplicationBuilder>, IPresenter<IEditApplicationBuilderView>,
      ICanEditPropertiesPresenter, IPresenterWithFormulaCache, IPresenterWithContextMenu<IViewItem>,
      IListener<AddedEvent>, IListener<RemovedEvent>
   {
      void AddApplicationMolecule();
      void RemoveApplicationMolecule(ApplicationMoleculeBuilderDTO dtoApplicationMoleculeBuilder);
      void SelectRelativeContainerPath(ApplicationMoleculeBuilderDTO applicationMoleculeBuilderDTO);
      void SetPropertyValueFor<T>(ApplicationMoleculeBuilderDTO dto, string propertyName, T newValue, T oldValue);
      IEnumerable<string> GetMoleculeNamesWithSelf();
      void UpdateFormula(ApplicationMoleculeBuilderDTO applicationMoleculeBuilderDTO, FormulaBuilderDTO newFormulaDTO);
      void SetRelativeContainerPath(ApplicationMoleculeBuilderDTO applicationMoleculeBuilderDTO, string newRelativeContainerPath);
   }

   internal class EditApplicationBuilderPresenter : AbstractEntityEditPresenter<IEditApplicationBuilderView, IEditApplicationBuilderPresenter, IApplicationBuilder>, IEditApplicationBuilderPresenter
   {
      private IApplicationBuilder _applicationBuilder;
      private readonly IEditTaskFor<IApplicationBuilder> _editTasks;
      private readonly IApplicationBuilderToApplicationBuilderDTOMapper _applicationBuilderToDTOApllicationBuilderMapper;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IInteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder> _interactionTasksForApplicationMoleculeBuilder;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private readonly IMoBiContext _context;
      private readonly IDescriptorConditionListPresenter<IApplicationBuilder> _descriptorConditionListPresenter;
      private readonly IApplicationController _applicationController;
      private readonly string _formulaPropertyName;

      public EditApplicationBuilderPresenter(IEditApplicationBuilderView view, IEditTaskFor<IApplicationBuilder> editTasks, 
         IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IApplicationBuilderToApplicationBuilderDTOMapper applicationBuilderToDTOApllicationBuilderMapper, 
         IInteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder> interactionTasksForApplicationMoleculeBuilder,
         IViewItemContextMenuFactory viewItemContextMenuFactory, 
         IEditParametersInContainerPresenter editParametersInContainerPresenter, IMoBiContext context,
         IDescriptorConditionListPresenter<IApplicationBuilder> descriptorConditionListPresenter, IApplicationController applicationController)
         : base(view)
      {
         _descriptorConditionListPresenter = descriptorConditionListPresenter;
         _applicationController = applicationController;
         _context = context;
         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _view.SetParametersView(_editParametersInContainerPresenter.BaseView);
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _interactionTasksForApplicationMoleculeBuilder = interactionTasksForApplicationMoleculeBuilder;
         _applicationBuilderToDTOApllicationBuilderMapper = applicationBuilderToDTOApllicationBuilderMapper;
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _editTasks = editTasks;
         _view.AddDescriptorConditionListView(_descriptorConditionListPresenter.View);
         _formulaPropertyName = MoBiReflectionHelper.PropertyName<IApplicationMoleculeBuilder>(x => x.Formula);

         AddSubPresenters(_editParametersInContainerPresenter, _descriptorConditionListPresenter);
      }

      public override void Edit(IApplicationBuilder applicationBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _applicationBuilder = applicationBuilder;
         _editParametersInContainerPresenter.BuildingBlock = BuildingBlock;
         _view.EnableDescriptors = (applicationBuilder.ParentContainer == null);
         var dto = _applicationBuilderToDTOApllicationBuilderMapper.MapFrom(applicationBuilder);
         dto.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(applicationBuilder, existingObjectsInParent));
         dto.GetMoleculeNames(GetMoleculeNames);
         _view.BindTo(dto);
         _editParametersInContainerPresenter.Edit(applicationBuilder);
         _descriptorConditionListPresenter.Edit(_applicationBuilder,  x=> x.SourceCriteria, BuildingBlock);
      }

      public override object Subject
      {
         get { return _applicationBuilder; }
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editParametersInContainerPresenter.Select(parameter);
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _applicationBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_applicationBuilder, _applicationBuilder.ParentContainer, BuildingBlock);
      }

      public IBuildingBlock BuildingBlock { get; set; }

      public IFormulaCache FormulaCache
      {
         get { return BuildingBlock.FormulaCache; }
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return BuildingBlock.FormulaCache.MapAllUsing(_formulaToDTOFormulaMapper);
      }

      public void AddApplicationMolecule()
      {
         AddCommand(_interactionTasksForApplicationMoleculeBuilder.AddNew(_applicationBuilder, BuildingBlock));
      }

      public void RemoveApplicationMolecule(ApplicationMoleculeBuilderDTO dtoApplicationMoleculeBuilder)
      {
         var applicationMoleculeBuilder = _context.Get<IApplicationMoleculeBuilder>(dtoApplicationMoleculeBuilder.Id);
         AddCommand(_interactionTasksForApplicationMoleculeBuilder.Remove(applicationMoleculeBuilder, _applicationBuilder, BuildingBlock));
      }

      public void SelectRelativeContainerPath(ApplicationMoleculeBuilderDTO applicationMoleculeBuilderDTO)
      {
         var applicationMoleculeBuilder = applicationMoleculeBuilderFrom(applicationMoleculeBuilderDTO);

         using (var presenter = _applicationController.Start<ISelectFormulaUsablePathPresenter>())
         {
            var selectionPresenter = _applicationController.Start<ISelectReferencePresenterAtApplicationBuilder>();
            presenter.Init(ob => ob.IsAnImplementationOf<IEntityContainer>(), _applicationBuilder, _applicationBuilder.RootContainer, AppConstants.Captions.RelativeContainerPath, selectionPresenter);
            var path = presenter.GetSelection();
            if (path == null)
               return;

            updateRelativeContainerPath(applicationMoleculeBuilder, path);
            applicationMoleculeBuilderDTO.RelativeContainerPath = path.ToString();
         }
      }

      private void updateRelativeContainerPath(IApplicationMoleculeBuilder applicationMoleculeBuilder, IObjectPath path)
      {
         AddCommand(new EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand(applicationMoleculeBuilder, path, BuildingBlock).Run(_context));
      }

      public void UpdateFormula(ApplicationMoleculeBuilderDTO applicationMoleculeBuilderDTO, FormulaBuilderDTO newFormulaDTO)
      {
         var applicationMoleculeBuilder = _context.Get<IApplicationMoleculeBuilder>(applicationMoleculeBuilderDTO.Id);
         var newFormula = FormulaCache.FindById(newFormulaDTO.Id);
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(_formulaPropertyName, newFormula, applicationMoleculeBuilder.Formula, applicationMoleculeBuilder, BuildingBlock).Run(_context));
      }

      public void SetRelativeContainerPath(ApplicationMoleculeBuilderDTO applicationMoleculeBuilderDTO, string newRelativeContainerPath)
      {
         var applicationMoleculeBuilder = applicationMoleculeBuilderFrom(applicationMoleculeBuilderDTO);
         updateRelativeContainerPath(applicationMoleculeBuilder, new ObjectPath(newRelativeContainerPath.ToPathArray()));
      }

      public void SetPropertyValueFor<T>(ApplicationMoleculeBuilderDTO dto, string propertyName, T newValue, T oldValue)
      {
         var applicationMoleculeBuilder = applicationMoleculeBuilderFrom(dto);
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, applicationMoleculeBuilder, BuildingBlock).Run(_context));
      }

      private IApplicationMoleculeBuilder applicationMoleculeBuilderFrom(ApplicationMoleculeBuilderDTO dto)
      {
         return _context.Get<IApplicationMoleculeBuilder>(dto.Id);
      }

      public IEnumerable<string> GetMoleculeNames()
      {
         IEnumerable<string> names = new HashSet<string>();
         return _context.CurrentProject.MoleculeBlockCollection
            .Aggregate(names, (current, moleculeBlock) => current.Union(moleculeBlock.AllNames()))
            .OrderBy(x => x);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public IEnumerable<string> GetMoleculeNamesWithSelf()
      {
         var moleculeNames = new List<string>(GetMoleculeNames());
         if (!moleculeNames.Contains(_applicationBuilder.MoleculeName))
         {
            moleculeNames.Add(_applicationBuilder.MoleculeName);
         }
         return moleculeNames;
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_applicationBuilder == null) return;
         if (eventToHandle.AddedObject.IsAnImplementationOf<IApplicationMoleculeBuilder>())
         {
            Edit(_applicationBuilder);
         }
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_applicationBuilder == null) return;
         if (eventToHandle.RemovedObjects.Any(removedObject => removedObject.IsAnImplementationOf<IApplicationMoleculeBuilder>()))
         {
            Edit(_applicationBuilder);
         }
      }
   }
}