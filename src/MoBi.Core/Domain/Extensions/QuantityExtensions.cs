﻿using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Domain.Extensions
{
   public static class QuantityExtensions
   {
      /// <summary>
      ///    Returns the <see cref="IQuantity" /> that should be edited based on the given <paramref name="quantity" />.
      ///    This is in general the <paramref name="quantity" /> itself. However, for  <see cref="IMoleculeAmount" />, we return
      ///    the
      ///    <see cref="Constants.Parameters.START_VALUE" /> parameter if it is available.
      /// </summary>
      public static IQuantity QuantityToEdit(this IQuantity quantity)
      {
         var moleculeAmount = quantity as IMoleculeAmount;
         if (moleculeAmount == null)
            return quantity;

         var startValue = moleculeAmount.GetSingleChildByName<IQuantity>(Constants.Parameters.START_VALUE);
         return startValue ?? moleculeAmount;
      }

      public static void UpdateQuantityValue(this IQuantity quantity, double valueToSet)
      {
         if (quantity.Formula.IsConstant() && quantity.IsFixedValue == false)
            quantity.Formula.DowncastTo<ConstantFormula>().Value = valueToSet;
         else
            quantity.Value = valueToSet;
      }
   }
}