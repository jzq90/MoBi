using MoBi.Assets;
using MoBi.Presentation.DTO;

namespace MoBi.Presentation.Formatters
{
   public class StartValueFormatter : NullableWithDisplayUnitFormatter
   {
      public override string Format(double? valueToFormat)
      {
         return !valueToFormat.HasValue || double.IsNaN(valueToFormat.Value) ? AppConstants.Captions.StartValueNotAvailable : base.Format(valueToFormat);
      }

      public StartValueFormatter(IStartValueDTO startValueDTO) : base(startValueDTO)
      {
      }
   }
}