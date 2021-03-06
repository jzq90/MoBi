﻿using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using Parameter = libsbmlcs.Parameter;
using SBMLModel = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
    public class ParameterImporter : SBMLImporter
    {
        private readonly List<IEntity> _paramList;

        public ParameterImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context)
            : base(objectPathFactory, objectBaseFactory, astHandler, context)
        {
            _paramList = new List<IEntity>();
        }

        protected override void Import(SBMLModel model)
        {
            for (long i = 0; i < model.getNumParameters(); i++)
            {
                _paramList.Add(CreateParameter(model.getParameter(i)));
            }
            AddToProject();
        }

        /// <summary>
        ///     Creates a MoBi Parameter from a given SBML Parameter.
        /// </summary>
        public IFormulaUsable CreateParameter(Parameter sbmlParameter)
        {
            var value = 0.0;
            if (sbmlParameter.isSetValue()) value = sbmlParameter.getValue();
            IFormula formula = ObjectBaseFactory.Create<ConstantFormula>().WithValue(value);

            var parameter = ObjectBaseFactory.Create<IParameter>()             
                .WithName(sbmlParameter.getId())
                .WithFormula(formula);

            if (!sbmlParameter.isSetUnits()) return parameter;

            if (_sbmlInformation.MobiDimension.ContainsKey(sbmlParameter.getUnits()))
                parameter.Dimension = _sbmlInformation.MobiDimension[sbmlParameter.getUnits()];
            return parameter;
        }

        /// <summary>
        ///     Adds the created MoBi Parameters to the TopContainer.
        /// </summary>
        public override void AddToProject()
        {
            var topContainer = GetMainTopContainer();
            foreach (var param in _paramList)
               topContainer.Add(param);
        }
    }
}
