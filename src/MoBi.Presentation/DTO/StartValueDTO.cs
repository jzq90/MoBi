using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public interface IStartValueDTO : IBreadCrumbsDTO, IWithDisplayUnitDTO, IWithValueOrigin
   {
   }

   public abstract class StartValueDTO<T> : BreadCrumbsDTO<T>, IStartValueDTO where T : class, IStartValue
   {
      private static readonly string _containerPathPropertyName;
      private static readonly string _formulaPropertyName;
      private StartValueFormulaDTO _formula;
      private readonly IStartValuesBuildingBlock<T> _buildingBlock;
      public T StartValueObject { get; private set; }

      static StartValueDTO()
      {
         _containerPathPropertyName = MoBiReflectionHelper.PropertyName<IStartValue>(x => x.ContainerPath);
         _formulaPropertyName = MoBiReflectionHelper.PropertyName<IStartValue>(x => x.Formula);
      }

      public string Name
      {
         get => StartValueObject.Name;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      /// <summary>
      ///    Updates the name of the start value
      /// </summary>
      /// <param name="newName">The new name for the start value</param>
      public abstract void UpdateStartValueName(string newName);

      protected StartValueDTO(T startValueObject, IStartValuesBuildingBlock<T> buildingBlock)
         : base(startValueObject)
      {
         StartValueObject = startValueObject;
         StartValueObject.PropertyChanged += underlyingObjectOnPropertyChanged;
         _buildingBlock = buildingBlock;

         Rules.AddRange(AllRules.All());
      }

      public IDimension Dimension
      {
         get => StartValueObject.Dimension;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public Unit DisplayUnit
      {
         get => StartValueObject.DisplayUnit;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }


      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         StartValueObject.UpdateValueOriginFrom(ValueOrigin);
      }

      public virtual ValueOrigin ValueOrigin
      {
         get => StartValueObject.ValueOrigin;
         set => UpdateValueOriginFrom(value);
      }


      public IEnumerable<Unit> AllUnits
      {
         get => Dimension.Units;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public StartValueFormulaDTO Formula
      {
         get => _formula;
         set
         {
            _formula = value;
            OnPropertyChanged(() => Formula);
         }
      }

      public double? StartValue
      {
         get
         {
            if (Formula == null || Formula.Formula == null)
               return StartValueObject.ConvertToDisplayUnit(StartValueObject.StartValue);
            return double.NaN;
         }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      private void underlyingObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
      {
         var changedProperty = propertyChangedEventArgs.PropertyName;
         if (changedProperty.Equals(_containerPathPropertyName))
         {
            ContainerPath = StartValueObject.ContainerPath;
         }
         else if (changedProperty.Equals(_formulaPropertyName))
         {
            Formula = StartValueObject.Formula.IsExplicit() ? new StartValueFormulaDTO(StartValueObject.Formula as ExplicitFormula) : new EmptyFormulaDTO();
         }
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All()
         {
            yield return renameMustNotCauseCollision();
            yield return mustNotAlreadyContainStartValue(x => x.PathElement0);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement1);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement2);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement3);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement4);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement5);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement6);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement7);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement8);
            yield return mustNotAlreadyContainStartValue(x => x.PathElement9);


            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement1);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement2);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement3);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement4);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement5);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement6);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement7);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement8);
            yield return mustHaveAllPreviousPathElementsSet(x => x.PathElement9);
         }

         private static IBusinessRule renameMustNotCauseCollision()
         {
            return CreateRule.For<StartValueDTO<T>>()
               .Property(x => x.Name).WithRule(namedOnlyElementDoesNotExist)
               .WithError((dto, name) => AppConstants.Validation.NameIsAlreadyUsedInThisContainer(dto.ContainerPath.ToString(), name));
         }

         private static bool namedOnlyElementDoesNotExist(StartValueDTO<T> dto, string name)
         {
            return !dto._buildingBlock.Any(sv =>
               string.Equals(sv.Name, name) && sv.ContainerPath.Equals(dto.ContainerPath) && sv != dto.StartValueObject);
         }

         private static IBusinessRule mustHaveAllPreviousPathElementsSet(Expression<Func<StartValueDTO<T>, string>> propertyToCheck)
         {
            var index = getPathIndex(propertyToCheck);
            return CreateRule.For<StartValueDTO<T>>()
               .Property(propertyToCheck).WithRule((dto, pathElement) => areAllPreviousPathElementsNonEmpty(dto, pathElement, index))
               .WithError((dto, pathElement) => AppConstants.Validation.CannotSetPathElementWhenPreviousElementsAreEmpty);
         }

         private static bool areAllPreviousPathElementsNonEmpty(StartValueDTO<T> dto, string pathElement, int index)
         {
            // to set a path element to empty, there is no requirement for the preceeding elements
            if (string.IsNullOrEmpty(pathElement))
               return true;

            for (var i = index - 1; i >= 0; i--)
            {
               if (string.IsNullOrEmpty(dto.PathElementByIndex(i)))
                  return false;
            }
            return true;
         }

         private static IBusinessRule mustNotAlreadyContainStartValue(Expression<Func<StartValueDTO<T>, string>> propertyToCheck)
         {
            var index = getPathIndex(propertyToCheck);
            return CreateRule.For<StartValueDTO<T>>()
               .Property(propertyToCheck)
               .WithRule((dto, pathElement) => noDuplicatesInBuildingBlockRule(dto, pathElement, index))
               .WithError((dto, pathElement) => AppConstants.Validation.PathIsIdenticalToExistingPath(newPathWithReplacement(dto, pathElement, index)));
         }

         private static int getPathIndex(Expression<Func<StartValueDTO<T>, string>> propertyToCheck)
         {
            var propertyName = propertyToCheck.PropertyInfo().Name;
            var index = int.Parse(propertyName.Last().ToString(CultureInfo.InvariantCulture));
            return index;
         }

         private static bool noDuplicatesInBuildingBlockRule(StartValueDTO<T> dto, string pathElement, int index)
         {
            var containerPath = newPathWithReplacement(dto, pathElement, index);

            var path = containerPath.Clone<IObjectPath>().AndAdd(dto.Name);

            if (dto._buildingBlock.Any(sv => sv.Path.Equals(path) && sv != dto.StartValueObject))
               return false;

            return true;
         }

         private static IObjectPath newPathWithReplacement(StartValueDTO<T> dto, string pathElement, int index)
         {
            var containerPath = dto.ContainerPath.Clone<IObjectPath>();

            // This means we don't have enough elements to do a replacement
            if (containerPath.Count > index)
               if (!pathElement.IsNullOrEmpty())
                  containerPath[index] = pathElement;
               else
                  containerPath.RemoveAt(index);

            // We are appending to the end of the list of elements
            else if (containerPath.Count == index)
               containerPath.Add(pathElement);
            return containerPath;
         }
      }
   }
}