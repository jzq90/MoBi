using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IRootContextMenuFor<TParent, TChild> : IContextMenu where TChild : IObjectBase
   {
      IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter);
      IContextMenu InitializeWith(IPresenter presenter);
   }

   public class RootContextMenuFor<TParent, TObjectBase> : ContextMenuBase, IRootContextMenuFor<TParent, TObjectBase>
      where TObjectBase : class, IObjectBase
      where TParent : class
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      protected readonly IList<IMenuBarItem> _allMenuItems;
      protected readonly IMoBiContext _context;

      public RootContextMenuFor(IObjectTypeResolver objectTypeResolver, IMoBiContext context)
      {
         _objectTypeResolver = objectTypeResolver;
         _context = context;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      protected IMenuBarItem CreateAddNewItemFor(TParent parent)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypeName))
            .WithIcon(ApplicationIcons.AddIconFor(typeof (TObjectBase).Name))
            .WithCommandFor<AddNewCommandFor<TParent, TObjectBase>, TParent>(parent);
      }

      protected IMenuBarItem CreateAddExistingItemFor(TParent parent)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypeName))
            .WithIcon(ApplicationIcons.LoadIconFor(typeof (TObjectBase).Name))
            .WithCommandFor<AddExistingCommandFor<TParent, TObjectBase>, TParent>(parent);
      }

      protected IMenuBarItem CreateAddExistingFromTemplateItemFor(TParent parent)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypeName))
            .WithIcon(ApplicationIcons.LoadTemplateIconFor(typeof(TObjectBase).Name))
            .WithCommandFor<AddExistingFromTemplateCommandFor<TParent, TObjectBase>, TParent>(parent);
      }


      protected virtual IMenuBarItem CreateReportItemForCollection()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportToPDF)
            .WithCommand<ExportCollectionToPDFCommand<TObjectBase>>()
            .WithIcon(ApplicationIcons.ExportToPDF);
      }

      protected string ObjectTypeName
      {
         get { return _objectTypeResolver.TypeFor<TObjectBase>(); }
      }

      public virtual IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         return InitializeWith(presenter);
      }

      public virtual IContextMenu InitializeWith(IPresenter presenter)
      {
         var subjectPresenter = presenter as ISubjectPresenter;
         if (subjectPresenter != null)
         {
            createAddItems(subjectPresenter.Subject as TParent);
         }
         else
         {
            if (typeof (TObjectBase).IsAnImplementationOf<IBuildingBlock>())
            {
               createAddItems(_context.CurrentProject as TParent);
            }
         }
         _allMenuItems.Add(CreateReportItemForCollection());

         return this;
      }

      private void createAddItems(TParent parent)
      {
         _allMenuItems.Add(CreateAddNewItemFor(parent));
         _allMenuItems.Add(CreateAddExistingItemFor(parent));
         _allMenuItems.Add(CreateAddExistingFromTemplateItemFor(parent));
      }
   }
}