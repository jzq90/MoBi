﻿using System;
using System.IO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Helpers
{
   public static class DomainHelperForSpecs
   {
      static DomainHelperForSpecs()
      {
         TimeDimension.AddUnit(new Unit("seconds", 1 / 60.0, 0.0));
         AmountDimension.AddUnit(new Unit("mmol", 1000.0, 0.0));
      }

      /// <summary>
      ///    Returns the full path of the test data file
      /// </summary>
      /// <param name="fileName">The filename and extension</param>
      /// <returns>The full path including the name and extension</returns>
      public static string TestFileFullPath(string fileName)
      {
         var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "TestFiles");
         return Path.Combine(dataFolder, fileName);
      }

      public static IDimension AmountDimension { get; } = new Dimension(new BaseDimensionRepresentation {AmountExponent = 1}, Constants.Dimension.AMOUNT, "µmol");

      public static IDimension ConcentrationDimension { get; } = new Dimension(new BaseDimensionRepresentation {LengthExponent = -3, MassExponent = 1, TimeExponent = -1}, Constants.Dimension.MOLAR_CONCENTRATION, "µmol/l");

      public static IDimension FractionDimension { get; } = new Dimension(new BaseDimensionRepresentation() , Constants.Dimension.FRACTION, "");

      public static IDimension ConcentrationPerTimeDimension { get; } = new Dimension(new BaseDimensionRepresentation {LengthExponent = -3, AmountExponent = 1, TimeExponent = -1}, Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME, "µmol/l/min");

      public static IDimension AmountPerTimeDimension { get; } = new Dimension(new BaseDimensionRepresentation {AmountExponent = 1, TimeExponent = -1}, Constants.Dimension.AMOUNT_PER_TIME, "µmol/min");

      public static IDimension TimeDimension { get; } = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, Constants.Dimension.TIME, "min");
   }

   public class ObjectPathFactoryForSpecs : ObjectPathFactory
   {
      public ObjectPathFactoryForSpecs() : base(new AliasCreator())
      {
      }
   }

   public class EntityPathResolverForSpecs : EntityPathResolver
   {
      public EntityPathResolverForSpecs() : base(new ObjectPathFactoryForSpecs())
      {
      }
   }

   public class PathCacheForSpecs<T> : PathCache<T> where T : class, IEntity
   {
      public PathCacheForSpecs() : base(new EntityPathResolverForSpecs())
      {
      }
   }
}