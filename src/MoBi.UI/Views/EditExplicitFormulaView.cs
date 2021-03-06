﻿using System;
using DevExpress.XtraEditors.DXErrorProvider;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditExplicitFormulaView : BaseUserControl, IEditExplicitFormulaView
   {
      private IEditExplicitFormulaPresenter _presenter;
      private readonly ScreenBinder<ExplicitFormulaBuilderDTO> _screenBinder;
      private readonly DXErrorProvider _warningProvider;
      private bool _readOnly;

      public EditExplicitFormulaView()
      {
         InitializeComponent();
         _warningProvider = new DXErrorProvider(this);
         _screenBinder = new ScreenBinder<ExplicitFormulaBuilderDTO>();
      }

      public void AttachPresenter(IEditExplicitFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(ExplicitFormulaBuilderDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }

      private async void formulaStringChanging(EventArgs e)
      {
         await txtFormulaString.Debounce(formulaStringChanged);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(item => item.FormulaString)
            .To(txtFormulaString)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetFormulaString(e.NewValue));

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         txtFormulaString.TextChanged += (o, e) => OnEvent(formulaStringChanging, e);

         ReadOnly = false;
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

      private void formulaStringChanged()
      {
         _presenter.Validate(txtFormulaString.Text);
      }

      public void SetValidationMessage(string parserError)
      {
         if (string.IsNullOrEmpty(parserError))
            _warningProvider.SetError(txtFormulaString, null);
         else
            _warningProvider.SetError(txtFormulaString, parserError, ErrorType.Critical);

         _presenter.ViewChanged();
      }

      public void SetFormulaCaption(string caption)
      {
         if (string.IsNullOrEmpty(caption))
            layoutItemFormulaString.TextVisible = false;
         else
         {
            layoutItemFormulaString.TextVisible = true;
            layoutItemFormulaString.Text = caption;
         }
      }

      public void HideFormulaCaption()
      {
         SetFormulaCaption(string.Empty);
      }

      public void AddFormulaPathListView(IView view)
      {
         panelReferencePaths.FillWith(view);
      }

      public bool ReadOnly
      {
         get => _readOnly;
         set
         {
            _readOnly = value;
            txtFormulaString.Enabled = !_readOnly;
         }
      }
   }
}