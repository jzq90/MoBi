﻿using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Service
{
   public abstract class concern_for_BuildingBlockRetriever : ContextSpecification<IBuildingBlockRetriever>
   {
      private IBuildingBlockRepository _buildingBlockRepository;
      protected List<IBuildingBlock> _allBuildingBlocks;

      protected override void Context()
      {
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new BuildingBlockRetriever(_buildingBlockRepository);
         _allBuildingBlocks = new List<IBuildingBlock>();
         A.CallTo(() => _buildingBlockRepository.All()).Returns(_allBuildingBlocks);
      }
   }

   internal class When_asking_building_block_retriever_to_get_a_buiding_block_for_an_objectbase_and_no_building_block_can_be_found : concern_for_BuildingBlockRetriever
   {
      [Observation]
      public void should_throw_an_argument_exception()
      {
         The.Action(() => sut.GetBuildingBlockFor(A.Fake<IObjectBase>(), A.Fake<IBuildingBlock>())).ShouldThrowAn<ArgumentException>();
      }
   }

   internal class When_asking_building_block_retriever_to_get_a_buiding_block_for_an_objectbase : concern_for_BuildingBlockRetriever
   {
      private IList<IBuildingBlock> _returnedBuildingBlocks;
      private IReactionBuilder _childReactionBuilder;

      private IReactionBuildingBlock _reactionBuildingBlock;
      private IMoleculeBuilder _moleculeBuilder;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private IObserverBuilder _obseverBuilder;
      private IObserverBuildingBlock _observerBuildingBlock;
      private ITransportBuilder _passiveTranportBuilder;
      private IPassiveTransportBuildingBlock _passiveTranportBuildingBlock;
      private IApplicationBuilder _applicationBuilder;
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
      private IParameter _parameter;
      private IMoBiSpatialStructure _spatialStructure;
      private IObjectBaseFactory _objectBaseFactory;
      private IFormula _formula;
      private ParameterStartValue _parameterStartValue;
      private ParameterStartValuesBuildingBlock _parameterStartValueBuildingBlock;
      private MoleculeStartValue _moleculeStartValue;
      private MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      private IParameterFactory _parmaeterFactory;

      protected override void Context()
      {
         base.Context();
         _returnedBuildingBlocks = new List<IBuildingBlock>();
         _childReactionBuilder = new ReactionBuilder().WithName("Test").WithId("FindME");
         _reactionBuildingBlock = new MoBiReactionBuildingBlock() { _childReactionBuilder };
         _allBuildingBlocks.Add(_reactionBuildingBlock);
         _moleculeBuilder = new MoleculeBuilder();
         _moleculeBuildingBlock = new MoleculeBuildingBlock() { _moleculeBuilder };
         _allBuildingBlocks.Add(_moleculeBuildingBlock);
         _obseverBuilder = new ObserverBuilder();
         _observerBuildingBlock = new ObserverBuildingBlock() { _obseverBuilder };
         _allBuildingBlocks.Add(_observerBuildingBlock);
         _passiveTranportBuilder = new TransportBuilder();
         _passiveTranportBuildingBlock = new PassiveTransportBuildingBlock() { _passiveTranportBuilder };
         _allBuildingBlocks.Add(_passiveTranportBuildingBlock);
         _applicationBuilder = new ApplicationBuilder();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock() { _applicationBuilder };
         _allBuildingBlocks.Add(_eventGroupBuildingBlock);
         _parameter = new Parameter().WithName("Para");
         var container = new Container().WithName("Cont");
         container.Add(_parameter);
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _parmaeterFactory = A.Fake<IParameterFactory>();
         A.CallTo(() => _objectBaseFactory.Create<IContainer>()).Returns(A.Fake<IContainer>());
         A.CallTo(() => _objectBaseFactory.Create<IMoBiSpatialStructure>()).Returns(new MoBiSpatialStructure());
         var diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         _spatialStructure = new MoBiSpatialStructureFactory(_objectBaseFactory, _parmaeterFactory, A.Fake<IconRepository>(), diagramManagerFactory).Create().DowncastTo<IMoBiSpatialStructure>();
         _spatialStructure.AddTopContainer(container);
         _allBuildingBlocks.Add(_spatialStructure);
         _formula = new ExplicitFormula();
         _moleculeBuildingBlock.AddFormula(_formula);
         _parameterStartValue = new ParameterStartValue { Path = new ObjectPath { "test" }, StartValue = 1, Dimension = A.Fake<IDimension>() };
         _parameterStartValueBuildingBlock = new ParameterStartValuesBuildingBlock() { _parameterStartValue };
         _allBuildingBlocks.Add(_parameterStartValueBuildingBlock);
         _moleculeStartValue = new MoleculeStartValue { ContainerPath = new ObjectPath { "test" }, Name = "drug" };
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock() { _moleculeStartValue };
         _allBuildingBlocks.Add(_moleculeStartValuesBuildingBlock);
      }

      protected override void Because()
      {
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_childReactionBuilder, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_moleculeBuilder, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_obseverBuilder, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_passiveTranportBuilder, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_applicationBuilder, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_parameter, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_formula, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_parameterStartValue, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_moleculeStartValue, null));
      }

      [Observation]
      public void should_return_the_containing_building_block()
      {
         _returnedBuildingBlocks.ShouldOnlyContainInOrder(_reactionBuildingBlock, _moleculeBuildingBlock, _observerBuildingBlock, _passiveTranportBuildingBlock, _eventGroupBuildingBlock, _spatialStructure, _moleculeBuildingBlock, _parameterStartValueBuildingBlock, _moleculeStartValuesBuildingBlock);
      }
   }

   internal class When_asking_building_block_retriever_to_get_a_buiding_block_for_an_parameter : concern_for_BuildingBlockRetriever
   {
      private IList<IBuildingBlock> _returnedBuildingBlocks;
      private IReactionBuilder _childReactionBuilder;

      private IReactionBuildingBlock _reactionBuildingBlock;
      private IMoleculeBuilder _moleculeBuilder;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private ITransportBuilder _passiveTranportBuilder;
      private IPassiveTransportBuildingBlock _passiveTranportBuildingBlock;
      private IApplicationBuilder _applicationBuilder;
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
      private IParameter _parameter;
      private IMoBiSpatialStructure _spatialStructure;
      private IObjectBaseFactory _objectBaseFactory;
      private IFormula _formula;
      private IParameter _moleculeParameter;
      private Parameter _reactionParameter;
      private Parameter _passiveTransportParameter;
      private IApplicationBuilder _applicationBuilderParameter;
      private IParameterFactory _parameterFactory;

      protected override void Context()
      {
         base.Context();
         _returnedBuildingBlocks = new List<IBuildingBlock>();
         _childReactionBuilder = new ReactionBuilder().WithName("Test").WithId("FindME");
         _reactionParameter = new Parameter().WithName("Para").WithId("ReactionPara");
         _childReactionBuilder.AddParameter(_reactionParameter);
         _reactionBuildingBlock = new MoBiReactionBuildingBlock() { _childReactionBuilder };
         _allBuildingBlocks.Add(_reactionBuildingBlock);
         _moleculeBuilder = new MoleculeBuilder();
         _moleculeParameter = new Parameter().WithName("para");
         _moleculeBuilder.AddParameter(_moleculeParameter);
         _moleculeBuildingBlock = new MoleculeBuildingBlock() { _moleculeBuilder };
         _allBuildingBlocks.Add(_moleculeBuildingBlock);
         _passiveTranportBuilder = new TransportBuilder();
         _passiveTransportParameter = new Parameter().WithName("PTParameter");
         _passiveTranportBuilder.AddParameter(_passiveTransportParameter);
         _passiveTranportBuildingBlock = new PassiveTransportBuildingBlock() { _passiveTranportBuilder };
         _allBuildingBlocks.Add(_passiveTranportBuildingBlock);
         _applicationBuilder = new ApplicationBuilder();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock() { _applicationBuilder };
         _applicationBuilderParameter = new ApplicationBuilder().WithName("AppParameter");
         _applicationBuilder.Add(_applicationBuilderParameter);
         _allBuildingBlocks.Add(_eventGroupBuildingBlock);
         _parameter = new Parameter().WithName("Para");
         var container = new Container().WithName("Cont");
         container.Add(_parameter);
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         A.CallTo(() => _objectBaseFactory.Create<IContainer>()).Returns(new Container());
         A.CallTo(() => _objectBaseFactory.Create<IMoBiSpatialStructure>()).Returns(new MoBiSpatialStructure());
         var diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         _spatialStructure = new MoBiSpatialStructureFactory(_objectBaseFactory, _parameterFactory, A.Fake<IIconRepository>(), diagramManagerFactory).Create() as IMoBiSpatialStructure;
         _spatialStructure.AddTopContainer(container);
         _allBuildingBlocks.Add(_spatialStructure);
         _formula = new ExplicitFormula();
         _moleculeBuildingBlock.AddFormula(_formula);
      }

      protected override void Because()
      {
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_reactionParameter, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_moleculeParameter, null));

         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_passiveTransportParameter, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_applicationBuilderParameter, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_parameter, null));
         _returnedBuildingBlocks.Add(sut.GetBuildingBlockFor(_formula, null));
      }

      [Observation]
      public void should_return_the_containing_building_block()
      {
         _returnedBuildingBlocks.ShouldOnlyContainInOrder(_reactionBuildingBlock, _moleculeBuildingBlock, _passiveTranportBuildingBlock, _eventGroupBuildingBlock, _spatialStructure, _moleculeBuildingBlock);
      }
   }

   public class When_retrieving_the_building_block_for_a_formula_defined_in_a_molecule_start_values : concern_for_BuildingBlockRetriever
   {
      private IFormula _formula;
      private IMoleculeStartValuesBuildingBlock _moleculeStartValues;
      private IMoleculeStartValuesBuildingBlock _moleculeStartValuesTemplate;
      private ExplicitFormula _templateFormula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula().WithName("F1").WithId("F11");
         _templateFormula = new ExplicitFormula().WithName("F1").WithId("F12");
         _moleculeStartValues = new MoleculeStartValuesBuildingBlock().WithName("MSV");
         _moleculeStartValuesTemplate = new MoleculeStartValuesBuildingBlock().WithName("MSV");
         _moleculeStartValuesTemplate.AddFormula(_templateFormula);
         _allBuildingBlocks.Add(_moleculeStartValuesTemplate);
      }

      [Observation]
      public void should_return_the_building_block_with_the_same_name_as_the_given_building_block_in_the_project()
      {
         sut.GetBuildingBlockFor(_formula, _moleculeStartValues).ShouldBeEqualTo(_moleculeStartValuesTemplate);
      }

      [Observation]
      public void should_return_null_if_the_building_block_is_not_found_with_the_same_name()
      {
         _moleculeStartValues.Name = "ANOTHER NAME";
         sut.GetBuildingBlockFor(_formula, _moleculeStartValues).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_if_the_building_block_is_found_with_the_same_name_but_the_formula_is_not()
      {
         _formula.Name = "ANOTHER_NAME";
         sut.GetBuildingBlockFor(_formula, _moleculeStartValues).ShouldBeNull();
      }
   }
}