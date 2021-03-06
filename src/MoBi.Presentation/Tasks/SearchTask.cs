﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public interface ISearchTask
   {
      IEnumerable<SearchResult> StartSearch(SearchOptions options, IObjectBase localSearchTarget);
   }

   public class SearchTask : ISearchTask
   {
      private readonly ISearchVisitor _searchVisitor;
      private readonly IMoBiContext _context;

      public SearchTask(ISearchVisitor searchVisitor, IMoBiContext context)
      {
         _searchVisitor = searchVisitor;
         _context = context;
      }

      public IEnumerable<SearchResult> StartSearch(SearchOptions options, IObjectBase localSearchTarget)
      {
         if (options.Expression.IsNullOrEmpty())
            return Enumerable.Empty<SearchResult>();

         _searchVisitor.SearchFor = options.Expression;
         _searchVisitor.RegExSearch = options.RegEx;
         _searchVisitor.WholeWord = options.WholeWord;
         _searchVisitor.CaseSensitive = options.CaseSensitive;

         var project = _context.CurrentProject;

         switch (options.Scope)
         {
            case SearchScope.Project:
               return _searchVisitor.SearchIn(project, project);

            case SearchScope.AllOfSameType:
               if (localSearchTarget == null)
                  return Enumerable.Empty<SearchResult>();

               return searchInAllOfSameType(localSearchTarget.GetType(), project);
            case SearchScope.Local:
               if (localSearchTarget == null)
                  return Enumerable.Empty<SearchResult>();

               return _searchVisitor.SearchIn(localSearchTarget, project);
            default:
               throw new ArgumentOutOfRangeException(nameof(options.Scope));
         }

      }

      private IEnumerable<SearchResult> searchInAllOfSameType(Type buildingBlockType, IMoBiProject project)
      {
         var result = new List<SearchResult>();
         var buildingBlocks = getBuildingBlocksOfType(buildingBlockType);
         buildingBlocks.Each(buildingBlock => result.AddRange(_searchVisitor.SearchIn(buildingBlock, project)));
         return result;
      }

      private IEnumerable<IObjectBase> getBuildingBlocksOfType(Type buildingBlockType)
      {
         if (buildingBlockType.IsAnImplementationOf<IModelCoreSimulation>())
            return _context.CurrentProject.Simulations;

         if (buildingBlockType.IsAnImplementationOf<IMoleculeBuildingBlock>())
            return _context.CurrentProject.MoleculeBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<IReactionBuildingBlock>())
            return _context.CurrentProject.ReactionBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<ISpatialStructure>())
            return _context.CurrentProject.SpatialStructureCollection;

         if (buildingBlockType.IsAnImplementationOf<IObserverBuildingBlock>())
            return _context.CurrentProject.ObserverBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<IEventGroupBuildingBlock>())
            return _context.CurrentProject.EventBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<IMoleculeStartValuesBuildingBlock>())
            return _context.CurrentProject.MoleculeStartValueBlockCollection;

         if (buildingBlockType.IsAnImplementationOf<IParameterStartValuesBuildingBlock>())
            return _context.CurrentProject.ParametersStartValueBlockCollection;

         return null;
      }
   }
}